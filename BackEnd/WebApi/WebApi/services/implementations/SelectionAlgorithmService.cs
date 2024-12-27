using System.Data;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Global.Utils;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Repository.Implementations;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Services;

/// <summary>
/// This class contains all the methods necessary to analyze a transport and choose the best truck to do it
/// </summary>
public class SelectionAlgorithmService : ISelectionAlgorithmService
{
    /*  -------------------------Services................*/
    private IDistanceService distanceService;

    private IHolidayService holidayService;

    private IScoreService scoreService;

    /*  -------------------------Repositories................*/
    private readonly ITransportRepository transportRepository;

    private readonly ITruckRepository truckRepository;

    private readonly IAbsenceRepository absenceRepository;

    private readonly ITruckBreakDownsRepository truckBreakDownsRepository;

    private readonly ITransportReviewParametersRepository transportReviewParametersRepository;

    private readonly IServiceRepository serviceRepository;


    /*  -------------------------Objects................*/

    private TransportReviewParameters transportReviewParameters = new TransportReviewParameters();

    private IEnumerable<TruckBreakDowns> listTruckBreakDowns = null;

    private DateTime dateTime;

    /// <summary>
    /// This constructor inject the truck repository, the distance service, the transport repository, the service repository, the holiday service, the absence repository and truck breakdowns to be use by the review service.
    /// </summary>
    /// <param name="transportRepository"> transport repository</param>
    /// <param name="distanceService"> distance service</param>
    /// <param name="truckRepository"> truck repository</param>
    /// <param name="truckBreakDownsRepository"> truck break downs repository</param>
    /// <param name="transportReviewParametersRepository"></param>
    /// <param name="holidayService"> holiday service</param>
    /// <param name="serviceRepository">service repository</param>        
    public SelectionAlgorithmService(ITransportRepository transportRepository, IDistanceService distanceService,
        ITruckRepository truckRepository,
        IAbsenceRepository absenceRepository, ITruckBreakDownsRepository truckBreakDownsRepository,
        ITransportReviewParametersRepository transportReviewParametersRepository, IHolidayService holidayService,
        IServiceRepository serviceRepository, IScoreService scoreService)
    {
        this.transportRepository = transportRepository;
        this.truckRepository = truckRepository;
        this.absenceRepository = absenceRepository;
        this.truckBreakDownsRepository = truckBreakDownsRepository;
        this.transportReviewParametersRepository = transportReviewParametersRepository;
        this.serviceRepository = serviceRepository;
        this.scoreService = scoreService;
        this.distanceService = distanceService;
        this.holidayService = holidayService;
    }

    /// <inheritdoc/>
    public async Task<ReviewTransportDto> selectionAndAnalysisAlgorithm(Guid idTransport)
    {
        //Load the parameters for analysis
        await getParametersOfReview();

        var reviewTransportReadDto = await loadDataToDto(idTransport);
        reviewTransportReadDto.description = "None";

        //Chamada API EXTERNA para obter kms
        /*reviewTransportReadDto.kms = await getTotalNumberKmsOfRoute(reviewTransportReadDto.origin,
            reviewTransportReadDto.destiny,
            reviewTransportReadDto.addresseOrganization);*/

        reviewTransportReadDto.kms = 60;

        //Obter todos os camiões por Categoria
        Console.WriteLine("CATEGORIGA " + reviewTransportReadDto.truckCategory);
        var listOfTrucksAvailable = await truckRepository.getTrucksByCategory(reviewTransportReadDto.truckCategory);


        var listTrucks = new List<TruckForReviewReadDto>();


        if (reviewTransportReadDto.truckCategory == TruckCategory.REFRIGERATOR_TRUCK ||
            reviewTransportReadDto.truckCategory == TruckCategory.CONTAINER_TRUCK)
        {
            listTrucks =
                await processInformationAboutTruck(listOfTrucksAvailable,
                    reviewTransportReadDto.kms, reviewTransportReadDto.capacity);

            reviewTransportReadDto.listTrucks = await analyzeTrucks(listTrucks, true);
        }
        else if (reviewTransportReadDto.truckCategory == TruckCategory.CISTERN_TRUCK)
        {
            listTrucks =
                await processInformationAboutTruck(listOfTrucksAvailable,
                    reviewTransportReadDto.kms, reviewTransportReadDto.liters);
            reviewTransportReadDto.listTrucks = await analyzeTrucksWithCategoryCistern(listTrucks, true);
        }
        else if (reviewTransportReadDto.truckCategory == TruckCategory.DUMP_TRUCK)
        {
            listTrucks =
                await processInformationAboutTruck(listOfTrucksAvailable,
                    reviewTransportReadDto.kms, reviewTransportReadDto.weight);
            reviewTransportReadDto.listTrucks = await analyzeTrucks((listTrucks), true);
        }

        DateTime dateOfTransport = Utils.transformStringToData2(reviewTransportReadDto.date);

        reviewTransportReadDto.listTrucks = await calculatesFinalValueByTruck((listTrucks), dateOfTransport);

        reviewTransportReadDto.listTrucks = await sortListTrucksByFitness(reviewTransportReadDto.listTrucks);

        reviewTransportReadDto = await calculateFinalServiceValueAndSelectTruck(reviewTransportReadDto);

        reviewTransportReadDto.listTrucks = await sortListTrucksByFitness(reviewTransportReadDto.listTrucks);

        if (reviewTransportReadDto.available && !reviewTransportReadDto.thinksTwice)
        {
            return reviewTransportReadDto;
        }

        if (reviewTransportReadDto.thinksTwice)
        {
            reviewTransportReadDto = await methodThinksTwice(reviewTransportReadDto);
        }

        return reviewTransportReadDto;
    }

    /*  --------------------------------------- Parameters to analyze ------------------------------------------ */

    /// <summary>
    /// This method returns parameters for transport review, in case it doesn't exist creates default
    /// </summary>
    /// <param name="user"><see cref="User"/> to be updated</param>
    /// <param name="id"> User <see cref="Guid"/>to edit</param>
    /// <returns>The <see cref="TransportReviewParameters"/> with the parameters </returns>
    private async Task<TransportReviewParameters> getParametersOfReview()
    {
        var parameters = await transportReviewParametersRepository.getTransportReviewParameters();
        if (parameters != null)
        {
            transportReviewParameters = parameters;
            return transportReviewParameters;
        }

        // There are no parameters, create default
        transportReviewParameters.valueFuel = 1.98;
        transportReviewParameters.valueHoliday = 0;
        transportReviewParameters.valueSunday = 0;
        transportReviewParameters.valueSaturday = 0;
        transportReviewParameters.valueToll = 0.15;
        transportReviewParameters.typeAnalysis = TypeAnalysis.BOTH;
        transportReviewParameters.considerTruckBreakDowns = ConsiderTruckBreakDowns.NO;
        await transportReviewParametersRepository.addTransportReviewParameters(transportReviewParameters);
        return transportReviewParameters;
    }
    /*  --------------------------------------- Methods of the final part of the analysis----------------------------------------------- */

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> calculatesFinalValueByTruck(List<TruckForReviewReadDto> listTrucks,
        DateTime dateTime)
    {
        var valueHoliday = 0.0;

        //Verify if transport is on holiday
        // transportReviewParameters.valueHoliday = 0;
        if (transportReviewParameters.valueHoliday > 0)
        {
            var isHoliday = await dateIsHoliday(dateTime.Day, dateTime.Month, dateTime.Year);
            if (isHoliday)
            {
                valueHoliday = transportReviewParameters.valueHoliday;
            }
        }

        foreach (var item in listTrucks)
        {
            item.valueSpend += valueHoliday;
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                item.valueSpend += transportReviewParameters.valueSunday;
            }

            if (dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                item.valueSpend += transportReviewParameters.valueSaturday;
            }

            item.valueSpend = Math.Round(item.valueSpend, 2);
        }

        return listTrucks;
    }

    /// <summary>
    ///  This method calculates (if possible) the expected profit for the transport and selects the best truck or trucks 
    /// </summary>
    /// <returns> <see cref="ReviewTransportDto" /> with the final results. Service is possible? Does the service cause financial loss? Does the service make a profit? </returns>
    private async Task<ReviewTransportDto> calculateFinalServiceValueAndSelectTruck(
        ReviewTransportDto reviewTransportDto)
    {
        reviewTransportDto.thinksTwice = false;
        reviewTransportDto.available = false;
        reviewTransportDto.serviceNotAvailableBecauseSizeOfTruck = false;
        reviewTransportDto.truckSelected = new List<TruckReadDto>();

        bool isAlreadySelect = false;

        foreach (var item in reviewTransportDto.listTrucks)
        {
            if (item.isAvailable && !item.noSpace)
            {
                var profit = reviewTransportDto.valueOffered - item.valueSpend;
                if (profit > 0)
                {
                    TruckReadDto truckSelected = new TruckReadDto();
                    truckSelected.matricula = item.matricula;
                    truckSelected.id = item.id;
                    truckSelected.driver = new UserReadDto();
                    truckSelected.driver.name = item.driver.name;
                    truckSelected.litresSpend = item.litresSpend;
                    truckSelected.valueSpend = item.valueSpend;
                    truckSelected.score = item.score;

                    if (item.truckCategory == TruckCategory.DUMP_TRUCK)
                        truckSelected.availableCapacity = item.maxWeight - reviewTransportDto.capacity;

                    if (item.truckCategory == TruckCategory.REFRIGERATOR_TRUCK ||
                        item.truckCategory == TruckCategory.CONTAINER_TRUCK)
                        truckSelected.availableCapacity = item.maxVolum - reviewTransportDto.capacity;

                    if (reviewTransportDto.truckSelected.Count != 0)
                    {
                        if (item.score == reviewTransportDto.truckSelected[0].score &&
                            item.valueSpend < reviewTransportDto.truckSelected[0].valueSpend)
                            reviewTransportDto.truckSelected[0] = truckSelected;

                        reviewTransportDto.profit = profit;
                        reviewTransportDto.available = true;
                        reviewTransportDto.thinksTwice = false;
                        reviewTransportDto.reviewIsClose = true;
                    }
                    else
                    {
                        reviewTransportDto.truckSelected.Add(truckSelected);
                        reviewTransportDto.profit = profit;
                        reviewTransportDto.available = true;
                        reviewTransportDto.thinksTwice = false;
                        reviewTransportDto.reviewIsClose = true;
                    }
                }
                else
                {
                    if (!isAlreadySelect)
                    {
                        TruckReadDto truckReadDto = new TruckReadDto();
                        truckReadDto.matricula = item.matricula;
                        truckReadDto.id = item.id;
                        truckReadDto.driver = new UserReadDto();
                        truckReadDto.driver.name = item.driver.name;
                        truckReadDto.litresSpend = item.litresSpend;
                        reviewTransportDto.truckSelected.Add(truckReadDto);
                        reviewTransportDto.profit = profit;
                        reviewTransportDto.thinksTwice = true;
                        isAlreadySelect = true;
                    }
                }
            }
        }

        //Assunto resolvido
        if (reviewTransportDto.available || isAlreadySelect)
        {
            return reviewTransportDto;
        }

        //Verifica se pode fazer o serviço com vários  camiões e mesmo assim dá lucro

        List<TruckForReviewReadDto> listTrucksByEstimatedCostOfTransport =
            await sortListTrucksByEstimatedCostOfTransport(reviewTransportDto.listTrucks);

        reviewTransportDto.listTrucks =
            await scoreService.assignsScoreValueSpend(listTrucksByEstimatedCostOfTransport, true, false,
                transportReviewParameters);

        double totalSize = 0;
        double totalCost = 0;
        bool isPossible = false;

        foreach (var item in reviewTransportDto.listTrucks)
        {
            //Volta a analisar o estado camião 
            var isAbsent = await this.isAbsent(dateTime, item.driver.id);
            var truckIsAbsent = await this.truckIsAbsent(dateTime, item.id);
            var truckHasOnService = await truckAlreadyHasService(dateTime, item.id);

            item.isAvailable = await isAvailableToService(isAbsent, truckIsAbsent, truckHasOnService);

            if (item.isAvailable)
            {
                totalSize += item.maxVolum;
                totalCost += item.valueSpend;
                TruckReadDto truckReadDto = new TruckReadDto();
                truckReadDto.litresSpend = item.litresSpend;
                truckReadDto.matricula = item.matricula;
                truckReadDto.id = item.id;
                truckReadDto.driver = new UserReadDto();
                truckReadDto.driver.name = item.driver.name;

                reviewTransportDto.truckSelected.Add(truckReadDto);

                if (totalSize >= reviewTransportDto.capacity &&
                    reviewTransportDto.truckCategory == TruckCategory.CONTAINER_TRUCK)
                {
                    isPossible = true;
                    break;
                }

                if (totalSize >= reviewTransportDto.capacity &&
                    reviewTransportDto.truckCategory == TruckCategory.REFRIGERATOR_TRUCK)
                {
                    isPossible = true;
                    break;
                }

                if (totalSize >= reviewTransportDto.weight &&
                    reviewTransportDto.truckCategory == TruckCategory.DUMP_TRUCK)
                {
                    isPossible = true;
                    break;
                }

                if (totalSize >= reviewTransportDto.liters &&
                    reviewTransportDto.truckCategory == TruckCategory.CISTERN_TRUCK)
                {
                    isPossible = true;
                    break;
                }
            }
        }


        var profitWithListTrucks = reviewTransportDto.valueOffered - totalCost;

        if (isPossible && profitWithListTrucks > 0)
        {
            reviewTransportDto.available = true;
            reviewTransportDto.thinksTwice = false;
            reviewTransportDto.reviewIsClose = true;
            reviewTransportDto.profit = profitWithListTrucks;
            if (reviewTransportDto.truckCategory == TruckCategory.CONTAINER_TRUCK ||
                reviewTransportDto.truckCategory == TruckCategory.REFRIGERATOR_TRUCK)

                reviewTransportDto.listTrucks = await adjustOccupiedVolume(reviewTransportDto.truckSelected,
                    reviewTransportDto.listTrucks, reviewTransportDto.capacity);

            if (reviewTransportDto.truckCategory == TruckCategory.DUMP_TRUCK)
                reviewTransportDto.listTrucks = await adjustOccupiedVolume(reviewTransportDto.truckSelected,
                    reviewTransportDto.listTrucks, reviewTransportDto.weight);

            if (reviewTransportDto.truckCategory == TruckCategory.CISTERN_TRUCK)
                reviewTransportDto = await adjustOccupiedThankVolume(reviewTransportDto.truckSelected,
                    reviewTransportDto.listTrucks, reviewTransportDto, reviewTransportDto.liters);
        }
        else if (isPossible && profitWithListTrucks <= 0)
        {
            if (reviewTransportDto.truckCategory == TruckCategory.CONTAINER_TRUCK ||
                reviewTransportDto.truckCategory == TruckCategory.REFRIGERATOR_TRUCK)
                reviewTransportDto.listTrucks = await adjustOccupiedVolume(reviewTransportDto.truckSelected,
                    reviewTransportDto.listTrucks, reviewTransportDto.capacity);

            if (reviewTransportDto.truckCategory == TruckCategory.DUMP_TRUCK)
                reviewTransportDto.listTrucks = await adjustOccupiedVolume(reviewTransportDto.truckSelected,
                    reviewTransportDto.listTrucks, reviewTransportDto.weight);

            reviewTransportDto.profit = profitWithListTrucks;
            reviewTransportDto.thinksTwice = true;
            reviewTransportDto.available = true;

            if (reviewTransportDto.truckCategory == TruckCategory.CISTERN_TRUCK)
                reviewTransportDto = await adjustOccupiedThankVolume(reviewTransportDto.truckSelected,
                    reviewTransportDto.listTrucks, reviewTransportDto, reviewTransportDto.liters);
        }

        else if (!isPossible)
        {
            reviewTransportDto.listTrucks = await disableTrucks(reviewTransportDto.listTrucks);
            reviewTransportDto.description = "Sem camiões para o fazer";
            reviewTransportDto.available = false;
            reviewTransportDto.thinksTwice = false;
            reviewTransportDto.reviewIsClose = true;
            reviewTransportDto.truckSelected = new List<TruckReadDto>();
        }

        return reviewTransportDto;
    }
    /*  --------------------------------------- ThinksTwice Methods-------------------------------------------------------- */

    /// <inheritdoc/>
    public async Task<ReviewTransportDto> methodThinksTwice(ReviewTransportDto reviewTransportDto)
    {
        var allServices = await serviceRepository.getAllServiceTransports();
        var profit = 0.0;
        int count = 0;
        List<ServiceTransportReadDto> listServices = new List<ServiceTransportReadDto>();

        foreach (var item in allServices)
        {
            if (item.transportReadDto.client_id.id == reviewTransportDto.client.id)
            {
                profit += item.profit;
                count++;
                listServices.Add(item);
            }
        }

        var balance = profit - reviewTransportDto.profit;
        if (balance > 0 && count >= 3)
        {
            reviewTransportDto.listServices = listServices;
            reviewTransportDto.alertManagerForGoodHistory = true;
        }
        else
        {
            reviewTransportDto.alertManagerForGoodHistory = false;
        }

        return reviewTransportDto;
    }

    /*  --------------------------------------- Analysis methods-------------------------------------------------------- */

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> analyzeTrucks(List<TruckForReviewReadDto> listToAnalyze,
        Boolean isContainerOrRefrigerator)
    {
        if (listToAnalyze.Count == 0)
            return listToAnalyze;

        var listWithFitness = new List<TruckForReviewReadDto>();

        if (transportReviewParameters.typeAnalysis != TypeAnalysis.COST)
        {
            List<TruckForReviewReadDto> listTrucksByOccupiedVolume =
                await sortListTrucksByOccupiedVolume(listToAnalyze);
            listWithFitness = await scoreService.assignsScoreOccupiedVolume(listTrucksByOccupiedVolume);
        }


        if (transportReviewParameters.typeAnalysis == TypeAnalysis.OCCUPANCY_RATE) //OCCUPANCY (Just)
        {
            if (transportReviewParameters.considerTruckBreakDowns == ConsiderTruckBreakDowns.YES)
                listWithFitness = await analyzeTruckBreakDowns(listWithFitness);

            listWithFitness = await sortListTrucksByFitness(listWithFitness);
            return listWithFitness;
        }

        if (listWithFitness.Count == 0) // Nao entrou no de cima
            listWithFitness = listToAnalyze;


        List<TruckForReviewReadDto> listTrucksByEstimatedCostOfTransport =
            await sortListTrucksByEstimatedCostOfTransport(listWithFitness);


        listWithFitness =
            await scoreService.assignsScoreValueSpend(listTrucksByEstimatedCostOfTransport,
                isContainerOrRefrigerator, true, transportReviewParameters);


        if (transportReviewParameters.considerTruckBreakDowns == ConsiderTruckBreakDowns.YES)
            listWithFitness = await analyzeTruckBreakDowns(listWithFitness);


        listWithFitness = await sortListTrucksByFitness(listWithFitness);

        return listWithFitness;
    }

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> analyzeTrucksWithCategoryCistern(
        List<TruckForReviewReadDto> listToAnalyze,
        Boolean isCistern)
    {
        if (listToAnalyze.Count == 0)
            return listToAnalyze;

        var listWithFitness = new List<TruckForReviewReadDto>();

        if (transportReviewParameters.typeAnalysis != TypeAnalysis.COST)
        {
            List<TruckForReviewReadDto> listTrucksByOccupiedVolume = await sortListTrucksByTankVolume(listToAnalyze);
            listWithFitness = await scoreService.assignsScoreByTankVolume(listTrucksByOccupiedVolume);
        }


        if (transportReviewParameters.typeAnalysis == TypeAnalysis.OCCUPANCY_RATE) //OCCUPANCY (Just)
        {
            if (transportReviewParameters.considerTruckBreakDowns == ConsiderTruckBreakDowns.YES)
                listWithFitness = await analyzeTruckBreakDowns(listWithFitness);

            listWithFitness = await sortListTrucksByFitness(listWithFitness);
            return listWithFitness;
        }

        if (listWithFitness.Count == 0) // Nao entrou no de cima
            listWithFitness = listToAnalyze;


        List<TruckForReviewReadDto> listTrucksByEstimatedCostOfTransport =
            await sortListTrucksByEstimatedCostOfTransport(listWithFitness);

        listWithFitness =
            await scoreService.assignsScoreValueSpendForTruckCistern(listTrucksByEstimatedCostOfTransport,
                isCistern, true);


        if (transportReviewParameters.considerTruckBreakDowns == ConsiderTruckBreakDowns.YES)
            listWithFitness = await analyzeTruckBreakDowns(listWithFitness);


        listWithFitness = await sortListTrucksByFitness(listWithFitness);

        return listWithFitness;
    }

    /*  --------------------------------------- UseFul methods, used in all categories--------------------------------  */

    /// <summary>
    ///  This method processes information about a truck to determine if the truck is available for a transport and what the estimated cost of doing so is
    /// </summary>
    /// <returns> <see cref="ReviewTransportDto"/>with the information, exception if the id entered is not valid</returns>
    private async Task<List<TruckForReviewReadDto>> processInformationAboutTruck(
        List<TruckForReviewReadDto> listOfTrucksAvailable, double kms, double transportVolume)
    {
        var isAvailable = false;

        //Obter os camioes disponiveis pro serviço
        var availableTruckandDriver = new List<TruckForReviewReadDto>();

        foreach (var item in listOfTrucksAvailable)
        {
            //Verificar disponibilidade do condutor e camião
            var isAbsent = await this.isAbsent(dateTime, item.driver.id);
            var truckIsAbsent = await this.truckIsAbsent(dateTime, item.id);
            var truckHasOnService = await truckAlreadyHasService(dateTime, item.id);
            var truckDetails = await getAllTruckInformation(item.matricula);
            item.maxVolum = truckDetails.capacity;

            item.litresSpend = getLitresConsumption(kms, item.fuelConsumption);
            item.valueSpend = await calculeValueSpend(transportReviewParameters.valueFuel, item.litresSpend,
                transportReviewParameters.valueToll, kms);

            item.occupiedVolumePercentage = getOccupiedVolumePercentage(transportVolume, item.maxVolum);

            item.isAvailable = await isAvailableToService(isAbsent, truckIsAbsent, truckHasOnService);


            availableTruckandDriver.Add(item);
        }

        return availableTruckandDriver;
    }

    /// <summary>
    ///  This method loads all the information needed to analyze a service
    /// </summary>
    /// <returns> <see cref="ReviewTransportDto"/>with the information, exception if the id entered is not valid</returns>
    private async Task<ReviewTransportDto> loadDataToDto(Guid id)
    {
        var item = await transportRepository.getTransport(id);

        if (item is null)
            throw new InvalidOperationException($"Transport with id \"{id}\" does not exists!");

        ReviewTransportDto reviewTransportDto = new ReviewTransportDto();
        reviewTransportDto.capacity = item.capacity;
        reviewTransportDto.status = item.status;
        reviewTransportDto.date = item.date.ToString("dd/MM/yyyy");
        reviewTransportDto.weight = item.weight;
        reviewTransportDto.valueOffered = item.value_offered;
        reviewTransportDto.truckCategory = item.truck_category;
        reviewTransportDto.liters = item.liters;
        reviewTransportDto.destiny = item.destiny;
        reviewTransportDto.origin = item.origin;
        reviewTransportDto.client = new UserReadDto();
        reviewTransportDto.client.name = item.user_client.name;
        reviewTransportDto.client.email = item.user_client.email;
        reviewTransportDto.client.id = item.user_client.id;
        reviewTransportDto.addresseOrganization = item.user_client.organization.addresses;
        reviewTransportDto.id = item.id;
        dateTime = item.date;
        return reviewTransportDto;
    }

    /// <inheritdoc/>
    public async Task<int> getNumberTruckBreakDownsByTruckInTheCurrentYear(Guid idTruck)
    {
        if (listTruckBreakDowns == null)
        {
            listTruckBreakDowns = await truckBreakDownsRepository.getAllTruckBreakDowns();
        }

        var count = 0;
        DateTime localDate = DateTime.Now;
        foreach (var item in listTruckBreakDowns)
        {
            if (idTruck.Equals(item.truck.id) && localDate.Year.Equals(item.date.Year))
            {
                count++;
            }
        }

        return count;
    }

    /// <inheritdoc/>
    public async Task<Double> calculeValueSpend(Double valueFuel, Double litresSpend, Double toll, Double kms)
    {
        var valueToReturn = valueFuel * litresSpend;
        valueToReturn += toll * kms;
        return valueToReturn;
    }


    /// <inheritdoc/>
    public async Task<bool> isAbsent(DateTime dateTime, Guid idDriver)
    {
        var enumerableAbsences = await absenceRepository.getAllAbsences();
        foreach (var item in enumerableAbsences)
        {
            if (dateTime.Day.Equals(item.date.Day) && dateTime.Month.Equals(item.date.Month) &&
                dateTime.Year.Equals(item.date.Year) && idDriver.Equals(item.driver.id) && item.status == Status.ACCEPT)
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task<bool> truckIsAbsent(DateTime dateTime, Guid idTruck)
    {
        var listOfTruckBreakDowns = await truckBreakDownsRepository.getAllTruckBreakDowns();
        listTruckBreakDowns = listOfTruckBreakDowns;

        foreach (var item in listOfTruckBreakDowns)
        {
            if (dateTime.Day.Equals(item.date.Day) && dateTime.Month.Equals(item.date.Month) &&
                dateTime.Year.Equals(item.date.Year) && idTruck.Equals(item.truck.id))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task<bool> truckAlreadyHasService(DateTime dateTime, Guid idTruck)
    {
        var listOfTruckBreakDowns = await serviceRepository.getAllServiceTransports();

        foreach (var item in listOfTruckBreakDowns)
        {
            DateTime dateTimeToVerify = Utils.transformStringToData2(item.transportReadDto.date);
            if (dateTime.Day.Equals(dateTimeToVerify.Day) && dateTime.Month.Equals(dateTimeToVerify.Month) &&
                dateTime.Year.Equals(dateTimeToVerify.Year) && idTruck.Equals(item.truckReadDto.id))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task<Boolean> isAvailableToService(Boolean isAbsent, Boolean truckIsAbsent, Boolean truckHasOnService)
    {
        if (!isAbsent && !truckIsAbsent && !truckHasOnService)
            return true;

        return false;
    }


    /// <inheritdoc/>
    public double getLitresConsumption(double kms, double consumption)
    {
        if (kms <= 0)
            throw new InvalidOperationException($"kms Invalid");

        var consumptionPerKm = consumption / Utils.kmsConsumption;

        return kms * consumptionPerKm;
    }

    /// <inheritdoc/>
    public async Task<double> getTotalNumberKmsOfRoute(String origin, String destiny, String organizationAddress)
    {
        var response = await distanceService.getDistanceAndEstimatedTimeWhitParams(origin,
            destiny);

        var responseOrganizationAddressToOrigin = await distanceService.getDistanceAndEstimatedTimeWhitParams(
            organizationAddress,
            origin);
        
        var responseDestinyToOrganization = await distanceService.getDistanceAndEstimatedTimeWhitParams(
            destiny,
            organizationAddress);
        var kms = (response.rows[0].elements[0].distance.value) / Utils.KmToMeters;

        var kmsOrganizationAddressToOrigin =
            (responseOrganizationAddressToOrigin.rows[0].elements[0].distance.value) / Utils.KmToMeters;

        var kmsDestinyToOrganization =
            (responseDestinyToOrganization.rows[0].elements[0].distance.value) / Utils.KmToMeters;
        
        return (kms + kmsOrganizationAddressToOrigin + kmsDestinyToOrganization);
    }

    /// <inheritdoc/>
    public Double getOccupiedVolumePercentage(Double transportVolume, Double truckVolume)
    {
        if (transportVolume < 0)
            throw new InvalidOperationException($"Transport Volume is invalid");

        if (truckVolume <= 0)
            throw new InvalidOperationException($"Truck Volume is invalid");

        var valueToReturn = transportVolume / truckVolume;
        valueToReturn = valueToReturn * 100;
        valueToReturn = Math.Round(valueToReturn, 2);
        return valueToReturn;
    }

    /// <inheritdoc/>
    public async Task<TruckDetailsXml> getAllTruckInformation(String matricula)
    {
        string xmlfilepath = Path.Combine(Directory.GetCurrentDirectory(), "XMLFiles", "TrucksXML.xml");
        var xmlString = File.ReadAllText(xmlfilepath);
        var stringReader = new StringReader(xmlString);
        var dsSet = new DataSet();
        dsSet.ReadXml(stringReader);

        for (int i = 0; i < dsSet.Tables[0].Rows.Count; i++)
        {
            if (dsSet.Tables[0].Rows[i][0].ToString().Equals(matricula))
            {
                TruckDetailsXml truckDetails = new TruckDetailsXml();
                truckDetails.fuelConsumption = Convert.ToInt32(dsSet.Tables[0].Rows[i][1]);
                truckDetails.matricula = dsSet.Tables[0].Rows[i][0].ToString();
                truckDetails.kms = Convert.ToInt32(dsSet.Tables[0].Rows[i][2]);
                truckDetails.capacity = Convert.ToInt32(dsSet.Tables[0].Rows[i][7]);
                return truckDetails;
            }
        }

        return null;
    }


    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> disableTrucks(List<TruckForReviewReadDto> listToUpdate)
    {
        foreach (var itemToUpdate in listToUpdate)
        {
            itemToUpdate.isAvailable = false;
            itemToUpdate.score = 0;
        }

        return listToUpdate;
    }

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> analyzeTruckBreakDowns(
        List<TruckForReviewReadDto> listTrucks)
    {
        foreach (var item in listTrucks)
        {
            var numberTruckBreakdowns = await getNumberTruckBreakDownsByTruckInTheCurrentYear(item.id);

            if (numberTruckBreakdowns == 3)
            {
                item.score = item.score - 2;
            }
            else if (numberTruckBreakdowns > 3 && numberTruckBreakdowns <= 5)
            {
                item.score = item.score - 3;
            }
            else if (numberTruckBreakdowns > 5)
            {
                item.score = item.score - 6;
            }

            if (item.score < 0)
                item.score = 0;
        }

        return listTrucks;
    }

    /// <inheritdoc/>
    public async Task<bool> dateIsHoliday(int day, int mouth, int year)
    {
        var isHoliday = await holidayService.dateIsHoliday(day, mouth, year);
        return isHoliday;
    }

    /*  --------------------------------------- Sorting methods--------------------------------------------*/

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> sortListTrucksByEstimatedCostOfTransport(
        List<TruckForReviewReadDto> listToSort)
    {
        List<TruckForReviewReadDto> sortedList = listToSort.OrderBy(o => o.valueSpend).ToList();

        return sortedList;
    }

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> sortListTrucksByFitness(
        List<TruckForReviewReadDto> listToSort)
    {
        List<TruckForReviewReadDto>
            sortedList = listToSort.OrderByDescending(o => o.score).ToList();

        return sortedList;
    }

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> sortListTrucksByOccupiedVolume(
        List<TruckForReviewReadDto> listToOrder)
    {
        List<TruckForReviewReadDto>
            sortedList = listToOrder.OrderByDescending(o => o.occupiedVolumePercentage).ToList();

        //Auxiliar map 
        List<TruckForReviewReadDto> sortedList100 = new List<TruckForReviewReadDto>();
        List<TruckForReviewReadDto> sortedListMore100 = new List<TruckForReviewReadDto>();
        List<TruckForReviewReadDto> listToReturn = new List<TruckForReviewReadDto>();

        foreach (var itemToUpdate in sortedList)
        {
            if (itemToUpdate.occupiedVolumePercentage <= 100)
            {
                sortedList100.Add(itemToUpdate);
            }
        }

        foreach (var itemToUpdate in sortedList)
        {
            if (itemToUpdate.occupiedVolumePercentage > 100)
            {
                sortedListMore100.Add(itemToUpdate);
            }
        }

        foreach (var itemToUpdate in sortedList100)
        {
            listToReturn.Add(itemToUpdate);
        }

        foreach (var itemToUpdate in sortedListMore100)
        {
            listToReturn.Add(itemToUpdate);
        }

        return listToReturn;
    }

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> sortListTrucksByTankVolume(
        List<TruckForReviewReadDto> listToSort)
    {
        List<TruckForReviewReadDto>
            sortedList = listToSort.OrderByDescending(o => o.occupiedVolumePercentage).ToList();

        //Auxiliar map 
        List<TruckForReviewReadDto> sortedList100 = new List<TruckForReviewReadDto>();
        List<TruckForReviewReadDto> sortedListMore100 = new List<TruckForReviewReadDto>();
        List<TruckForReviewReadDto> listToReturn = new List<TruckForReviewReadDto>();

        foreach (var itemToUpdate in sortedList)
        {
            if (itemToUpdate.occupiedVolumePercentage <= 100 && itemToUpdate.occupiedVolumePercentage > 90)
            {
                sortedList100.Add(itemToUpdate);
            }
        }

        foreach (var itemToUpdate in sortedList)
        {
            if (itemToUpdate.occupiedVolumePercentage > 100 || itemToUpdate.occupiedVolumePercentage < 90)
            {
                sortedListMore100.Add(itemToUpdate);
            }
        }

        foreach (var itemToUpdate in sortedList100)
        {
            listToReturn.Add(itemToUpdate);
        }

        foreach (var itemToUpdate in sortedListMore100)
        {
            listToReturn.Add(itemToUpdate);
        }

        return listToReturn;
    }

    /*  -----------------------------------Adjustment methods, used for services with multiple trucks-------------------------------------------------*/

    /// <inheritdoc/>
    public async Task<ReviewTransportDto> adjustOccupiedThankVolume(List<TruckReadDto> listTruckSelect,
        List<TruckForReviewReadDto> listToUpdate, ReviewTransportDto reviewTransportDto, Double capacity)
    {
        for (int i = 0; i < listTruckSelect.Count - 1; i++)
        {
            foreach (var itemToUpdate in listToUpdate)
            {
                if (listTruckSelect[i].id == itemToUpdate.id)
                {
                    capacity = capacity - itemToUpdate.maxVolum;
                    itemToUpdate.occupiedVolumePercentage = 100;
                    itemToUpdate.score += 6;
                    var oldString = itemToUpdate.summaryReview.Split("||");
                    itemToUpdate.summaryReview = "" + Utils.MSG_EXCELLENT_OCCUPATION + "||" + oldString[1];
                    itemToUpdate.isAvailable = true;
                }
            }
        }

        Boolean isToRemove = false;
        //Importante Atualiza o ultimo
        foreach (var itemToUpdate in listToUpdate)
        {
            if (listTruckSelect[listTruckSelect.Count - 1].id == itemToUpdate.id)
            {
                itemToUpdate.occupiedVolumePercentage = getOccupiedVolumePercentage(capacity, itemToUpdate.maxVolum);
                var oldString = itemToUpdate.summaryReview.Split("||");

                if (itemToUpdate.occupiedVolumePercentage > 100 || itemToUpdate.occupiedVolumePercentage < 90)
                {
                    isToRemove = true;
                    reviewTransportDto.available = false;
                    reviewTransportDto.thinksTwice = false;
                    itemToUpdate.summaryReview = "Impossível ||" + oldString[1];
                    reviewTransportDto.description = "Último Camião não cumpre requisitos";
                    reviewTransportDto.reviewIsClose = true;
                }
                else
                {
                    itemToUpdate.summaryReview = "" + Utils.MSG_EXCELLENT_OCCUPATION + "||" + oldString[1];
                    itemToUpdate.score += 4;
                    itemToUpdate.isAvailable = true;
                }
            }
        }

        if (isToRemove)
        {
            reviewTransportDto.listTrucks = await disableTrucks(reviewTransportDto.listTrucks);
            reviewTransportDto.truckSelected = new List<TruckReadDto>();
        }

        reviewTransportDto.listTrucks = listToUpdate;
        return reviewTransportDto;
    }


    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> adjustOccupiedVolume(List<TruckReadDto> listTruckSelect,
        List<TruckForReviewReadDto> listToUpdate, Double capacity)
    {
        for (int i = 0; i < listTruckSelect.Count - 1; i++)
        {
            foreach (var itemToUpdate in listToUpdate)
            {
                if (listTruckSelect[i].id == itemToUpdate.id)
                {
                    capacity = capacity - itemToUpdate.maxVolum;
                    itemToUpdate.occupiedVolumePercentage = 100;
                    itemToUpdate.score += 6;
                    var oldString = itemToUpdate.summaryReview.Split("||");

                    itemToUpdate.isAvailable = true;
                    if (transportReviewParameters.typeAnalysis == TypeAnalysis.OCCUPANCY_RATE)
                    {
                        itemToUpdate.summaryReview = Utils.MSG_EXCELLENT_OCCUPATION;
                    }
                    else if (transportReviewParameters.typeAnalysis == TypeAnalysis.COST)
                    {
                        itemToUpdate.summaryReview = "" + Utils.MSG_EXCELLENT_OCCUPATION + "||" + oldString[2];
                    }
                    else
                    {
                        itemToUpdate.summaryReview = Utils.MSG_EXCELLENT_OCCUPATION + "||" + oldString[1];
                    }
                }
            }
        }

        //Importante Atualiza o ultimo
        foreach (var itemToUpdate in listToUpdate)
        {
            if (listTruckSelect[listTruckSelect.Count - 1].id == itemToUpdate.id)
            {
                itemToUpdate.occupiedVolumePercentage = getOccupiedVolumePercentage(capacity, itemToUpdate.maxVolum);

                if (listTruckSelect[listTruckSelect.Count - 1].truckCategory == TruckCategory.DUMP_TRUCK)
                    listTruckSelect[listTruckSelect.Count - 1].availableCapacity = itemToUpdate.maxWeight - capacity;

                if (listTruckSelect[listTruckSelect.Count - 1].truckCategory == TruckCategory.REFRIGERATOR_TRUCK ||
                    listTruckSelect[listTruckSelect.Count - 1].truckCategory == TruckCategory.CONTAINER_TRUCK)
                    listTruckSelect[listTruckSelect.Count - 1].availableCapacity = itemToUpdate.maxVolum - capacity;

                if (transportReviewParameters.typeAnalysis == TypeAnalysis.OCCUPANCY_RATE)
                {
                    itemToUpdate.summaryReview = Utils.MSG_EXCELLENT_OCCUPATION;
                }
                else if (transportReviewParameters.typeAnalysis == TypeAnalysis.COST)
                {
                    var oldString = itemToUpdate.summaryReview.Split("||");
                    itemToUpdate.summaryReview = "" + Utils.MSG_EXCELLENT_OCCUPATION + "||" + oldString[2];
                }
                else
                {
                    var oldString = itemToUpdate.summaryReview.Split("||");
                    itemToUpdate.summaryReview = Utils.MSG_EXCELLENT_OCCUPATION + "||" + oldString[1];
                }

                itemToUpdate.score += 4;
                itemToUpdate.isAvailable = true;
            }
        }

        return listToUpdate;
    }
}
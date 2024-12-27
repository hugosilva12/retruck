using Moq;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;
using WebApplication1.Services;

namespace ApiTest.UTest;

public class testSelectionAlgorithmService
{
    private readonly Mock<ITruckBreakDownsRepository> mock = new Mock<ITruckBreakDownsRepository>();

    private readonly SelectionAlgorithmService reviewService =
        new SelectionAlgorithmService(null, null, null, null, null, null, null, null, null);

    private readonly ScoreService scoreService = new ScoreService();

    [Test]
    public async Task getOccupiedVolumePercentageTest()
    {
        var percentage = reviewService.getOccupiedVolumePercentage(2000, 4000);
        var expected = 50;

        //Assert
        Assert.IsTrue(percentage.Equals(50));
    }

    [Test]
    public async Task getOccupiedVolumePercentageBva01Test()
    {
        Assert.Throws<InvalidOperationException>(() => reviewService.getOccupiedVolumePercentage(2212, 0));
    }


    [Test]
    public async Task getOccupiedVolumePercentageBva02Test()
    {
        Assert.Throws<InvalidOperationException>(() => reviewService.getOccupiedVolumePercentage(-1, 4000));
    }

    [Test]
    public async Task getLitresConsumptionTest()
    {
        double expected = 100;

        var valueReturned = reviewService.getLitresConsumption(200, 50);

        Assert.AreEqual(expected, valueReturned);
    }

    [Test]
    public async Task getLitresConsumptionBva01Test()
    {
        Assert.Throws<InvalidOperationException>(() => reviewService.getLitresConsumption(0, 40));
    }

    [Test]
    public async Task getNumberTruckBreakDownsByTruckInTheCurrentYearTest()
    {
        SelectionAlgorithmService reviewService =
            new SelectionAlgorithmService(null, null, null, null, mock.Object, null, null, null, null);

        List<TruckBreakDowns> truckBreakDownsList = new List<TruckBreakDowns>();
        TruckBreakDowns truckBreakDown = new TruckBreakDowns();
        truckBreakDown.truck = new Truck();
        truckBreakDown.truck.matricula = "test2";
        truckBreakDown.date = DateTime.Now;
        truckBreakDownsList.Add(truckBreakDown);

        var id = truckBreakDown.truck.id;

        //Simulation
        mock.Setup(x => x.getAllTruckBreakDowns())
            .ReturnsAsync(() => truckBreakDownsList);

        var count = await reviewService.getNumberTruckBreakDownsByTruckInTheCurrentYear(id);

        Assert.AreEqual(1, count);
    }

    [Test]
    public async Task getNumberTruckBreakDownsByTruckInTheCurrentYearBva01Test()
    {
        SelectionAlgorithmService reviewService =
            new SelectionAlgorithmService(null, null, null, null, mock.Object, null, null, null, null);

        List<TruckBreakDowns> truckBreakDownsList = new List<TruckBreakDowns>();

        //Simulation
        mock.Setup(x => x.getAllTruckBreakDowns())
            .ReturnsAsync(() => truckBreakDownsList);

        var count = await reviewService.getNumberTruckBreakDownsByTruckInTheCurrentYear(Guid.NewGuid());

        Assert.AreEqual(0, count);
    }

    [Test]
    public async Task disableTrucksTest()
    {
        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 90;
        truckForReviewReadDto.isAvailable = true;

        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(truckForReviewReadDto);

        var listUpdated = await reviewService.disableTrucks(list);

        Assert.AreEqual(1, listUpdated.Count);
        Assert.IsFalse(listUpdated[0].isAvailable);
    }

    public async Task<TruckForReviewReadDto> instanceTruckForReviewReadDto()
    {
        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 90;
        truckForReviewReadDto.score = 6;
        return truckForReviewReadDto;
    }

    [Test]
    public async Task sortListTrucksByFitnessTest()
    {
        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(await instanceTruckForReviewReadDto());
        var secondTruck = await instanceTruckForReviewReadDto();
        secondTruck.score = 2;
        list.Add(secondTruck);

        var count = await reviewService.sortListTrucksByFitness(list);

        Assert.AreEqual(2, count.Count);
        Assert.AreEqual(6, count[0].score);
        Assert.AreEqual(2, count[1].score);
    }

    [Test]
    public async Task assignsFitnessScoreOccupiedVolumeBva01Test()
    {
        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 100.1;

        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(truckForReviewReadDto);

        var listAferExecuteMethod = await scoreService.assignsScoreOccupiedVolume(list);

        Assert.IsTrue(listAferExecuteMethod[0].noSpace);
    }

    [Test]
    public async Task assignsFitnessScoreOccupiedVolumeBva02Test()
    {
        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 100;

        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(truckForReviewReadDto);

        var listAferExecuteMethod = await scoreService.assignsScoreOccupiedVolume(list);

        Assert.IsFalse(listAferExecuteMethod[0].noSpace);
    }

    [Test]
    public async Task assignsFitnessScoreOccupiedVolumeTest()
    {
        TransportReviewParameters transportReviewParameters = new TransportReviewParameters();
        transportReviewParameters.typeAnalysis = TypeAnalysis.BOTH;

        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 105;

        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(truckForReviewReadDto);

        var listAferExecuteMethod = await scoreService.assignsScoreOccupiedVolume(list);
        var listAferExecuteMethodAssignsFitnessScoreOccupiedVolume =
            await scoreService.assignsScoreValueSpend(listAferExecuteMethod, true, true,
                transportReviewParameters);
        Assert.IsTrue(listAferExecuteMethodAssignsFitnessScoreOccupiedVolume[0].score == 0);
    }

    [Test]
    public async Task assignsFitnessScoreByTankVolumeTest()
    {
        TransportReviewParameters transportReviewParameters = new TransportReviewParameters();
        transportReviewParameters.typeAnalysis = TypeAnalysis.BOTH;

        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 80;

        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(truckForReviewReadDto);

        var listAferExecuteMethod = await scoreService.assignsScoreByTankVolume(list);
        var listAferExecuteMethodAssignsFitnessScoreOccupiedVolume =
            await scoreService.assignsScoreValueSpendForTruckCistern(listAferExecuteMethod, true, true);
        Assert.IsTrue(listAferExecuteMethodAssignsFitnessScoreOccupiedVolume[0].score == 0);
    }

    [Test]
    public async Task assignsFitnessBvaO1Test()
    {
        TransportReviewParameters transportReviewParameters = new TransportReviewParameters();
        transportReviewParameters.typeAnalysis = TypeAnalysis.BOTH;

        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 90;

        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(truckForReviewReadDto);

        var listAferExecuteMethod = await scoreService.assignsScoreByTankVolume(list);
        var listAferExecuteMethodAssignsFitnessScoreOccupiedVolume =
            await scoreService.assignsScoreValueSpendForTruckCistern(listAferExecuteMethod, true, true);
        Assert.IsTrue(listAferExecuteMethodAssignsFitnessScoreOccupiedVolume[0].score == 12);
    }

    [Test]
    public async Task assignsFitnessScoreByTankVolumeBva01Test()
    {
        TransportReviewParameters transportReviewParameters = new TransportReviewParameters();
        transportReviewParameters.typeAnalysis = TypeAnalysis.BOTH;

        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 90;

        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(truckForReviewReadDto);

        var listAferExecuteMethod = await scoreService.assignsScoreByTankVolume(list);

        Assert.IsTrue(listAferExecuteMethod[0].score == 6);
    }

    [Test]
    public async Task assignsAllFitnessTest()
    {
        TransportReviewParameters transportReviewParameters = new TransportReviewParameters();
        transportReviewParameters.typeAnalysis = TypeAnalysis.BOTH;

        TruckForReviewReadDto truckForReviewReadDto = new TruckForReviewReadDto();
        truckForReviewReadDto.occupiedVolumePercentage = 90;
        List<TruckForReviewReadDto> list = new List<TruckForReviewReadDto>();
        list.Add(truckForReviewReadDto);


        var listAferExecuteMethod = await scoreService.assignsScoreOccupiedVolume(list);

        Assert.IsTrue(listAferExecuteMethod[0].score == 6);

        var listAferExecuteMethodAssignsFitnessScoreOccupiedVolume =
            await scoreService.assignsScoreValueSpend(listAferExecuteMethod, true, true,
                transportReviewParameters);
        Assert.IsTrue(listAferExecuteMethodAssignsFitnessScoreOccupiedVolume[0].score == 12);
    }
}
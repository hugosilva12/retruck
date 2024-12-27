using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;

namespace WebApplication1.Services;

/// <summary>
/// This class contains all the methods needed to score trucks
/// </summary>
public class ScoreService : IScoreService
{
    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> assignsScoreByTankVolume(
        List<TruckForReviewReadDto> listTrucksByTankVolume)
    {
        int nextFitness = 6;
        for (int i = 0; i < listTrucksByTankVolume.Count; i++)
        {
            if (listTrucksByTankVolume[i].occupiedVolumePercentage >= 90 &&
                listTrucksByTankVolume[i].occupiedVolumePercentage <= 100)
            {
                listTrucksByTankVolume[i].summaryReview += Utils.MSG_EXCELLENT_OCCUPATION;
            }
            else if (listTrucksByTankVolume[i].occupiedVolumePercentage < 90)
            {
                listTrucksByTankVolume[i].summaryReview += Utils.MSG_BAD_OCCUPATION;
                listTrucksByTankVolume[i].isAvailable = false;
            }

            //Verify the space
            if (listTrucksByTankVolume[i].occupiedVolumePercentage > 100)
            {
                listTrucksByTankVolume[i].summaryReview += " " + Utils.MSG_ERROR;
                listTrucksByTankVolume[i].noSpace = true;
                listTrucksByTankVolume[i].isAvailable = false;
            }
            else
            {
                listTrucksByTankVolume[i].noSpace = false;
            }

            //assigns score
            if (i == 0 && listTrucksByTankVolume[0].occupiedVolumePercentage <= 100 &&
                listTrucksByTankVolume[0].occupiedVolumePercentage >= 90) //6
            {
                listTrucksByTankVolume[0].score += nextFitness;
                nextFitness = 4;
            }
            else if (i == 1 && listTrucksByTankVolume[1].occupiedVolumePercentage <= 100 &&
                     listTrucksByTankVolume[1].occupiedVolumePercentage >= 90) //4
            {
                if (listTrucksByTankVolume[0].occupiedVolumePercentage
                    .Equals(listTrucksByTankVolume[1].occupiedVolumePercentage))
                {
                    listTrucksByTankVolume[1].score += (nextFitness + 2);
                    nextFitness = 4;
                }
                else
                {
                    listTrucksByTankVolume[1].score += nextFitness;
                    nextFitness = 2;
                }
            }
            else if (i == 2 && listTrucksByTankVolume[2].occupiedVolumePercentage <= 100 &&
                     listTrucksByTankVolume[2].occupiedVolumePercentage >= 90) //2
            {
                if (listTrucksByTankVolume[0].occupiedVolumePercentage
                    .Equals(listTrucksByTankVolume[2].occupiedVolumePercentage))
                {
                    var diff = 6 - nextFitness;
                    listTrucksByTankVolume[2].score += (nextFitness + diff);
                    nextFitness = 4;
                }
                else if (listTrucksByTankVolume[1].occupiedVolumePercentage
                         .Equals(listTrucksByTankVolume[2].occupiedVolumePercentage))
                {
                    var diff = 4 - nextFitness;
                    listTrucksByTankVolume[2].score += (nextFitness + diff);
                    nextFitness = 2;
                }
                else
                {
                    listTrucksByTankVolume[2].score += nextFitness;

                    if (nextFitness == 2)
                    {
                        nextFitness = nextFitness - 1;
                    }
                    else
                    {
                        nextFitness = nextFitness / 2;
                    }
                }
            }
            else if (i == 3 && listTrucksByTankVolume[3].occupiedVolumePercentage <= 100 &&
                     listTrucksByTankVolume[0].occupiedVolumePercentage >= 90) // 1
            {
                if (listTrucksByTankVolume[0].occupiedVolumePercentage
                    .Equals(listTrucksByTankVolume[3].occupiedVolumePercentage))
                {
                    var diff = 6 - nextFitness;
                    listTrucksByTankVolume[3].score += (nextFitness + diff);
                    nextFitness = 4;
                }
                else if (listTrucksByTankVolume[1].occupiedVolumePercentage
                         .Equals(listTrucksByTankVolume[3].occupiedVolumePercentage))
                {
                    var diff = 4 - nextFitness;
                    listTrucksByTankVolume[3].score += (nextFitness + diff);
                    nextFitness = 2;
                }
                else if (listTrucksByTankVolume[2].occupiedVolumePercentage
                         .Equals(listTrucksByTankVolume[3].occupiedVolumePercentage))
                {
                    var diff = 2 - nextFitness;
                    listTrucksByTankVolume[3].score += (nextFitness + diff);
                    nextFitness = 1;
                }
                else
                {
                    listTrucksByTankVolume[3].score += nextFitness;

                    if (nextFitness > 2) //4
                    {
                        nextFitness = nextFitness - 2;
                    }
                    else if (nextFitness == 2)
                    {
                        nextFitness = nextFitness - 1;
                    }
                    else
                    {
                        nextFitness = 0;
                    }
                }
            }
            else
            {
                if (listTrucksByTankVolume[i].occupiedVolumePercentage > 100 ||
                    listTrucksByTankVolume[0].occupiedVolumePercentage < 90)
                {
                    listTrucksByTankVolume[i].score = 0;
                }
                else
                {
                    listTrucksByTankVolume[i].score += nextFitness;
                    if (nextFitness > 2) //4
                    {
                        nextFitness = nextFitness - 2;
                    }
                    else if (nextFitness == 2)
                    {
                        nextFitness = nextFitness - 1;
                    }
                    else
                    {
                        nextFitness = 0;
                    }
                }
            }
        }

        return listTrucksByTankVolume;
    }

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> assignsScoreOccupiedVolume(
        List<TruckForReviewReadDto> listTrucksByOccupiedVolume)
    {
        int nextFitness = 6;
        for (int i = 0; i < listTrucksByOccupiedVolume.Count; i++)
        {
            if (listTrucksByOccupiedVolume[i].occupiedVolumePercentage > 90 &&
                listTrucksByOccupiedVolume[i].occupiedVolumePercentage <= 95)
            {
                listTrucksByOccupiedVolume[i].summaryReview += Utils.MSG_VERY_GOOD_OCCUPATION;
            }
            else if (listTrucksByOccupiedVolume[i].occupiedVolumePercentage <= 90 &&
                     listTrucksByOccupiedVolume[i].occupiedVolumePercentage > 70)
            {
                listTrucksByOccupiedVolume[i].summaryReview += Utils.MSG_GOOD_OCCUPATION;
            }
            else if (listTrucksByOccupiedVolume[i].occupiedVolumePercentage <= 70 &&
                     listTrucksByOccupiedVolume[i].occupiedVolumePercentage > 50)
            {
                listTrucksByOccupiedVolume[i].summaryReview += Utils.MSG_MEDIUM_OCCUPATION;
            }
            else if (listTrucksByOccupiedVolume[i].occupiedVolumePercentage > 95 &&
                     listTrucksByOccupiedVolume[i].occupiedVolumePercentage <= 100)
            {
                listTrucksByOccupiedVolume[i].summaryReview += Utils.MSG_EXCELLENT_OCCUPATION;
            }
            else if (listTrucksByOccupiedVolume[i].occupiedVolumePercentage < 50)
            {
                listTrucksByOccupiedVolume[i].summaryReview = Utils.MSG_BAD_OCCUPATION;
            }

            //Verify the space
            if (listTrucksByOccupiedVolume[i].occupiedVolumePercentage > 100)
            {
                listTrucksByOccupiedVolume[i].summaryReview += " " + Utils.MSG_ERROR;
                listTrucksByOccupiedVolume[i].noSpace = true;
                listTrucksByOccupiedVolume[i].isAvailable = false;
            }
            else
            {
                listTrucksByOccupiedVolume[i].noSpace = false;
            }

            //assings score
            if (i == 0 && listTrucksByOccupiedVolume[0].occupiedVolumePercentage <= 100) //6
            {
                listTrucksByOccupiedVolume[0].score += nextFitness;
                nextFitness = 4;
            }
            else if (i == 1 && listTrucksByOccupiedVolume[1].occupiedVolumePercentage <= 100) //4
            {
                if (listTrucksByOccupiedVolume[0].occupiedVolumePercentage
                    .Equals(listTrucksByOccupiedVolume[1].occupiedVolumePercentage))
                {
                    listTrucksByOccupiedVolume[1].score += (nextFitness + 2);
                    nextFitness = 4;
                }
                else
                {
                    listTrucksByOccupiedVolume[1].score += nextFitness;
                    nextFitness = 2;
                }
            }
            else if (i == 2 && listTrucksByOccupiedVolume[2].occupiedVolumePercentage <= 100) //2
            {
                if (listTrucksByOccupiedVolume[0].occupiedVolumePercentage
                    .Equals(listTrucksByOccupiedVolume[2].occupiedVolumePercentage))
                {
                    var diff = 6 - nextFitness;
                    listTrucksByOccupiedVolume[2].score += (nextFitness + diff);
                    nextFitness = 4;
                }
                else if (listTrucksByOccupiedVolume[1].occupiedVolumePercentage
                         .Equals(listTrucksByOccupiedVolume[2].occupiedVolumePercentage))
                {
                    var diff = 4 - nextFitness;
                    listTrucksByOccupiedVolume[2].score += (nextFitness + diff);
                    nextFitness = 2;
                }
                else
                {
                    listTrucksByOccupiedVolume[2].score += nextFitness;

                    if (nextFitness == 2)
                    {
                        nextFitness = nextFitness - 1;
                    }
                    else
                    {
                        nextFitness = nextFitness / 2;
                    }
                }
            }
            else if (i == 3 && listTrucksByOccupiedVolume[3].occupiedVolumePercentage <= 100) // 1
            {
                if (listTrucksByOccupiedVolume[0].occupiedVolumePercentage
                    .Equals(listTrucksByOccupiedVolume[3].occupiedVolumePercentage))
                {
                    var diff = 6 - nextFitness;
                    listTrucksByOccupiedVolume[3].score += (nextFitness + diff);
                    nextFitness = 4;
                }
                else if (listTrucksByOccupiedVolume[1].occupiedVolumePercentage
                         .Equals(listTrucksByOccupiedVolume[3].occupiedVolumePercentage))
                {
                    var diff = 4 - nextFitness;
                    listTrucksByOccupiedVolume[3].score += (nextFitness + diff);
                    nextFitness = 2;
                }
                else if (listTrucksByOccupiedVolume[2].occupiedVolumePercentage
                         .Equals(listTrucksByOccupiedVolume[3].occupiedVolumePercentage))
                {
                    var diff = 2 - nextFitness;
                    listTrucksByOccupiedVolume[3].score += (nextFitness + diff);
                    nextFitness = 1;
                }
                else
                {
                    listTrucksByOccupiedVolume[3].score += nextFitness;

                    if (nextFitness > 2) //4
                    {
                        nextFitness = nextFitness - 2;
                    }
                    else if (nextFitness == 2)
                    {
                        nextFitness = nextFitness - 1;
                    }
                    else
                    {
                        nextFitness = 0;
                    }
                }
            }
            else
            {
                if (listTrucksByOccupiedVolume[i].occupiedVolumePercentage > 100)
                {
                    listTrucksByOccupiedVolume[i].score = 0;
                    nextFitness = 6;
                }
                else
                {
                    listTrucksByOccupiedVolume[i].score += nextFitness;
                    if (nextFitness > 2) //4
                    {
                        nextFitness = nextFitness - 2;
                    }
                    else if (nextFitness == 2)
                    {
                        nextFitness = nextFitness - 1;
                    }
                    else
                    {
                        nextFitness = 0;
                    }
                }
            }
        }

        return listTrucksByOccupiedVolume;
    }

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> assignsScoreValueSpendForTruckCistern(
        List<TruckForReviewReadDto> listTrucksByEstimatedCostOfCisternTruck, Boolean isContainerOrRefrigerator,
        Boolean firstAvaliation)
    {
        int nextFitness = 6;
        for (int i = 0; i < listTrucksByEstimatedCostOfCisternTruck.Count; i++)
        {
            if (listTrucksByEstimatedCostOfCisternTruck[i].occupiedVolumePercentage > 100 ||
                listTrucksByEstimatedCostOfCisternTruck[i].occupiedVolumePercentage < 90 && firstAvaliation)
            {
                listTrucksByEstimatedCostOfCisternTruck[i].score = 0;
                listTrucksByEstimatedCostOfCisternTruck[i].isAvailable = false;
                if (nextFitness == 6)
                    listTrucksByEstimatedCostOfCisternTruck[i].summaryReview += " ||" + Utils.MSG_1_COST;
                if (nextFitness == 4)
                    listTrucksByEstimatedCostOfCisternTruck[i].summaryReview += " ||" + Utils.MSG_2_COST;
                if (nextFitness == 2)
                    listTrucksByEstimatedCostOfCisternTruck[i].summaryReview += " ||" + Utils.MSG_3_COST;
                if (nextFitness == 0)
                    listTrucksByEstimatedCostOfCisternTruck[i].summaryReview += " ||" + Utils.MSG_4_COST;
            }
            else
            {
                if (i == 0) //6
                {
                    listTrucksByEstimatedCostOfCisternTruck[0].score += nextFitness;
                    listTrucksByEstimatedCostOfCisternTruck[0].summaryReview += " ||" + Utils.MSG_1_COST;
                    nextFitness = 4;
                }
                else if (i == 1) //4
                {
                    if (listTrucksByEstimatedCostOfCisternTruck[0].valueSpend
                        .Equals(listTrucksByEstimatedCostOfCisternTruck[1].valueSpend))
                    {
                        listTrucksByEstimatedCostOfCisternTruck[1].score += (nextFitness + 2);
                        listTrucksByEstimatedCostOfCisternTruck[1].summaryReview += " ||" + Utils.MSG_1_COST;
                        nextFitness = 4;
                    }
                    else
                    {
                        listTrucksByEstimatedCostOfCisternTruck[1].score += nextFitness;
                        listTrucksByEstimatedCostOfCisternTruck[1].summaryReview += " ||" + Utils.MSG_2_COST;
                        nextFitness = 2;
                    }
                }
                else if (i == 2) //2
                {
                    if (listTrucksByEstimatedCostOfCisternTruck[0].valueSpend
                        .Equals(listTrucksByEstimatedCostOfCisternTruck[2].valueSpend))
                    {
                        var diff = 6 - nextFitness;
                        listTrucksByEstimatedCostOfCisternTruck[2].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfCisternTruck[2].summaryReview += " ||" + Utils.MSG_1_COST;
                        nextFitness = 4;
                    }
                    else if (listTrucksByEstimatedCostOfCisternTruck[1].valueSpend
                             .Equals(listTrucksByEstimatedCostOfCisternTruck[2].valueSpend))
                    {
                        var diff = 4 - nextFitness;
                        listTrucksByEstimatedCostOfCisternTruck[2].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfCisternTruck[2].summaryReview += " ||" + Utils.MSG_2_COST;
                        nextFitness = 2;
                    }
                    else
                    {
                        listTrucksByEstimatedCostOfCisternTruck[2].score += nextFitness;

                        if (nextFitness == 2)
                        {
                            listTrucksByEstimatedCostOfCisternTruck[2].summaryReview += " ||" + Utils.MSG_3_COST;
                            nextFitness = nextFitness - 1;
                        }
                        else
                        {
                            listTrucksByEstimatedCostOfCisternTruck[2].summaryReview += " ||" + Utils.MSG_2_COST;
                            nextFitness = nextFitness / 2;
                        }
                    }
                }
                else if (i == 3) // 1
                {
                    if (listTrucksByEstimatedCostOfCisternTruck[0].valueSpend
                        .Equals(listTrucksByEstimatedCostOfCisternTruck[3].valueSpend))
                    {
                        var diff = 6 - nextFitness;
                        listTrucksByEstimatedCostOfCisternTruck[3].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfCisternTruck[3].summaryReview += " ||" + Utils.MSG_1_COST;
                        nextFitness = 4;
                    }
                    else if (listTrucksByEstimatedCostOfCisternTruck[1].valueSpend
                             .Equals(listTrucksByEstimatedCostOfCisternTruck[3].valueSpend))
                    {
                        var diff = 4 - nextFitness;
                        listTrucksByEstimatedCostOfCisternTruck[3].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfCisternTruck[3].summaryReview += " ||" + Utils.MSG_2_COST;
                        nextFitness = 2;
                    }
                    else if (listTrucksByEstimatedCostOfCisternTruck[2].valueSpend
                             .Equals(listTrucksByEstimatedCostOfCisternTruck[3].valueSpend))
                    {
                        var diff = 2 - nextFitness;
                        listTrucksByEstimatedCostOfCisternTruck[3].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfCisternTruck[3].summaryReview += " ||" + Utils.MSG_3_COST;
                        nextFitness = 1;
                    }
                    else
                    {
                        listTrucksByEstimatedCostOfCisternTruck[3].score += nextFitness;

                        if (nextFitness > 2) //4
                        {
                            listTrucksByEstimatedCostOfCisternTruck[3].summaryReview += " ||" + Utils.MSG_2_COST;
                            nextFitness = nextFitness - 2;
                        }
                        else if (nextFitness == 2) //2
                        {
                            listTrucksByEstimatedCostOfCisternTruck[3].summaryReview += " ||" + Utils.MSG_3_COST;
                            nextFitness = nextFitness - 1;
                        }
                        else //1
                        {
                            listTrucksByEstimatedCostOfCisternTruck[3].summaryReview += " ||" + Utils.MSG_4_COST;
                            nextFitness = 0;
                        }
                    }
                }
                else
                {
                    listTrucksByEstimatedCostOfCisternTruck[i].score += nextFitness;
                    if (nextFitness > 2) //4
                    {
                        listTrucksByEstimatedCostOfCisternTruck[i].summaryReview += " ||" + Utils.MSG_2_COST;
                        nextFitness = nextFitness - 2;
                    }
                    else if (nextFitness == 2)
                    {
                        listTrucksByEstimatedCostOfCisternTruck[i].summaryReview += " ||" + Utils.MSG_3_COST;
                        nextFitness = nextFitness - 1;
                    }
                    else if (nextFitness == 1)
                    {
                        listTrucksByEstimatedCostOfCisternTruck[i].summaryReview += " ||" + Utils.MSG_4_COST;
                        nextFitness = 0;
                    }
                    else
                    {
                        listTrucksByEstimatedCostOfCisternTruck[i].summaryReview += " ||" + Utils.MSG_5_COST;
                        nextFitness = 0;
                    }
                }
            }
        }

        return listTrucksByEstimatedCostOfCisternTruck;
    }

    /// <inheritdoc/>
    public async Task<List<TruckForReviewReadDto>> assignsScoreValueSpend(
        List<TruckForReviewReadDto> listTrucksByEstimatedCostOfTransport, Boolean isContainerOrRefrigerator,
        Boolean firstAvaliation, TransportReviewParameters transportReviewParameters)
    {
        int nextFitness = 6;
        for (int i = 0; i < listTrucksByEstimatedCostOfTransport.Count; i++)
        {
            if (listTrucksByEstimatedCostOfTransport[i].noSpace && isContainerOrRefrigerator && firstAvaliation)
            {
                listTrucksByEstimatedCostOfTransport[i].score = 0;
            }
            else if (transportReviewParameters.typeAnalysis == TypeAnalysis.COST &&
                     listTrucksByEstimatedCostOfTransport[i].occupiedVolumePercentage > 100 && firstAvaliation)
            {
                listTrucksByEstimatedCostOfTransport[i].isAvailable = false;
                listTrucksByEstimatedCostOfTransport[i].score = 0;
                listTrucksByEstimatedCostOfTransport[i].summaryReview += " || " + Utils.MSG_ERROR;
            }
            else
            {
                if (i == 0) //6
                {
                    listTrucksByEstimatedCostOfTransport[0].score += nextFitness;
                    listTrucksByEstimatedCostOfTransport[0].summaryReview += " ||" + Utils.MSG_1_COST;
                    nextFitness = 4;
                }
                else if (i == 1) //4
                {
                    if (listTrucksByEstimatedCostOfTransport[0].valueSpend
                        .Equals(listTrucksByEstimatedCostOfTransport[1].valueSpend))
                    {
                        listTrucksByEstimatedCostOfTransport[1].score += (nextFitness + 2);
                        listTrucksByEstimatedCostOfTransport[1].summaryReview += " ||" + Utils.MSG_1_COST;
                        nextFitness = 4;
                    }
                    else
                    {
                        listTrucksByEstimatedCostOfTransport[1].score += nextFitness;
                        listTrucksByEstimatedCostOfTransport[1].summaryReview += " ||" + Utils.MSG_2_COST;
                        nextFitness = 2;
                    }
                }
                else if (i == 2) //2
                {
                    if (listTrucksByEstimatedCostOfTransport[0].valueSpend
                        .Equals(listTrucksByEstimatedCostOfTransport[2].valueSpend))
                    {
                        var diff = 6 - nextFitness;
                        listTrucksByEstimatedCostOfTransport[2].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfTransport[2].summaryReview += " ||" + Utils.MSG_1_COST;
                        nextFitness = 4;
                    }
                    else if (listTrucksByEstimatedCostOfTransport[1].valueSpend
                             .Equals(listTrucksByEstimatedCostOfTransport[2].valueSpend))
                    {
                        var diff = 4 - nextFitness;
                        listTrucksByEstimatedCostOfTransport[2].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfTransport[2].summaryReview += " ||" + Utils.MSG_2_COST;
                        nextFitness = 2;
                    }
                    else
                    {
                        listTrucksByEstimatedCostOfTransport[2].score += nextFitness;

                        if (nextFitness == 2)
                        {
                            listTrucksByEstimatedCostOfTransport[2].summaryReview += " ||" + Utils.MSG_3_COST;
                            nextFitness = nextFitness - 1;
                        }
                        else
                        {
                            listTrucksByEstimatedCostOfTransport[2].summaryReview += " ||" + Utils.MSG_2_COST;
                            nextFitness = nextFitness / 2;
                        }
                    }
                }
                else if (i == 3) // 1
                {
                    if (listTrucksByEstimatedCostOfTransport[0].valueSpend
                        .Equals(listTrucksByEstimatedCostOfTransport[3].valueSpend))
                    {
                        var diff = 6 - nextFitness;
                        listTrucksByEstimatedCostOfTransport[3].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfTransport[3].summaryReview += " ||" + Utils.MSG_1_COST;
                        nextFitness = 4;
                    }
                    else if (listTrucksByEstimatedCostOfTransport[1].valueSpend
                             .Equals(listTrucksByEstimatedCostOfTransport[3].valueSpend))
                    {
                        var diff = 4 - nextFitness;
                        listTrucksByEstimatedCostOfTransport[3].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfTransport[3].summaryReview += " ||" + Utils.MSG_2_COST;
                        nextFitness = 2;
                    }
                    else if (listTrucksByEstimatedCostOfTransport[2].valueSpend
                             .Equals(listTrucksByEstimatedCostOfTransport[3].valueSpend))
                    {
                        var diff = 2 - nextFitness;
                        listTrucksByEstimatedCostOfTransport[3].score += (nextFitness + diff);
                        listTrucksByEstimatedCostOfTransport[3].summaryReview += " ||" + Utils.MSG_3_COST;
                        nextFitness = 1;
                    }
                    else
                    {
                        listTrucksByEstimatedCostOfTransport[3].score += nextFitness;

                        if (nextFitness > 2) //4
                        {
                            listTrucksByEstimatedCostOfTransport[3].summaryReview += " ||" + Utils.MSG_2_COST;
                            nextFitness = nextFitness - 2;
                        }
                        else if (nextFitness == 2) //2
                        {
                            listTrucksByEstimatedCostOfTransport[3].summaryReview += " ||" + Utils.MSG_3_COST;
                            nextFitness = nextFitness - 1;
                        }
                        else //1
                        {
                            listTrucksByEstimatedCostOfTransport[3].summaryReview += " ||" + Utils.MSG_4_COST;
                            nextFitness = 0;
                        }
                    }
                }
                else
                {
                    listTrucksByEstimatedCostOfTransport[i].score += nextFitness;
                    if (nextFitness > 2) //4
                    {
                        listTrucksByEstimatedCostOfTransport[i].summaryReview += " ||" + Utils.MSG_2_COST;
                        nextFitness = nextFitness - 2;
                    }
                    else if (nextFitness == 2)
                    {
                        listTrucksByEstimatedCostOfTransport[i].summaryReview += " ||" + Utils.MSG_3_COST;
                        nextFitness = nextFitness - 1;
                    }
                    else if (nextFitness == 1)
                    {
                        listTrucksByEstimatedCostOfTransport[i].summaryReview += " ||" + Utils.MSG_4_COST;
                        nextFitness = 0;
                    }
                    else
                    {
                        listTrucksByEstimatedCostOfTransport[i].summaryReview += " ||" + Utils.MSG_5_COST;
                        nextFitness = 0;
                    }
                }
            }
        }

        return listTrucksByEstimatedCostOfTransport;
    }
}
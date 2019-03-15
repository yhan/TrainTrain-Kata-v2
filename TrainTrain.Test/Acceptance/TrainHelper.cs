using System;
using System.Collections.Generic;
using TrainTrain;
using TrainTrain.Domain;
using TrainTrain.Infrastructure.Adapter;

static internal class TrainHelper
{
    public static string BuildCoachJson(string coachNumber, int numberOfseat, int numberOfReservedSeat)
    {
        var defaultBookingReference = "reserved";
        var coachJson = String.Empty;
            
        for (int i = 1; i <= numberOfseat; i++)
        {
            var bookingReference = i <= numberOfReservedSeat ? defaultBookingReference : String.Empty;
            coachJson += $"\"{i}{coachNumber}\": {{\"booking_reference\": \"{bookingReference}\", \"seat_number\": \"{i}\", \"coach\": \"{coachNumber}\"}},";
        }
        return coachJson.Trim(',');
    }

    public static  Train BuildTrainWith_1_coach_and_0_reserved_seat(string coachId)
    {
        var trainId = new TrainId("express_2000");
        string trainTopology = BuildTrainTopology(new CoachConfig[]
        {
            new CoachConfig(coachId, 3, 0)
        });
        var seats =  TrainDataAdapter.AdaptTrainTopology(trainTopology);

        return new Train(trainId, seats);
    }

    public static string BuildTrainTopology(IEnumerable<CoachConfig> coachConfigs )
    {
        string trainTopology = "{\"seats\": {"; //+ BuildCoachJson("A", 3, 0) + "}}";
        
        foreach (var coachConfig in coachConfigs)
        {
            trainTopology += BuildCoachJson(coachConfig.CoachNumber, coachConfig.NumberOfseat, coachConfig.NumberOfReservedSeat);
        }
        return trainTopology += "}}";
    }
}


public class CoachConfig
{
    public string CoachNumber { get; }
    public int NumberOfseat { get; }
    public int NumberOfReservedSeat { get; }

    public CoachConfig(string coachNumber, int numberOfseat, int numberOfReservedSeat)
    {
        CoachNumber = coachNumber;
        NumberOfseat = numberOfseat;
        NumberOfReservedSeat = numberOfReservedSeat;
    }
}
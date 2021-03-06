﻿using Newtonsoft.Json;

namespace TrainTrain.Infrastructure
{
    public class SeatJsonPoco
    {
        [JsonProperty("booking_reference")] public string BookingReference { get; set; }

        [JsonProperty("seat_number")] public string SeatNumber { get; set; }

        [JsonProperty("coach")] public string Coach { get; set; }
    }
}
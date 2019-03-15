﻿using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class Reservation
    {
        public Reservation(TrainId trainId, string bookingReference, List<Seat> seats)
        {
            TrainId = trainId;
            BookingReference = bookingReference;
            Seats = seats;
        }

        public TrainId TrainId { get; }
        public string BookingReference { get; }
        public List<Seat> Seats { get; }
    }
}
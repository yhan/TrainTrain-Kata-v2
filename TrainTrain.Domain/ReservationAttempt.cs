namespace TrainTrain.Domain
{
    using System.Collections.Generic;

    public class ReservationAttempt
    {
        public ReservationAttempt(TrainId trainId, SeatsRequested seatsRequested, List<Seat> seats)
        {
            TrainId = trainId;
            SeatsRequested = seatsRequested;
            Seats = seats;
        }

        public TrainId TrainId { get; }
        public SeatsRequested SeatsRequested { get; }
        public List<Seat> Seats { get; }

        public string BookingReference { get; set; }

        public bool IsFulFilled()
        {
            return Seats.Count == SeatsRequested.Value;
        }

        public void AssignBookingReference(string bookingRef)
        {
            BookingReference = bookingRef;
            foreach (var availableSeat in Seats)
            {
                availableSeat.BookingRef = bookingRef;
            }
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }
    }
}
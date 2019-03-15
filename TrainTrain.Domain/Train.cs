using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Train
    {
        public Train(TrainId trainId, List<Seat> seats)
        {
            TrainId = trainId;
            Seats = seats;

            Coaches = seats.GroupBy(x => x.CoachName).ToDictionary(x => x.Key, x => new Coach(x));
        }

        public int ReservedSeats => Seats.Count(x => !string.IsNullOrWhiteSpace(x.BookingRef));
        public TrainId TrainId { get; }
        public List<Seat> Seats { get; set; }

        public Dictionary<string, Coach> Coaches { get; }


        public int GetMaxSeat()
        {
            return Seats.Count;
        }

        public bool MustNotExceedTrainCapacity(SeatsRequested seatsRequested)
        {
            return ReservedSeats + seatsRequested.Value <= Math.Floor(ThreasholdManager.GetMaxRes() * GetMaxSeat());
        }

        public ReservationAttempt BuildReservationAttempt(SeatsRequested seatsRequested)
        {
            foreach (var coach in Coaches.Values)
            {
                var reservationAttenpt =
                    new ReservationAttempt(TrainId, seatsRequested, coach.GetAvailableSeats(seatsRequested));
                if (reservationAttenpt.IsFulFilled()) return reservationAttenpt;
            }

            return new ReservationAttempt(TrainId, seatsRequested, GetReservableSeats().Take(seatsRequested.Value).ToList());
        }

        private IEnumerable<Seat> GetReservableSeats()
        {
            return Coaches.Values.SelectMany(x => x.GetReservableSeats());
        }
    }
}
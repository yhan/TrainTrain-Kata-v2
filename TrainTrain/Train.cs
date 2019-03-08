using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TrainTrain;
using Value;

namespace TrainTrain
{
    public class Train
    {

        public Train(string trainId, List<Seat> seats)
        {
            TrainId = trainId;
            Seats = seats;

            Coaches = seats.GroupBy(x => x.CoachName).ToDictionary(x => x.Key, x => new Coach(x));
        }



        public int GetMaxSeat()
        {
            return this.Seats.Count;
        }

        public int ReservedSeats => Seats.Count(x => !string.IsNullOrWhiteSpace(x.BookingRef));
        public string TrainId { get; }
        public List<Seat> Seats { get; set; }

        public bool MustNotExceedTrainCapacity(int seatRequested)
        {
            return ReservedSeats + seatRequested <= Math.Floor(ThreasholdManager.GetMaxRes() * GetMaxSeat());
        }

        public ReservationAttenpt BuildReservationAttempt(int seatRequested)
        {
            foreach (var coach in this.Coaches.Values)
            {
                var reservationAttenpt = new ReservationAttenpt(TrainId, seatRequested, coach.GetAvailableSeats(seatRequested));

                if (reservationAttenpt.IsFulFilled())
                {
                    return reservationAttenpt;
                }
            }
            return new ReservationAttenpt(TrainId, seatRequested, new List<Seat>());
        }

        public Dictionary<string, Coach> Coaches { get; }

    }

    public class Coach : ValueType<Coach>
    {
        public IEnumerable<Seat> Seats { get; }

        public Coach(IEnumerable<Seat> seats)
        {
            Seats = seats;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { new ListByValue<Seat>(this.Seats.ToList()) };
        }

        public List<Seat> GetAvailableSeats(int seatRequested)
        {
            return this.Seats.Where(x => x.IsSeatNotReserved()).Take(seatRequested).ToList();
        }
    }

    public class ReservationAttenpt
    {
        public string TrainId { get; }
        public int SeatRequested { get; }
        public List<Seat> Seats { get; }

        public string BookingReference { get; set; }

        public ReservationAttenpt(string trainId, int seatRequested, List<Seat> seats)
        {
            TrainId = trainId;
            SeatRequested = seatRequested;
            Seats = seats;
        }

        public bool IsFulFilled()
        {
            return Seats.Count == SeatRequested;
        }

        public void AssignBookingReference(string bookingRef)
        {
            BookingReference = bookingRef;
            foreach (var availableSeat in this.Seats)
            {
                availableSeat.BookingRef = bookingRef;
            }
        }
    }
}
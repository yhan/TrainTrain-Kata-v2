using System;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain.Domain
{
    public class Coach : ValueType<Coach>
    {
        public Coach(IEnumerable<Seat> seats)
        {
            Seats = seats;
        }

        public IEnumerable<Seat> Seats { get; }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {new ListByValue<Seat>(Seats.ToList())};
        }

        public List<Seat> GetAvailableSeats(SeatsRequested seatsRequested)
        {
            if (ExceedCapacity(seatsRequested)) return new List<Seat>();
            return GetReserveSeats(seatsRequested).ToList();
        }

        private IEnumerable<Seat> GetReserveSeats(SeatsRequested seatsRequested)
        {
            return Seats.Where(x => x.IsSeatNotReserved()).Take(seatsRequested.Value);
        }

        private bool ExceedCapacity(SeatsRequested seatsRequested)
        {
            var numberOfReservedSeats = Seats.Count(x => !x.IsSeatNotReserved());
            return seatsRequested.Value + numberOfReservedSeats > Seats.Count() * 0.7;
        }

        public IEnumerable<Seat> GetReservableSeats()
        {
            var numberOfReservableSeats = (int) Math.Floor(Seats.Count() * 0.7) - Seats.Count(x => !x.IsSeatNotReserved());
            return GetReserveSeats(new SeatsRequested(numberOfReservableSeats));
        }
    }
}
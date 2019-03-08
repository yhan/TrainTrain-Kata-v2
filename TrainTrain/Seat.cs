using System.Collections.Generic;
using Value;

namespace TrainTrain
{
    public class Seat : ValueType<Seat>
    {
        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get; set;  }

        public Seat(string coachName, int seatNumber) : this(coachName, seatNumber, string.Empty)
        {
        }

        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            this.CoachName = coachName;
            this.SeatNumber = seatNumber;
            this.BookingRef = bookingRef;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {this.CoachName, this.SeatNumber, this.BookingRef};
        }

        public bool IsSeatNotReserved()
        {
            return this.BookingRef == "";
        }
    }
}
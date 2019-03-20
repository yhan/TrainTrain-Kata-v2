using System.Collections.Generic;
using Value;

namespace TrainTrain.Domain
{
    public class Seat : ValueType<Seat>
    {
        public Seat(string coachName, int seatNumber) : this(coachName, seatNumber, string.Empty)
        {
        }

        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            CoachName = coachName;
            SeatNumber = seatNumber;
            BookingRef = bookingRef;
        }

        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get; set; }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { CoachName, SeatNumber, BookingRef };
        }

        public bool IsSeatNotReserved()
        {
            return BookingRef == "";
        }

        public override string ToString()
        {
            return $"\"{SeatNumber}{CoachName}\"";
        }
    }
}
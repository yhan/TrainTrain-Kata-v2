using System.Collections.Generic;
using System.Text;
using TrainTrain.Domain;

namespace TrainTrain.Infrastructure.Adapter
{
    public class SeatReservationAdapter
    {
        public string AdaptReservation(Reservation reservation)
        {
            return
                $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingReference}\", \"seats\": {DumpSeats(reservation.Seats)}}}";
        }

        private string DumpSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");

            var firstTime = true;
            foreach (var seat in seats)
            {
                if (!firstTime)
                    sb.Append(", ");
                else
                    firstTime = false;

                sb.Append($"\"{seat.SeatNumber}{seat.CoachName}\"");
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
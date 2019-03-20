using System.Text;
using TrainTrain.Domain;

namespace TrainTrain.Infrastructure.Adapter
{
    using System.Threading.Tasks;

    public class SeatReservationAdapter
    {
        private readonly IProvideReservation _hexagon;

        public SeatReservationAdapter(IProvideReservation hexagon)
        {
            _hexagon = hexagon;
        }

        public string AdaptReservation(Reservation reservation)
        {
            return
                $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingReference}\", \"seats\": {$"[{string.Join(", ", reservation.Seats)}]"}}}";
        }

        public async Task<string> PostSeatsRequest(ReservationRequestDto reservationRequestDto)
        {
            // Infra => Domain
            var trainId = new TrainId(reservationRequestDto.train_id);
            var seatsRequested = new SeatsRequested(reservationRequestDto.number_of_seats);

            var reservation = await _hexagon.ReserveAsync(trainId, seatsRequested);

            // Domain => Infra
            return AdaptReservation(reservation);
        }
    }
}
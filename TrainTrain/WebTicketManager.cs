using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain
{
    public class SeatReservationAdapter
    {
        public SeatReservationAdapter()
        {
        }

        public string AdaptReservation(Reservation reservation)
        {
            return $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingReference}\", \"seats\": {DumpSeats(reservation.Seats)}}}";
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

    public class WebTicketManager
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        public const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReference _bookingReference;
        private readonly SeatReservationAdapter _seatReservationAdapter;

        public WebTicketManager(ITrainDataService trainDataService, IBookingReference bookingReference)
        {
            _seatReservationAdapter = new SeatReservationAdapter();
            _trainDataService = trainDataService;
            _bookingReference = bookingReference;
        }

        public WebTicketManager() : this(new TrainDataAdapter(UriTrainDataService), new BookingReferenceAdapter(UriBookingReferenceService))
        {
            _seatReservationAdapter = new SeatReservationAdapter();
        }

        public async Task<Reservation> ReserveAsync(string trainId, int seatRequested)
        {
            // get the train
            var train = await _trainDataService.GetTrain(trainId);

            if (train.MustNotExceedTrainCapacity(seatRequested))
            {
                var reservationAttenpt = train.BuildReservationAttempt(seatRequested);

                if (reservationAttenpt.IsFulFilled())
                {
                    var bookingRef = await _bookingReference.GetBookingReference();

                    reservationAttenpt.AssignBookingReference(bookingRef);

                    await _trainDataService.ReserveAsync(reservationAttenpt);

                    return reservationAttenpt.Confirm();
                }
            }

            return new ReservationFailure(train.TrainId);
        }
    }

    public class ReservationFailure : Reservation
    {
        public ReservationFailure(string trainId): base(trainId, string.Empty, new List<Seat>())
        {
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain
{
    public class WebTicketManager
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        public const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReference _bookingReference;

        public WebTicketManager(ITrainDataService trainDataService, IBookingReference bookingReference)
        {
            _trainDataService = trainDataService;
            _bookingReference = bookingReference;
        }

        public WebTicketManager() : this(new TrainDataAdapter(UriTrainDataService), new BookingReferenceAdapter(UriBookingReferenceService))
        {
        }

        public async Task<string> ReserveAsync(string trainId, int seatRequested)
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

                    return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bookingRef}\", \"seats\": {DumpSeats(reservationAttenpt.Seats)}}}";
                }
            }

            return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
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
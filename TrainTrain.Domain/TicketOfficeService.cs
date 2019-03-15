using System;
using System.Threading.Tasks;
using TrainTrain.Domain.Port;

namespace TrainTrain.Domain
{
    public class TicketOfficeService
    {
        public const string UriBookingReferenceService = "http://localhost:51691/";
        public const string UriTrainDataService = "http://localhost:50680";
        private readonly IBookingReference _bookingReference;
        private readonly ITrainDataService _trainDataService;

        public TicketOfficeService(ITrainDataService trainDataService, IBookingReference bookingReference)
        {
            _trainDataService = trainDataService;
            _bookingReference = bookingReference;
        }

        public async Task<Reservation> ReserveAsync(TrainId trainId, SeatsRequested seatsRequested)
        {
            // get the train
            var train = await _trainDataService.GetTrain(trainId);

            if (train.MustNotExceedTrainCapacity(seatsRequested))
            {
                var reservationAttenpt = train.BuildReservationAttempt(seatsRequested);

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

    public class SeatsRequested
    {
        public SeatsRequested(int numberOfSeatsRequested)
        {
            if(numberOfSeatsRequested <= 0 || numberOfSeatsRequested > 20)
            {
                throw new ArgumentException($"Invalid argument {nameof(numberOfSeatsRequested)}");
            }

            Value = numberOfSeatsRequested;
        }

        public int Value { get; }
    }
}
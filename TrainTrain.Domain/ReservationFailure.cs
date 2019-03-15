using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class ReservationFailure : Reservation
    {
        public ReservationFailure(TrainId trainId) : base(trainId, string.Empty, new List<Seat>())
        {
        }
    }
}
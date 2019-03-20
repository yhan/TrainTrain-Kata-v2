namespace TrainTrain.Domain
{
    using System.Collections.Generic;

    public class ReservationFailure : Reservation
    {
        public ReservationFailure(TrainId trainId)
            : base(trainId, string.Empty, new List<Seat>())
        {
        }
    }
}
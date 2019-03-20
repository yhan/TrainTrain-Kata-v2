namespace TrainTrain.Domain.Port
{
    using System.Threading.Tasks;

    public interface ITrainDataService
    {
        Task<Train> GetTrain(TrainId trainId);

        Task ReserveAsync(ReservationAttempt reservationAttempt);
    }
}
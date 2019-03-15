using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface ITrainDataService
    {
        Task<Train> GetTrain(TrainId trainId);
        Task ReserveAsync(ReservationAttempt reservationAttempt);
    }
}
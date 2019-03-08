using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrainTrain
{
    public interface ITrainDataService
    {
        Task<Train> GetTrain(string trainId);
        Task ReserveAsync(ReservationAttenpt reservationAttenpt);
    }
}
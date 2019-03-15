using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface IBookingReference
    {
        Task<string> GetBookingReference();
    }
}
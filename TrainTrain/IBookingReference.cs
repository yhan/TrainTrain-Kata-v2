using System.Threading.Tasks;

namespace TrainTrain
{
    public interface IBookingReference
    {
        Task<string> GetBookingReference();
    }
}
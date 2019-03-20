namespace TrainTrain.Domain.Port
{
    using System.Threading.Tasks;

    public interface IBookingReference
    {
        Task<string> GetBookingReference();
    }
}
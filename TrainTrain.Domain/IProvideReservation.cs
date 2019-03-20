namespace TrainTrain.Domain
{
    using System.Threading.Tasks;

    public interface IProvideReservation
    {
        Task<Reservation> ReserveAsync(TrainId trainId, SeatsRequested seatsRequested);
    }
}
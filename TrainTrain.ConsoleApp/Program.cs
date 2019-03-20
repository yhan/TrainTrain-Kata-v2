using System;
using TrainTrain.Domain;
using TrainTrain.Infrastructure.Adapter;

namespace TrainTrain.ConsoleApp
{
    internal class Program
    {
        
        public const string UriBookingReferenceService = "http://localhost:51691/";
        public const string UriTrainDataService = "http://localhost:50680";

        private static void Main(string[] args)
        {
            var train = args[0];
            var seats = int.Parse(args[1]);

            var manager = new TicketOfficeService(new TrainDataAdapter(UriTrainDataService),
                new BookingReferenceAdapter(UriBookingReferenceService));

            var jsonResult = manager.ReserveAsync(new TrainId(train), new SeatsRequested(seats));

            Console.WriteLine(jsonResult.Result);

            Console.WriteLine("Type <enter> to exit.");
            Console.ReadLine();
        }
    }
}
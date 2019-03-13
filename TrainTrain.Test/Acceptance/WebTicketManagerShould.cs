using System;
using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace TrainTrain.Test.Acceptance
{
    public class WebTicketManagerShould
    {
        [Test]
        public async Task Return_no_resrevation_When_resereved_seats_exeed_train_capacity()
        {
            var trainId = "express_2000";
            var bookingReference = Substitute.For<IBookingReference>();
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(trainId).Returns(Task.FromResult(new Train(trainId, TrainDataAdapter.AdaptTrainTopology("{\"seats\": {" + TrainHelper.BuildCoachJson("A", 2, 0) + "}}"))));
            var manager = new WebTicketManager(trainDataService, bookingReference);
            
            int seatRequested = 3;
            var jsonResult = new SeatReservationAdapter().AdaptReservation(await manager.ReserveAsync(trainId, seatRequested));
            Check.That(jsonResult).IsEqualTo($"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }

        [Test]
        public async Task Return_resrevation_When_there_is_enough_seats()
        {
            var bokingReferenceNumber = "10";
            var trainId = "express_2000";
            var bookingReference = Substitute.For<IBookingReference>();
            bookingReference.GetBookingReference().Returns(Task.FromResult(bokingReferenceNumber));
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(trainId).Returns(Task.FromResult(new Train(trainId, TrainDataAdapter.AdaptTrainTopology("{\"seats\": {" + TrainHelper.BuildCoachJson("A", 3, 0) + "}}"))));
            var manager = new WebTicketManager(trainDataService, bookingReference);

            int seatRequested = 1;
            var jsonResult = new SeatReservationAdapter().AdaptReservation(await manager.ReserveAsync(trainId, seatRequested));
            Check.That(jsonResult).IsEqualTo($"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bokingReferenceNumber}\", \"seats\": [\"1A\"]}}");
        }

        [Test]
        public async Task Return_no_resrevation_When_resereved_seats_exeed_coach_capacity()
        {
            int seatRequested = 3;
            var trainId = "express_2000";
            var bookingReferenceNumber = "10";
            var bookingReference = Substitute.For<IBookingReference>();
            bookingReference.GetBookingReference().Returns(Task.FromResult(bookingReferenceNumber));
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(trainId).Returns(Task.FromResult(new Train(trainId, TrainDataAdapter.AdaptTrainTopology("{\"seats\": {" + TrainHelper.BuildCoachJson("A", 10, 9) + "," + TrainHelper.BuildCoachJson("B", 10, 0) + "}}"))));
            var manager = new WebTicketManager(trainDataService, bookingReference);

            var jsonResult = new SeatReservationAdapter().AdaptReservation(await manager.ReserveAsync(trainId, seatRequested));
            Check.That(jsonResult).IsEqualTo($"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"10\", \"seats\": [\"1B\", \"2B\", \"3B\"]}}");
        }


        [Test]
        public async Task Reserve_seats_on_2_coaches_When_resereved_seats_exeed_coach_capacity()
        {
            int seatRequested = 2;
            var trainId = "express_2000";
            var bookingReferenceNumber = "10";
            var bookingReference = Substitute.For<IBookingReference>();
            bookingReference.GetBookingReference().Returns(Task.FromResult(bookingReferenceNumber));
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(trainId).Returns(Task.FromResult(new Train(trainId, TrainDataAdapter.AdaptTrainTopology("{\"seats\": {" + TrainHelper.BuildCoachJson("A", 10, numberOfReservedSeat: 6) + "," + TrainHelper.BuildCoachJson("B", 10, numberOfReservedSeat: 6) + "}}"))));
            var manager = new WebTicketManager(trainDataService, bookingReference);

            var jsonResult = new SeatReservationAdapter().AdaptReservation(await manager.ReserveAsync(trainId, seatRequested));
            Check.That(jsonResult).IsEqualTo($"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"10\", \"seats\": [\"7A\", \"7B\"]}}");
        }
    }
}

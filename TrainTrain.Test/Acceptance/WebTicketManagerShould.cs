namespace TrainTrain.Test.Acceptance
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.Port;
    using Infrastructure;
    using Infrastructure.Adapter;
    using NFluent;
    using NSubstitute;
    using NUnit.Framework;

    public class WebTicketManagerShould
    {
        [Test]
        public async Task Return_no_resrevation_When_resereved_seats_exeed_train_capacity()
        {
            var bookingReferenceAdapter = Substitute.For<IBookingReference>();
            var trainDataServiceAdapter = Substitute.For<ITrainDataService>();

            var trainId = new TrainId("express_2000");
            var seatRequested = new SeatsRequested(3);

            trainDataServiceAdapter.GetTrain(trainId).Returns(Task.FromResult(new Train(trainId, TrainDataAdapter.AdaptTrainTopology("{\"seats\": {" + TrainHelper.BuildCoachJson("A", 2, 0) + "}}"))));
            IProvideReservation hexagon = new TicketOfficeService(trainDataServiceAdapter, bookingReferenceAdapter);
            var seatReservationAdapter = new SeatReservationAdapter(hexagon);
            var reservationRequestDto = new ReservationRequestDto {train_id = trainId.ToString(), number_of_seats = seatRequested.Value};
            var jsonResult = await seatReservationAdapter.PostSeatsRequest(reservationRequestDto);
            Check.That(jsonResult).IsEqualTo($"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }

        [Test]
        public async Task Return_resrevation_When_there_is_enough_seats()
        {
            var bokingReferenceNumber = "10";
            var trainId = new TrainId("express_2000");
            var bookingReferenceAdapter = Substitute.For<IBookingReference>();
            bookingReferenceAdapter.GetBookingReference().Returns(Task.FromResult(bokingReferenceNumber));
            var trainDataServiceAdapter = Substitute.For<ITrainDataService>();
            trainDataServiceAdapter.GetTrain(trainId).Returns(Task.FromResult(new Train(trainId, TrainDataAdapter.AdaptTrainTopology("{\"seats\": {" + TrainHelper.BuildCoachJson("A", 3, 0) + "}}"))));

            var seatRequested = new SeatsRequested(1);
            IProvideReservation hexagon = new TicketOfficeService(trainDataServiceAdapter, bookingReferenceAdapter);
            var seatReservationAdapter = new SeatReservationAdapter(hexagon);
            var reservationRequestDto = new ReservationRequestDto {train_id = trainId.ToString(), number_of_seats = seatRequested.Value};
            var jsonResult = await seatReservationAdapter.PostSeatsRequest(reservationRequestDto);
            Check.That(jsonResult).IsEqualTo($"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bokingReferenceNumber}\", \"seats\": [\"1A\"]}}");
        }

        [Test]
        public async Task Return_no_resrevation_When_resereved_seats_exeed_coach_capacity()
        {
            var seatRequested = new SeatsRequested(3);
            var trainId = new TrainId("express_2000");
            var bookingReferenceNumber = "10";

            var bookingReferenceAdapter = Substitute.For<IBookingReference>();
            bookingReferenceAdapter.GetBookingReference().Returns(Task.FromResult(bookingReferenceNumber));

            var trainDataServiceAdapter = Substitute.For<ITrainDataService>();
            trainDataServiceAdapter.GetTrain(trainId).Returns(Task.FromResult(new Train(trainId, TrainDataAdapter.AdaptTrainTopology("{\"seats\": {" + TrainHelper.BuildCoachJson("A", 10, 9) + "," + TrainHelper.BuildCoachJson("B", 10, 0) + "}}"))));

            var manager = new TicketOfficeService(trainDataServiceAdapter, bookingReferenceAdapter);

            IProvideReservation hexagon = new TicketOfficeService(trainDataServiceAdapter, bookingReferenceAdapter);

            var seatReservationAdapter = new SeatReservationAdapter(hexagon);
            var reservationRequestDto = new ReservationRequestDto {train_id = trainId.ToString(), number_of_seats = seatRequested.Value};

            var jsonResult = await seatReservationAdapter.PostSeatsRequest(reservationRequestDto);

            Check.That(jsonResult).IsEqualTo($"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"10\", \"seats\": [\"1B\", \"2B\", \"3B\"]}}");
        }


        [Test]
        public async Task Reserve_seats_on_2_coaches_When_resereved_seats_exeed_coach_capacity()
        {
            var seatRequested = new SeatsRequested(2);
            var trainId = new TrainId("express_2000");
            var bookingReferenceNumber = "10";

            var bookingReferenceAdapter = Substitute.For<IBookingReference>();
            bookingReferenceAdapter.GetBookingReference().Returns(Task.FromResult(bookingReferenceNumber));

            var trainDataServiceAdapter = Substitute.For<ITrainDataService>();
            trainDataServiceAdapter.GetTrain(trainId).Returns(Task.FromResult(new Train(trainId, TrainDataAdapter.AdaptTrainTopology("{\"seats\": {" + TrainHelper.BuildCoachJson("A", 10, numberOfReservedSeat: 6) + "," + TrainHelper.BuildCoachJson("B", 10, numberOfReservedSeat: 6) + "}}"))));
            var manager = new TicketOfficeService(trainDataServiceAdapter, bookingReferenceAdapter);

            IProvideReservation hexagon = new TicketOfficeService(trainDataServiceAdapter, bookingReferenceAdapter);

            var seatReservationAdapter = new SeatReservationAdapter(hexagon);
            var reservationRequestDto = new ReservationRequestDto {train_id = trainId.ToString(), number_of_seats = seatRequested.Value};

            var jsonResult = await seatReservationAdapter.PostSeatsRequest(reservationRequestDto);

            Check.That(jsonResult).IsEqualTo($"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"10\", \"seats\": [\"7A\", \"7B\"]}}");
        }
    }
}
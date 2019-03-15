using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrainTrain.Api.Models;
using TrainTrain.Domain;
using TrainTrain.Domain.Port;
using TrainTrain.Infrastructure.Adapter;

namespace TrainTrain.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReservationsController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/reservations
        [HttpPost]
        public async Task<string> Post([FromBody]ReservationRequestDto reservationRequest, [FromServices] ITrainDataService trainDataService, IBookingReference bookingReference)
        {
            var manager = new TicketOfficeService(trainDataService, bookingReference);
            var trainId = new TrainId(reservationRequest.train_id);
            var seatsRequested = new SeatsRequested(reservationRequest.number_of_seats);

            return new SeatReservationAdapter().AdaptReservation(await manager.ReserveAsync(trainId, seatsRequested));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

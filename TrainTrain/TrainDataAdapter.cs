using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainTrain
{
    public class TrainDataAdapter : ITrainDataService
    {
        private readonly string _uriTrainDataService;

        public TrainDataAdapter(string uriTrainDataService)
        {
            _uriTrainDataService = uriTrainDataService;
        }

        public async Task<Train> GetTrain(string trainId)
        {
            string jsonTrainTopology;
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_uriTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);

                // HTTP GET
                var response = await client.GetAsync($"api/data_for_train/{trainId}");
                response.EnsureSuccessStatusCode();
                jsonTrainTopology = await response.Content.ReadAsStringAsync();
            }

            return new Train(trainId, AdaptTrainTopology(jsonTrainTopology));
        }

        public async Task ReserveAsync(ReservationAttenpt reservationAttenpt)
        {
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_uriTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);


                // HTTP POST
                HttpContent resJson = new StringContent(BuildPostContent(reservationAttenpt.TrainId, reservationAttenpt.BookingReference, reservationAttenpt.Seats),
                    Encoding.UTF8, "application/json");
                var response = await client.PostAsync("reserve", resJson);

                response.EnsureSuccessStatusCode();
            }
        }

        private static string BuildPostContent(string trainId, string bookingRef, IEnumerable<Seat> availableSeats)
        {
            var seats = new StringBuilder("[");
            var firstTime = true;

            foreach (var s in availableSeats)
            {
                if (!firstTime)
                    seats.Append(", ");
                else
                    firstTime = false;

                seats.Append($"\"{s.SeatNumber}{s.CoachName}\"");
            }

            seats.Append("]");

            var result =
                $"{{\r\n\t\"train_id\": \"{trainId}\",\r\n\t\"seats\": {seats},\r\n\t\"booking_reference\": \"{bookingRef}\"\r\n}}";

            return result;
        }

        public static List<Seat> AdaptTrainTopology(string trainTopologie)
        {
            var seats = new List<Seat>();
            //var sample =
            //"{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";

            // Forced to workaround with dynamic parsing since the received JSON is invalid format ;-(
            dynamic parsed = JsonConvert.DeserializeObject(trainTopologie);

            foreach (var token in ((Newtonsoft.Json.Linq.JContainer) parsed))
            {
                var allStuffs = ((Newtonsoft.Json.Linq.JObject) ((Newtonsoft.Json.Linq.JContainer) token).First);

                foreach (var stuff in allStuffs)
                {
                    var seatPoco = stuff.Value.ToObject<SeatJsonPoco>();
                    seats.Add(new Seat(seatPoco.Coach, Int32.Parse(seatPoco.SeatNumber), seatPoco.BookingReference))
                        ;
                }
            }

            return seats;
        }
    }
}
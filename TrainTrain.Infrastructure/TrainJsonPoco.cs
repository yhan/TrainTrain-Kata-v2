using System.Collections.Generic;

namespace TrainTrain.Infrastructure
{
    public class TrainJsonPoco
    {
        public TrainJsonPoco()
        {
            Seats = new List<SeatJsonPoco>();
        }

        public List<SeatJsonPoco> Seats { get; set; }
    }
}
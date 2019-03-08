using System.Collections.Generic;

namespace TrainTrain
{
    public class TrainJsonPoco
    {
        public List<SeatJsonPoco> Seats { get; set;  }

        public TrainJsonPoco()
        {
            this.Seats = new List<SeatJsonPoco>();
        }
    }
}
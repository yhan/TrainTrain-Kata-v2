using System.Collections.Generic;
using System.Linq;
using NFluent;
using NUnit.Framework;

namespace TrainTrain.Test.Acceptance
{
    class TrainTrainShould
    {
        [Test]
        public void Expose_coaches()
        {
            var coachId = "A";
            var train = TrainHelper.BuildTrainWith_1_coach_and_0_reserved_seat(coachId);

            Check.That(train.Coaches).HasSize(1);

            var coach = train.Coaches.Single().Value;
            Check.That(coach.Seats).HasSize(3);
            Check.That(coach.Seats.Select(x => x.CoachName).All(x => x == coachId)).IsTrue();

            Check.That(coach.Seats).ContainsExactly(new Seat(coachId, 1, ""), new Seat(coachId, 2, ""),  new Seat(coachId, 3, ""));
        }
    }
}
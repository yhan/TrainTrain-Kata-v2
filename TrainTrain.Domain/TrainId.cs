using System;
using System.Collections.Generic;
using Value;

namespace TrainTrain.Domain
{
    public class TrainId :ValueType<TrainId>
    {
        private readonly string _trainId;

        public override string ToString()
        {
            return _trainId;
        }

        public TrainId(string trainId)
        {
            if (string.IsNullOrWhiteSpace(trainId))
            {
                throw new ArgumentException($"Invalid argument {nameof(trainId)}");
            }

            _trainId = trainId;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {this._trainId};
        }
    }
}
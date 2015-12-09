using System;

namespace Raven.Database.TimeSeries.Raft.Commands
{
    public class AppendPointCommand : TimeSeriesCommand
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public DateTimeOffset Time { get; set; }
        public double[] Values { get; set; }

        public override void Apply(TimeSeriesStorage storage)
        {
            using (var writer = storage.CreateWriter())
            {
                writer.Append(Type, Key, Time, Values);
                writer.Commit();
            }
        }
    }
}
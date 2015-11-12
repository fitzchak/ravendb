using System;
using System.Threading.Tasks;

namespace Raven.Database.TimeSeries.Raft.Commands
{
    public class AppendPointCommand : TimeSeriesCommand
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public DateTimeOffset Time { get; set; }
        public double[] Values { get; set; }
    }
}
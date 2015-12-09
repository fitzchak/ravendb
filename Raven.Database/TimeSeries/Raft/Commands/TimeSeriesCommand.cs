using System.Threading.Tasks;
using Rachis.Commands;

namespace Raven.Database.TimeSeries.Raft.Commands
{
    public abstract class TimeSeriesCommand : Command
    {
        public string TimeSeries { get; set; }

        public TimeSeriesCommand()
        {
            Completion = new TaskCompletionSource<object>();
        }

        public abstract void Apply(TimeSeriesStorage storage);
    }
}
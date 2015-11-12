using System;
using Rachis.Commands;
using Raven.Database.Server.Tenancy;

namespace Raven.Database.TimeSeries.Raft.Handlers
{
    public abstract class TimeSeriesCommandHandler<TCommand> : TimeSeriesCommandHandler
        where TCommand : Command
    {
        protected TimeSeriesCommandHandler(TimeSeriesStorage timeSeries, TimeSeriesLandlord landlord)
            : base(timeSeries, landlord)
        {
        }

        public Type HandledCommandType
        {
            get { return typeof(TCommand); }
        }

        public abstract void Handle(TCommand command);

        public override void Handle(object command)
        {
            Handle((TCommand)command);
        }
    }

    public abstract class TimeSeriesCommandHandler
    {
        protected TimeSeriesStorage TimeSeries { get; private set; }

        protected TimeSeriesLandlord Landlord { get; private set; }

        protected TimeSeriesCommandHandler(TimeSeriesStorage timeSeries, TimeSeriesLandlord landlord)
        {
            TimeSeries = timeSeries;
            Landlord = landlord;
        }

        public abstract void Handle(object command);
    }
}
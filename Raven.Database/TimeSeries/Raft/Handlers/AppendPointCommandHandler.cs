// -----------------------------------------------------------------------
//  <copyright file="AppendPointCommand.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System.Threading.Tasks;
using Raven.Database.Server.Tenancy;
using Raven.Database.TimeSeries.Raft.Commands;

namespace Raven.Database.TimeSeries.Raft.Handlers
{
    public class AppendPointCommandHandler : TimeSeriesCommandHandler<AppendPointCommand>
    {
        public AppendPointCommandHandler(TimeSeriesStorage timeSeries, TimeSeriesLandlord landlord) : base(timeSeries, landlord)
        {
        }

        public override void Handle(AppendPointCommand command)
        {
            Task<TimeSeriesStorage> resourceTask;
            if (Landlord.TryGetOrCreateResourceStore(command.TimeSeries, out resourceTask))
            {
                var ts = resourceTask.Result;
                using (var writer = ts.CreateWriter())
                {
                    command.CommandResult = writer.Append(command.Type, command.Key, command.Time, command.Values);
                }
            }
        }
    }
}
using System.Collections.Generic;
using Raven.Abstractions.TimeSeries;
using Raven.Database.Config;
using Raven.Database.TimeSeries;

namespace Raven.Tests.TimeSeries
{
    public class TimeSeriesTest
    {
        protected readonly List<TimeSeriesStorage> Storages = new List<TimeSeriesStorage>();

        public TimeSeriesStorage GetStorage(int port = 8079, bool createSimpleType = true)
        {
            var storage = new TimeSeriesStorage(string.Format("http://localhost:{0}/", port), "TimeSeriesTest-" + (Storages.Count + 1), new RavenConfiguration { RunInMemory = true });
            using (var writer = storage.CreateWriter())
            {
                if (createSimpleType == false)
                {
                    writer.CreateType("Simple", new[] {"Value"});
                }
                writer.Commit();
            }
            Storages.Add(storage);
            return storage;
        }
    }
}
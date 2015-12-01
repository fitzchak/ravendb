using System;
using System.Threading.Tasks;
using Xunit;

namespace Raven.Tests.TimeSeries
{
    public class TimeSeriesRaft : RavenBaseTimeSeriesTest
    {
        [Fact]
        public async Task Replication_setup_should_work()
        {
            using (var storeA = NewRemoteTimeSeriesStore(port: 8079))
            using (var storeB = NewRemoteTimeSeriesStore(port: 8078))
            {
                await storeA.BootstrapLeader();

                await storeA.CreateTypeAsync("SmartWatch", new [] { "Heartrate", "Geo Latitude", "Geo Longitude" });
                await storeA.AppendAsync("SmartWatch", "Watch-123456", DateTimeOffset.UtcNow, new [] { 111d, 222d, 333d });
            }
        }
    }
}
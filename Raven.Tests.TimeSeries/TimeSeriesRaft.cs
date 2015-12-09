using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Raven.Tests.TimeSeries
{
    public class TimeSeriesRaft : RavenBaseTimeSeriesTest
    {
        [Fact]
        public async Task WriteToLeader()
        {
            using (var storeA = NewRemoteTimeSeriesStore(port: 8079))
            using (var storeB = NewRemoteTimeSeriesStore(port: 8078))
            {
                await storeA.BootstrapLeader();
                await storeA.JoinTopology(storeB.Name, storeB.Url);

                await storeA.CreateTypeAsync("SmartWatch", new [] { "Heartrate", "Geo Latitude", "Geo Longitude" });
                await storeA.AppendAsync("SmartWatch", "Watch-123456", DateTimeOffset.UtcNow, new [] { 111d, 222d, 333d });

                SpinWait.SpinUntil(() => false, TimeSpan.FromSeconds(5));

                var types = await storeB.Advanced.GetTypes();
                Assert.Equal("SmartWatch", types.Single().Type);

                var points = await storeB.Advanced.GetPoints("SmartWatch", "Watch-123456");
                Assert.Equal(new[] { 111d, 222d, 333d }, points.Single().Values);
            }
        }

        [Fact]
        public async Task WriteToFollower()
        {
            using (var storeA = NewRemoteTimeSeriesStore(port: 8079))
            using (var storeB = NewRemoteTimeSeriesStore(port: 8078))
            {
                await storeA.BootstrapLeader();
                await storeA.JoinTopology(storeB.Name, storeB.Url);

                await storeB.CreateTypeAsync("SmartWatch", new [] { "Heartrate", "Geo Latitude", "Geo Longitude" });
                await storeB.AppendAsync("SmartWatch", "Watch-123456", DateTimeOffset.UtcNow, new [] { 111d, 222d, 333d });

                SpinWait.SpinUntil(() => false, TimeSpan.FromSeconds(5));

                var types = await storeA.Advanced.GetTypes();
                Assert.Equal("SmartWatch", types.Single().Type);

                var points = await storeA.Advanced.GetPoints("SmartWatch", "Watch-123456");
                Assert.Equal(new[] { 111d, 222d, 333d }, points.Single().Values);
            }
        }
    }
}
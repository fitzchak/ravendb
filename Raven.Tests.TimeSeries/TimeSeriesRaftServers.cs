using System;
using Xunit;

namespace Raven.Tests.TimeSeries
{
    public class TimeSeriesRaftServers : TimeSeriesTest
    {
        [Fact]
        public void TestCluster()
        {
            using (var storageA = GetStorage(port: 8079, createSimpleType: false))
            using (var storageB = GetStorage(port: 8078, createSimpleType: false))
            {
                storageA.RaftClient.RaftBootstrap();

                using (var writer = storageA.CreateWriter())
                {
                    writer.CreateType("SmartWatch", new[] { "Heartrate", "Geo Latitude", "Geo Longitude" });
                    writer.Append("SmartWatch", "Watch-123456", DateTimeOffset.UtcNow, new[] { 111d, 222d, 333d });
                    writer.Commit();
                }
            }
        }
    }
}
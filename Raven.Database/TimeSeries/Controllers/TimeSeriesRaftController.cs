using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Abstractions.TimeSeries;
using Raven.Database.Server.WebApi.Attributes;

namespace Raven.Database.TimeSeries.Controllers
{
    public class TimeSeriesRaftController : BaseTimeSeriesApiController
    {
        [RavenRoute("ts/{timeSeriesName}/raft/bootstrap")]
        [HttpPost]
        public HttpResponseMessage RaftBootstrap()
        {
            TimeSeries.RaftClient.RaftBootstrap();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
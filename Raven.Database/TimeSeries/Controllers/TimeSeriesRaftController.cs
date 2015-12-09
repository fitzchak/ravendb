using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Rachis.Transport;
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
            return GetEmptyMessage();
        }

        [HttpGet]
        [Route("ts/{timeSeriesName}/raft/topology")]
        public HttpResponseMessage Topology()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                TimeSeries.RaftClient.RaftEngine.CurrentLeader,
                TimeSeries.RaftClient.RaftEngine.PersistentState.CurrentTerm,
                TimeSeries.RaftClient.RaftEngine.State,
                TimeSeries.RaftClient.RaftEngine.CommitIndex,
                TimeSeries.RaftClient.RaftEngine.CurrentTopology.AllVotingNodes,
                TimeSeries.RaftClient.RaftEngine.CurrentTopology.PromotableNodes,
                TimeSeries.RaftClient.RaftEngine.CurrentTopology.NonVotingNodes
            });
        }

        [HttpPost]
        [Route("ts/{timeSeriesName}/raft/join")]
        public async Task<HttpResponseMessage> Join(string url, string name = null)
        {
            Uri uri = null;
            if (string.IsNullOrEmpty(url) == false)
            {
                uri = new Uri(url);
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = uri.Host + (uri.IsDefaultPort ? "" : ":" + uri.Port);
                }
            }
            await TimeSeries.RaftClient.RaftEngine.AddToClusterAsync(new NodeConnectionInfo
            {
                Name = name,
                Uri = uri
            }).ConfigureAwait(false);
            return GetEmptyMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        [Route("ts/{timeSeriesName}/raft/leave")]
        public async Task<HttpResponseMessage> Leave([FromUri] string name)
        {
            await TimeSeries.RaftClient.RaftEngine.RemoveFromClusterAsync(new NodeConnectionInfo
            {
                Name = name
            }).ConfigureAwait(false);
            return GetEmptyMessage(HttpStatusCode.Accepted);
        }
    }
}
// -----------------------------------------------------------------------
//  <copyright file="BaseTimeSeriesApiController.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Rachis.Utils;
using Raven.Database.Common;
using Raven.Database.Server.Tenancy;

namespace Raven.Database.TimeSeries.Controllers
{
    public abstract class BaseTimeSeriesApiController : ResourceApiController<TimeSeriesStorage, TimeSeriesLandlord>
    {
        public bool UseRaft { get; } = true;

        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            try
            {
                return await base.ExecuteAsync(controllerContext, cancellationToken).ConfigureAwait(false);
            }
            catch (NotLeadingException)
            {
                var currentLeader = TimeSeries.RaftClient.RaftEngine.CurrentLeader;
                if (currentLeader == null)
                {
                    return GetMessageWithString("No current leader, try again later", HttpStatusCode.PreconditionFailed);
                }
                var leaderNode = TimeSeries.RaftClient.RaftEngine.CurrentTopology.GetNodeByName(currentLeader);
                if (leaderNode == null)
                {
                    return GetMessageWithString("Current leader " + currentLeader + " is not found in the topology. This should not happen.", HttpStatusCode.PreconditionFailed);
                }
                return new HttpResponseMessage(HttpStatusCode.Redirect)
                {
                    Headers =
                    {
                        Location = leaderNode.Uri
                    }
                };
            }
        }

        protected string TimeSeriesName
        {
            get { return ResourceName; }
        }

        protected TimeSeriesStorage TimeSeries
        {
            get { return Resource; }
        }

        public override ResourceType ResourceType
        {
            get { return ResourceType.TimeSeries; }
        }

        public override void MarkRequestDuration(long duration)
        {
            if (Resource == null)
                return;

            Resource.MetricsTimeSeries.RequestDurationMetric.Update(duration);
        }
    }
}

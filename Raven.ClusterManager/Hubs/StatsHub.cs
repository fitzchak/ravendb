using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Raven.ClusterManager.Hubs
{
	public class StatsHub : Hub
	{
		public override Task OnConnected()
		{
			return Task.Factory.StartNew(() =>
			{
				Clients.Caller.loadInitialData();
			});
		}
	}
}
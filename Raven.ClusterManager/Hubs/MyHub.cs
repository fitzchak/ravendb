using Microsoft.AspNet.SignalR;

namespace Raven.ClusterManager.Hubs
{
	public class MyHub : Hub
	{
		private readonly IExampleSharedService exampleSharedService;

		public MyHub(IExampleSharedService exampleSharedService)
		{
			this.exampleSharedService = exampleSharedService;
		}
	}
}
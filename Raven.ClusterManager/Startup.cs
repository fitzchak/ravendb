using Microsoft.AspNet.SignalR;
using Owin;

namespace Raven.ClusterManager
{
	public class Startup
	{
		private readonly IExampleSharedService exampleSharedService;

		public Startup(IExampleSharedService exampleSharedService)
		{
			//TODO guard clause
			this.exampleSharedService = exampleSharedService;
		}

		public void Configuration(IAppBuilder appBuilder)
		{
			var dependencyResolver = new DefaultDependencyResolver();
			dependencyResolver.Register(typeof(IExampleSharedService), () => exampleSharedService);

			appBuilder
				.MapHubs(new HubConfiguration { Resolver = dependencyResolver }) //Will handle requests that start with /signalr, and pass through all other requests to Nancy
				.UseNancy(new Bootstrapper(exampleSharedService));
		}
	}
}
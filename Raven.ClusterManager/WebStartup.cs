using Microsoft.AspNet.SignalR;
using Owin;
using Raven.Client;

namespace Raven.ClusterManager
{
	public class WebStartup
	{
		private readonly IExampleSharedService exampleSharedService;
		private readonly IDocumentStore store;

		public WebStartup(IExampleSharedService exampleSharedService, IDocumentStore store)
		{
			//TODO guard clause
			this.exampleSharedService = exampleSharedService;
			this.store = store;
		}

		public void Configuration(IAppBuilder appBuilder)
		{
			var dependencyResolver = new DefaultDependencyResolver();
			dependencyResolver.Register(typeof(IExampleSharedService), () => exampleSharedService);

			appBuilder
				.MapHubs(new HubConfiguration { Resolver = dependencyResolver }) //Will handle requests that start with /signalr, and pass through all other requests to Nancy
				.UseNancy(new Bootstrapper(exampleSharedService, store));
		}
	}
}
using Owin;

namespace Raven.ClusterManager
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			appBuilder.UseNancy(new Bootstrapper());
		}
	}
}
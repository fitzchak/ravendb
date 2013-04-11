using System;
using Microsoft.Owin.Hosting;
using Raven.Database.Server;

namespace Raven.ClusterManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			const int port = 9020;
			NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(port);

			var defaultServiceProvider = new Microsoft.Owin.Hosting.Services.DefaultServiceProvider();
			defaultServiceProvider.AddInstance<IExampleSharedService>(new ExampleSharedService());

			using (WebApplication.Start<Startup>(defaultServiceProvider, port))
			{
				while (true)
				{
					Console.WriteLine("Available commands: q.");
					var line = Console.ReadLine();
					if (line == "q")
					{
						break;
					}
				}
			}
		}
	}
}
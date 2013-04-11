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

			using (WebApplication.Start<Startup>(port))
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
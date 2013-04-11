using System;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Services;
using Raven.Client;
using Raven.Client.Document;
using Raven.Database.Server;

namespace Raven.ClusterManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			const int port = 9020;
			NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(port);

			var serviceProvider = DefaultServices
				.Create(defaultServiceProvider =>
				{
					defaultServiceProvider.AddInstance<IExampleSharedService>(new ExampleSharedService());
					defaultServiceProvider.AddInstance<IDocumentStore>(
						new DocumentStore {ConnectionStringName = "RavenDB"}.Initialize());
				});

			using (WebApplication.Start<WebStartup>(serviceProvider, port))
			{
				while (true)
				{
					Console.WriteLine("Available commands: q.");
					string line = Console.ReadLine();
					if (line == "q")
					{
						break;
					}
				}
			}
		}
	}
}
﻿// -----------------------------------------------------------------------
//  <copyright file="Bootstrapper.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using System.IO;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Responses;
using Nancy.TinyIoc;
using Raven.Abstractions.Util;
using Raven.Bundles.Replication.Tasks;
using Raven.Client;
using Raven.ClusterManager.Models;
using Raven.ClusterManager.Tasks;

namespace Raven.ClusterManager
{
	public class Bootstrapper : DefaultNancyBootstrapper
	{
		private readonly IExampleSharedService exampleSharedService;
		private readonly IDocumentStore store;

		public Bootstrapper(IExampleSharedService exampleSharedService, IDocumentStore store)
		{
			this.exampleSharedService = exampleSharedService;
			this.store = store;
		}

		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			// Disable the AutoRegister since we're not using it
			// base.ConfigureApplicationContainer(container);
			container.Register(this.exampleSharedService);

			store.Conventions.RegisterIdConvention<ServerRecord>((s, commands, serverRecord) => "serverRecords/" + ReplicationTask.EscapeDestinationName(serverRecord.Url));
			store.Conventions.RegisterIdConvention<DatabaseRecord>((s, commands, databaseRecord) => databaseRecord.ServerId + "/" + databaseRecord.Name);

			store.Conventions.RegisterAsyncIdConvention<ServerRecord>((s, commands, serverRecord) => new CompletedTask<string>("serverRecords/" + ReplicationTask.EscapeDestinationName(serverRecord.Url)));
			store.Conventions.RegisterAsyncIdConvention<DatabaseRecord>((s, commands, databaseRecord) => new CompletedTask<string>(databaseRecord.ServerId + "/" + databaseRecord.Name));

			container.Register(store);
		}

		protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
		{
			base.ConfigureRequestContainer(container, context);

			var store = container.Resolve<IDocumentStore>();

			container.Register(store.OpenSession());
			container.Register(store.OpenAsyncSession());
		}

		protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
		{
			base.RequestStartup(container, pipelines, context);

			pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
			{
				using (var documentSession = container.Resolve<IDocumentSession>())
				{
					if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
					{
						documentSession.SaveChanges();
					}
				}

				using (var documentSession = container.Resolve<IAsyncDocumentSession>())
				{
					if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
					{
						documentSession.SaveChangesAsync().Wait();
					}
				}
			});
		}

		protected override void ConfigureConventions(NancyConventions conventions)
		{
			base.ConfigureConventions(conventions);

			string rootAppPath = null;
			conventions.StaticContentsConventions.Add((ctx, rootPath) =>
			{
				var path = ctx.Request.Url.Path.Substring(1); // Remove the starting '/'.
				if (path == "")
					path = "index.html";

				if (path.StartsWith("api/"))
					return null;

#if DEBUG
				// For debug, we want to edit the static HTML files and serve them without recompiling the assembly.
				if (rootAppPath == null)
				{
					rootAppPath = rootPath;
					do
					{
						rootAppPath = Path.GetDirectoryName(rootAppPath);
					} while (rootAppPath.EndsWith("Raven.ClusterManager") == false);
					GenericFileResponse.SafePaths.Add(rootAppPath);
				}
				var filePath = Path.Combine(rootAppPath, @"Assets", path);
				if (File.Exists(filePath))
				{
					return new GenericFileResponse(filePath);
				}
				else
				{
					throw new InvalidOperationException("404 - File does not found: " + filePath);
				}
#endif

				var resourcePath = @"Raven.ClusterManager.Assets";
				var directoryName = Path.GetDirectoryName(path);
				if (string.IsNullOrEmpty(directoryName) == false)
				{
					resourcePath += "." + directoryName.Replace('\\', '.');
				}
				var fileName = Path.GetFileName(path);

				var assembly = typeof (Bootstrapper).Assembly;
				if (assembly.GetManifestResourceInfo(resourcePath + "." + fileName) == null)
				{
					fileName = "index.html";
					resourcePath = @"Raven.ClusterManager.Assets";
				}
				
				return new EmbeddedFileResponse(assembly, resourcePath, fileName);
			});
		}

		protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
		{
			var store = container.Resolve<IDocumentStore>();
			var healthMonitorTask = new HealthMonitorTask(store);
		}
	}
}
// -----------------------------------------------------------------------
//  <copyright file="RavenClientTest.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using Raven.Client.Document;

namespace Raven.Client.Tests
{
	public class RavenClientTest : IDisposable
	{
		protected readonly IDocumentStore Store;

		public RavenClientTest(int port = 8081)
		{
			Store = new DocumentStore {Url = "http://localhost:" + port}.Initialize();
		}

		protected IDocumentSession OpenSession()
		{
			return Store.OpenSession();
		}

		public void Dispose()
		{
			Store.Dispose();
		}
	}
}
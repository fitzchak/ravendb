using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Raven.Client.Embedded;
using Xunit;

namespace Raven.ClusterManager.Tests
{
	public class ClusterTests
	{
		[Fact]
		public async Task ExampleTest()
		{
			using (var store = new EmbeddableDocumentStore {RunInMemory = true}.Initialize())
			{
				// The nice thing about this test is that it uses the complete OWIN pipeline without hitting network
				var testServer = TestServer.Create(builder => new WebStartup(new FakeExampleSharedService(), store));
				var response = await testServer.HttpClient.PostAsync("http://localhost/api/servers/credentials/test?Server=x", null);
				Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
			}
		}

		private class FakeExampleSharedService : IExampleSharedService
		{
			public void Foo()
			{
				throw new System.NotImplementedException();
			}
		}
	}
}
// -----------------------------------------------------------------------
//  <copyright file="ClientTests.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using Xunit;

namespace Raven.Client.Tests
{
	public class DateAndTimeTests : RavenClientTest
	{
		public DateAndTimeTests()
		{
			using (var session = Store.OpenSession())
			{
				for (int i = 0; i < 10; i++)
				{
					session.Store(new Post
					{
						Id = "posts/" + i,
						PublishedAt = DateTimeOffset.Now.AddDays(i * -1),
					});
				}
				session.SaveChanges();
			}
		}


		[Fact]
		public void CanLoad()
		{
			using (var session = Store.OpenSession())
			{
				var post = session.Load<Post>("posts/1");
				Assert.NotNull(post);
			}
		}

		private class Post
		{
			public string Id { get; set; }
			public DateTimeOffset PublishedAt { get; set; }
		}
	}
}
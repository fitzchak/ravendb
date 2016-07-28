// -----------------------------------------------------------------------
//  <copyright file="SubscriptionDoesNotExistException.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.Net;
using Raven.Client.Documents.Subscriptions;

namespace Raven.Server.Documents.Subscriptions
{
    public class SubscriptionDoesNotExistException : SubscriptionException
    {
        public static HttpStatusCode RelevantHttpStatusCode = HttpStatusCode.NotFound;

        public SubscriptionDoesNotExistException() : base(RelevantHttpStatusCode)
        {
        }

        public SubscriptionDoesNotExistException(string message)
            : base(message, RelevantHttpStatusCode)
        {
        }

        public SubscriptionDoesNotExistException(string message, Exception inner)
            : base(message, inner, RelevantHttpStatusCode)
        {
        }

    }
}

// -----------------------------------------------------------------------
//  <copyright file="SubscriptionAlreadyInUseException.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.Net;
using Raven.Client.Documents.Subscriptions;

namespace Raven.Server.Documents.Subscriptions
{
    public class SubscriptionInUseException : SubscriptionException
    {
        public static HttpStatusCode RelavantHttpStatusCode = HttpStatusCode.Gone;

        public SubscriptionInUseException() : base(RelavantHttpStatusCode)
        {
        }

        public SubscriptionInUseException(string message)
            : base(message, RelavantHttpStatusCode)
        {
        }

        public SubscriptionInUseException(string message, Exception inner)
            : base(message, inner, RelavantHttpStatusCode)
        {
        }

    }
}

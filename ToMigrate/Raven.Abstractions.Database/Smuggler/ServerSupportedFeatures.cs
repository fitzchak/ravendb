// -----------------------------------------------------------------------
//  <copyright file="Smuggler.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Raven.Abstractions.Database.Smuggler
{
    public class ServerSupportedFeatures
    {
        public bool IsTransformersSupported { get; set; }
        public bool IsDocsStreamingSupported { get; set; }
        public bool IsIdentitiesSmugglingSupported { get; set; }
    }
}

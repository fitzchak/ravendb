﻿using System;
using Raven.Client.Documents.Replication.Messages;
using Sparrow.Json;

namespace Raven.Client.Documents.Commands
{
    public class GetConflictsResult
    {
        public string Key { get; set; }

        public Conflict[] Results { get; internal set; }

        public long LargestEtag { get; set; } //etag of the conflict itself

        public class Conflict
        {
            public ArraySegment<ChangeVectorEntry> ChangeVector { get; set; }

            public BlittableJsonReaderObject Doc { get; set; }
        }
    }
}

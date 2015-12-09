// -----------------------------------------------------------------------
//  <copyright file="ClusterStateMachine.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Rachis;
using Rachis.Commands;
using Rachis.Interfaces;
using Rachis.Messages;
using Raven.Abstractions.Data;
using Raven.Abstractions.Logging;
using Raven.Database.Impl;
using Raven.Database.Raft.Commands;
using Raven.Database.Raft.Storage.Handlers;
using Raven.Database.Server.Tenancy;
using Raven.Database.Storage;
using Raven.Database.TimeSeries.Raft.Commands;
using Raven.Database.Util;
using Raven.Imports.Newtonsoft.Json;
using Raven.Json.Linq;

namespace Raven.Database.TimeSeries.Raft
{
    public class TimeSeriesStateMachine : IRaftStateMachine
    {
        private readonly TimeSeriesStorage storage;

        public TimeSeriesStateMachine(TimeSeriesStorage storage)
        {
            this.storage = storage;
        }

        public void Dispose()
        {

        }

        public long LastAppliedIndex
        {
            get { return storage.CreateReader().ReadLastAppliedIndex(); }
            private set
            {
                using (var writer = storage.CreateWriter())
                {
                    writer.SetLastAppliedIndex(value);
                    writer.Commit();
                }
            }
        }

        public void Apply(LogEntry entry, Command cmd)
        {
            if (LastAppliedIndex >= entry.Index)
                throw new InvalidOperationException("Already applied " + entry.Index);

            LastAppliedIndex = entry.Index;

            var timeSeriesCommand = cmd as TimeSeriesCommand;
            if (timeSeriesCommand != null)
            {
                timeSeriesCommand.Apply(storage);
            }
            else
            {
                
            }
        }

        public bool SupportSnapshots { get { return false; } }
        public void CreateSnapshot(long index, long term, ManualResetEventSlim allowFurtherModifications)
        {
            throw new NotSupportedException();
        }

        public ISnapshotWriter GetSnapshotWriter()
        {
            throw new NotSupportedException();
        }

        public void ApplySnapshot(long term, long index, Stream stream)
        {
            throw new NotSupportedException();
        }
    }
}
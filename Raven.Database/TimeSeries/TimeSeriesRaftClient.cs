using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Rachis;
using Rachis.Storage;
using Rachis.Transport;
using Rachis.Utils;
using Raven.Client.TimeSeries;
using Raven.Database.Config;
using Raven.Database.Raft.Util;
using Raven.Database.TimeSeries.Raft;
using Raven.Database.TimeSeries.Raft.Commands;
using Voron;

namespace Raven.Database.TimeSeries
{
    public class TimeSeriesRaftClient
    {
        private readonly TimeSeriesStorage timeSeriesStorage;
        private readonly ConcurrentDictionary<string, TimeSeriesStore> stores = new ConcurrentDictionary<string, TimeSeriesStore>();

        public RaftEngine RaftEngine { get; set; }

        public TimeSeriesRaftClient(TimeSeriesStorage timeSeriesStorage)
        {
            this.timeSeriesStorage = timeSeriesStorage;
            var raftEngineOptions = GetRaftOptions();
            RaftEngine = new RaftEngine(raftEngineOptions);
        }

        private RaftEngineOptions GetRaftOptions()
        {
            StorageEnvironmentOptions raftOptions;
            var configuration = timeSeriesStorage.Configuration;
            if (configuration.RunInMemory)
            {
                raftOptions = StorageEnvironmentOptions.CreateMemoryOnly();
            }
            else
            {
                var directoryPath = Path.Combine(configuration.TimeSeries.DataDirectory, "Raft");
                if (Directory.Exists(directoryPath) == false)
                    Directory.CreateDirectory(directoryPath);

                raftOptions = StorageEnvironmentOptions.ForPath(directoryPath);
            }
            var connectionInfo = new NodeConnectionInfo { Name = timeSeriesStorage.Name, Uri = new Uri(timeSeriesStorage.TimeSeriesUrl) };
            var transport = new HttpTransport(connectionInfo.Name, CancellationToken.None);
            var raftEngineOptions = new RaftEngineOptions(connectionInfo, raftOptions, transport, new TimeSeriesStateMachine(timeSeriesStorage))
            {
                ElectionTimeout = configuration.TimeSeries.ElectionTimeout,
                HeartbeatTimeout = configuration.TimeSeries.HeartbeatTimeout,
                MaxLogLengthBeforeCompaction = configuration.TimeSeries.MaxLogLengthBeforeCompaction,
                MaxEntriesPerRequest = configuration.TimeSeries.MaxEntriesPerRequest,
                MaxStepDownDrainTime = configuration.TimeSeries.MaxStepDownDrainTime,
            };
            return raftEngineOptions;
        }


        public async Task SendPutType(string type, string[] fields)
        {
            try
            {
                var command = new PutTypeCommand {Type = type, Fields = fields};
                RaftEngine.AppendCommand(command);

                await command.Completion.Task.ConfigureAwait(false);
            }
            catch (NotLeadingException)
            {
                var leaderNode = RaftEngine.GetLeaderNode(WaitForLeaderTimeoutInSeconds);

                var store = stores.GetOrAdd(leaderNode.Uri.AbsoluteUri, x => new TimeSeriesStore
                {
                    Url = leaderNode.Uri.AbsoluteUri,
                }.Initialize());

                await store.CreateTypeAsync(type, fields).ConfigureAwait(false);
            }
        }

        public int WaitForLeaderTimeoutInSeconds { get; } = 30;

        public void RaftBootstrap()
        {
            var options = GetRaftOptions();
            options.StorageOptions.OwnsPagers = false;
            PersistentState.ClusterBootstrap(options);
        }
    }
}
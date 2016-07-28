using System;

namespace Raven.Client.Documents.Indexing
{
    public class IndexingPerformanceOperation
    {
        public IndexingPerformanceOperation(TimeSpan duration)
        {
            DurationInMilliseconds = Math.Round(duration.TotalMilliseconds, 2);
        }

        public string Name { get; set; }

        public double DurationInMilliseconds { get; }

        public IndexingPerformanceOperation[] Operations { get; set; }
    }
}
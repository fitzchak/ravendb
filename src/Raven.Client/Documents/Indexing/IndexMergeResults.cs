using System.Collections.Generic;

namespace Raven.Client.Documents.Indexing
{
    public class IndexMergeResults
    {
        public Dictionary<string, string> Unmergables = new Dictionary<string, string>(); // index name, reason

        public List<MergeSuggestions> Suggestions = new List<MergeSuggestions>();
    }
}

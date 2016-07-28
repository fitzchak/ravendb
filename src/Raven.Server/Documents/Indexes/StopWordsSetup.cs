using System.Collections.Generic;

namespace Raven.Server.Documents.Indexes
{
    public class StopWordsSetup
    {
        public string Id { get; set; }
        public List<string> StopWords { get; set; } 
    }
}

using System.Collections.Generic;
using System.Diagnostics;

namespace Raven.Client.Authentication
{
    public class AccessToken
    {
        public string Token;
        public string Name;
        public Dictionary<string, AccessModes> AuthorizedDatabases;
        public long Issued;
        public bool IsServerAdminAuthorized;

        public static readonly long MaxAge = Stopwatch.Frequency * 60 * 30; // 30 minutes

        public bool IsExpired
        {
            get
            {
                var ticks = Stopwatch.GetTimestamp() - Issued;
                return ticks > MaxAge;
            }
        }
    }
}

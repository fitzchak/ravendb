using System;
using System.Text.RegularExpressions;

namespace Raven.Client.Documents
{
    ///<summary>
    /// Methods to create multitenant databases
    ///</summary>
    public static class DatabaseNameHelper
    {
        private static readonly Regex ValidDatabaseNameChars = new Regex(@"([A-Za-z0-9_\-\.]+)", RegexOptions.Compiled);

        internal static void AssertValidDatabaseName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var result = ValidDatabaseNameChars.Matches(name);
            if (result.Count == 0 || result[0].Value != name)
                throw new InvalidOperationException($"Database name can only contain only A-Z, a-z, \"_\", \".\" or \"-\" but was: {name}");
        }

        public static string GetDatabaseName(string url)
        {
            if (url == null)
                return null;

            var indexOfDatabases = url.IndexOf("/databases/", StringComparison.OrdinalIgnoreCase);
            if (indexOfDatabases == -1)
                return null;

            url = url.Substring(indexOfDatabases + "/databases/".Length);
            return ValidDatabaseNameChars.Match(url).Value;
        }
    }
}
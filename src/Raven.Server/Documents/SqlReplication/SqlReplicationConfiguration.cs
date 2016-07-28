using System.Collections.Generic;

namespace Raven.Server.Documents.SqlReplication
{
    public class SqlReplicationConfiguration
    {
        public string Id;

        public string Name;

        public bool Disabled;

        public bool ParameterizeDeletesDisabled;

        public bool ForceSqlServerQueryRecompile;

        public bool QuoteTables;

        public string Collection;

        public string Script;

        public string ConnectionStringName;

        public readonly List<SqlReplicationTable> SqlReplicationTables = new List<SqlReplicationTable>();
    }

    public class SqlReplicationTable
    {
        public string TableName;
        public string DocumentKeyColumn;
        public bool InsertOnlyMode;

        protected bool Equals(SqlReplicationTable other)
        {
            return string.Equals(TableName, other.TableName) && 
                string.Equals(DocumentKeyColumn, other.DocumentKeyColumn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SqlReplicationTable)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TableName?.GetHashCode() ?? 0)*397) ^
                       (DocumentKeyColumn?.GetHashCode() ?? 0);
            }
        }
    }
}
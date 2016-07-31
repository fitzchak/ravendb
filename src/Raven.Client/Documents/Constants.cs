﻿namespace Raven.Client.Documents
{
    public class Constants
    {
        public static class Database
        {
            public const string Prefix = "db/";

            public const string DataDirectory = "Raven/Databases/DataDir";

            public const string UrlPrefix = "databases";
        }

        public class Metadata
        {
            public const string MetadataId = "@metadata";

            public const string MetadataDocId = "@id";

            public const string MetadataEtagId = "@etag";
        }

        public class Document
        {
            public const string Id = "Id";
            public const string Etag = "ETag";
        }

        public class Versioning
        {
            public const string RavenVersioningConfiguration = "Raven/Versioning/Configuration";

            public const string RavenEnableVersioning = "Raven-Enable-Versioning";

            public const string RavenDisableVersioning = "Raven-Disable-Versioning";
        }

        public class Expiration
        {
            public const string ConfigurationDocumentKey = "Raven/Expiration/Configuration";

            public const string RavenExpirationDate = "Raven-Expiration-Date";
        }

        public class SqlReplication
        {
            public const string Connections = "Raven/SqlReplication/Connections";
            public const string ConfigurationPrefix = "Raven/SqlReplication/Configuration/";
            public const string StatusPrefix = "Raven/SqlReplication/Status/";
        }

        public class Replication
        {
            public class PropertyNames
            {
                public const string LastSentEtag = "LastSentEtag";
            }

            public const string DocumentChangeVector = "Raven-Document-Change-Vector";

            public const string DocumentReplicationTenantChangeVector = "Raven/DocumentReplication/TenantData";

            public const string DocumentReplicationConfiguration = "Raven/DocumentReplication/Configuration";

            //among others stuff, this will return node's change vector
            public const string DocumentReplicationStatus = "Raven/DocumentReplication/Status";

            public const string RavenReplicationDestinations = "Raven/Replication/Destinations";
        }

        public class PeriodicExport
        {
            public const string ConfigurationDocumentKey = "Raven/PeriodicExport/Configuration";

            public const string StatusDocumentKey = "Raven/PeriodicExport/Status";

            public const string AwsAccessKey = "Raven/AWSAccessKey";

            public const string AwsSecretKey = "Raven/AWSSecretKey";

            public const string AzureStorageAccount = "Raven/AzureStorageAccount";

            public const string AzureStorageKey = "Raven/AzureStorageKey";

            public const string IncrementalExportExtension = ".ravendb-incremental-export";
        }

        public static class Smuggler
        {
            public const string CallContext = "Raven/Smuggler/CallContext";
        }

        public static class Cluster
        {
            public const string ClusterConfigurationDocumentKey = "Raven/Cluster/Configuration";

            public const string ClusterAwareHeader = "Raven-Cluster-Aware";

            public const string ClusterReadBehaviorHeader = "Raven-Cluster-Read-Behavior";

            public const string ClusterFailoverBehaviorHeader = "Raven-Cluster-Failover-Behavior";
        }

        public static class Indexing
        {
            public const string DocumentIdFieldName = "__document_id";

            public const string ReduceKeyFieldName = "__reduce_key";
        }

// TODO: Delete
        public const string DocumentIdFieldName = "__document_id";
        public const string ReduceKeyFieldName = "__reduce_key";


        public static class Encryption
        {
            public const string DontEncryptDocumentsStartingWith = "Raven/";

            public const string InResourceKeyVerificationDocumentName = "Raven/Encryption/Verification";

            public const int DefaultGeneratedEncryptionKeyLength = 256 / 8;

            public const int MinimumAcceptableEncryptionKeyLength = 64 / 8;

            public const int Rfc2898Iterations = 1000;

            public const int DefaultIndexFileBlockSize = 12 * 1024;

            public const int ChangeHistoryLength = 50;
        }
































        public class Headers
        {
            private Headers()
            {
            }

            public const string RavenClientPrimaryServerUrl = "Raven-Client-Primary-Server-Url";

            public const string RavenClientPrimaryServerLastCheck = "Raven-Client-Primary-Server-LastCheck";

            public const string RavenForcePrimaryServerCheck = "Raven-Force-Primary-Server-Check";

            public const string RavenShardId = "Raven-Shard-Id";

            public const string LastModified = "Last-Modified";

            public const string CreationDate = "Creation-Date";

            public const string RavenCreationDate = "Raven-Creation-Date";

            public const string RavenLastModified = "Raven-Last-Modified";

            public const string TemporaryScoreValue = "Temp-Index-Score";

            public const string RavenClrType = "Raven-Clr-Type";

            public const string RavenEntityName = "Raven-Entity-Name";

            public const string RavenReadOnly = "Raven-Read-Only";

            public const string RavenDocumentDoesNotExists = "Raven-Document-Does-Not-Exists";

            public const string NotForReplication = "Raven-Not-For-Replication";

            public const string RavenDeleteMarker = "Raven-Delete-Marker";

            public const string RavenIndexDeleteMarker = "Raven-Index-Delete-Marker";

            public const string RavenTransformerDeleteMarker = "Raven-Transformer-Delete-Marker";

            public const string AllowBundlesChange = "Raven-Temp-Allow-Bundles-Change";

            public const string RavenServerBuild = "Raven-Server-Build";

            public const string RavenReplicationSource = "Raven-Replication-Source";

            public const string RavenReplicationVersion = "Raven-Replication-Version";

            public const string RavenReplicationHistory = "Raven-Replication-History";

            public const string RavenReplicationConflict = "Raven-Replication-Conflict";

            public const string RavenReplicationConflictSkipResolution = "Raven-Replication-Conflict-Skip-Resolution";

            public const string RavenReplicationConflictDocument = "Raven-Replication-Conflict-Document";

            public const string RavenReplicationConflictDocumentForcePut = "Raven-Replication-Conflict-Document-Force-Put";

            public const string RavenCreateVersion = "Raven-Create-Version";

            public const string RavenIgnoreVersioning = "Raven-Ignore-Versioning";

            public const string RavenClientVersion = "Raven-Client-Version";

            public const string NextPageStart = "Next-Page-Start";

            public const string RequestTime = "Raven-Request-Time";
        }

        public const string LastEtagFieldName = "Raven-LastEtag";

        public const string ParticipatingIDsPropertyName = "Participating-IDs-Property-Name";

        public const string IsReplicatedUrlParamName = "is-replicated";

        public const string TenantIdKey = "Database-Tenant-Id";

        public const string AlphaNumericFieldName = "__alphaNumeric";

        public const string RandomFieldName = "__random";

        public const string CustomSortFieldName = "__customSort";

        public const string NullValueNotAnalyzed = "[[NULL_VALUE]]";

        public const string EmptyStringNotAnalyzed = "[[EMPTY_STRING]]";

        public const string NullValue = "NULL_VALUE";

        public const string EmptyString = "EMPTY_STRING";

        public const string ReduceValueFieldName = "__reduced_val";

        public const string IntersectSeparator = " INTERSECT ";

        public const string AllFields = "__all_fields";

        public const string TemporaryTransformerPrefix = "Temp/";

        public const string RavenAlerts = "Raven/Alerts";

        public const string RavenJavascriptFunctions = "Raven/Javascript/Functions";

        public const string BulkImportHeartbeatDocKey = "Raven/BulkImport/Heartbeat";

        public const string IndexReplacePrefix = "Raven/Indexes/Replace/";
        public const string SideBySideIndexNamePrefix = "ReplacementOf/";

        public const string ApiKeyPrefix = "apikeys/";

        //Files
        public const int WindowsMaxPath = 260 - 30;

        public const int LinuxMaxPath = 4096;

        public const int LinuxMaxFileNameLength = WindowsMaxPath;

        public static readonly string[] WindowsReservedFileNames = { "con", "prn", "aux", "nul", "com1", "com2", "com3", "com4", "com5", "com6", "com7", "com8", "com9", "lpt1", "lpt2", "lpt3", "lpt4", "lpt5", "lpt6", "lpt7", "lpt8", "lpt9", "clock$" };
       

        //Spatial
        public const string DefaultSpatialFieldName = "__spatial";

        public const string SpatialShapeFieldName = "__spatialShape";

        public const double DefaultSpatialDistanceErrorPct = 0.025d;

        public const string DistanceFieldName = "__distance";

        /// <summary>
        /// The International Union of Geodesy and Geophysics says the Earth's mean radius in KM is:
        ///
        /// [1] http://en.wikipedia.org/wiki/Earth_radius
        /// </summary>
        public const double EarthMeanRadiusKm = 6371.0087714;

        public const double MilesToKm = 1.60934;

        public const string RavenDefaultQueryTimeout = "Raven_Default_Query_Timeout";



        /// <summary>
        /// if no encoding information in headers of incoming request, this encoding is assumed
        /// </summary>
        public const string DefaultRequestEncoding = "UTF-8";


        public const string TempUploadsDirectoryName = "RavenTempUploads";

        public const string DataCouldNotBeDecrypted = "<data could not be decrypted>";

        public const int NumberOfCachedRequests = 1024;

        // Backup

        public const string DatabaseDocumentFilename = "Database.Document";

        public const string FilesystemDocumentFilename = "Filesystem.Document";

        public const string IncrementalBackupAlertTimeout = "Raven/IncrementalBackup/AlertTimeoutHours";

        public const string IncrementalBackupRecurringAlertTimeout = "Raven/IncrementalBackup/RecurringAlertTimeoutDays";

        public const string IncrementalBackupState = "IncrementalBackupState.Document";

        // General

        public const string ResourceMarkerPrefix = ".resource.";

        //File System
        public static class FileSystem
        {
            public const string Prefix = "fs/";

            public const string UrlPrefix = "fs";

            public const string RavenFsSize = "RavenFS-Size";
            public const string FsResourceMarker = ResourceMarkerPrefix + "file-system";
        }

        //Counters
        public static class Counter
        {
            public const string Prefix = "cs/";

            public const string UrlPrefix = "cs";
        }

        //Time Series
        public static class TimeSeries
        {
            public const string Prefix = "ts/";

            public const string UrlPrefix = "ts";
        }

        // Subscriptions
        public const string RavenSubscriptionsPrefix = "Raven/Subscriptions/";


        

        public class Authorization
        {
            public const string RavenAuthorizationUser = "Raven-Authorization-User";

            public const string RavenAuthorizationOperation = "Raven-Authorization-Operation";

            public const string RavenDocumentAuthorization = "Raven-Document-Authorization";
        }

        public class Monitoring
        {
            public class Snmp
            {
                public const string DatabaseMappingDocumentKey = "Raven/Monitoring/Snmp/Databases";

                public const string DatabaseMappingDocumentPrefix = "Raven/Monitoring/Snmp/Databases/";
            }
        }

        public const string RequestFailedExceptionMarker = "ExceptionRequestFailed";


    }
}
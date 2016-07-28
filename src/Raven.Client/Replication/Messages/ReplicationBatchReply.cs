namespace Raven.Client.Replication.Messages
{
    public class ReplicationBatchReply
    {
        public enum ReplyType
        {
            None,
            Ok,
            Error
        }

        public ReplyType Type { get; set; }

        public long LastEtagAccepted { get; set; }

        public string Error { get; set; }
    }
}

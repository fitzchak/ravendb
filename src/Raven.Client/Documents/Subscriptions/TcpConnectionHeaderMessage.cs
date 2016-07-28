namespace Raven.Client.Documents.Subscriptions
{
    public class TcpConnectionHeaderMessage
    {
        public enum OperationTypes
        {
            None,
            BulkInsert,
            Subscription,
            Replication
        }

        public string DatabaseName { get; set; }

        public OperationTypes Operation { get; set; }
    }
}
namespace Raven.Client.Documents.Data
{
    /// <summary>
    /// The result of a PUT operation
    /// </summary>
    public class PutResult
    {
        /// <summary>
        /// ID of the document that was PUT.
        /// </summary>
        public string Id;

        /// <summary>
        /// long? of the document after PUT operation.
        /// </summary>
        public long? ETag;
    }
}
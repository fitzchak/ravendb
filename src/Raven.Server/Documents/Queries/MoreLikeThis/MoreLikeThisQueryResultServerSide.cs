using Raven.Client.Documents.Queries;

namespace Raven.Server.Documents.Queries.MoreLikeThis
{
    public class MoreLikeThisQueryResultServerSide : MoreLikeThisQueryResult<Document>
    {
        public bool NotModified { get; set; }
    }
}
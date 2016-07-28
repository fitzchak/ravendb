namespace Raven.Client.Documents.Indexing
{
    public enum IndexLockMode
    {
        Unlock,
        LockedIgnore,
        LockedError,
        SideBySide
    }
}
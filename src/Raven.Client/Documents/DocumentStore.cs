using System;
using Sparrow.Json;

namespace Raven.Client.Documents
{
    public class DocumentStore : IDisposable
    {
        private string _url;

        /// <summary>
        /// Gets or sets the default database name.
        /// </summary>
        public string DefaultDatabase;

        public UnmanagedBuffersPool UnmanagedBuffersPool { get; private set; }

        public readonly DocumentsConvention Conventions = new DocumentsConvention();

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public virtual string Url
        {
            get { return _url; }
            set { _url = value.EndsWith("/") ? value.Substring(0, value.Length - 1) : value; }
        }

        /// <summary>
        /// The API Key to use when authenticating against a RavenDB server that enabled API Key authentication
        /// </summary>
        public string ApiKey;

        private bool _wasDisposed;

        public DocumentStore Initialize()
        {
            UnmanagedBuffersPool = new UnmanagedBuffersPool($"DocumentStore: {DefaultDatabase}, {Url}");

            return this;
        }

        public DocumentSession OpenSession(string database = null, bool forceReadFromMaster = false)
        {
            EnsureNotDisposed();

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _wasDisposed = true;

#if DEBUG
            GC.SuppressFinalize(this);
#endif

            UnmanagedBuffersPool?.Dispose();
        }

        protected void EnsureNotDisposed()
        {
            if (_wasDisposed)
                throw new ObjectDisposedException(GetType().Name, "The document store has already been disposed and cannot be used");
        }
    }
}
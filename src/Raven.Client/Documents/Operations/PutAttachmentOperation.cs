﻿using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Raven.Client.Documents.Conventions;
using Raven.Client.Http;
using Raven.Client.Util;
using Sparrow.Json;

namespace Raven.Client.Documents.Operations
{
    public class PutAttachmentOperation : IOperation<long>
    {
        private readonly string _documentId;
        private readonly string _name;
        private readonly Stream _stream;
        private readonly string _contentType;
        private readonly long? _etag;

        public PutAttachmentOperation(string documentId, string name, Stream stream, string contentType, long? etag = null)
        {
            _documentId = documentId;
            _name = name;
            _stream = stream;
            _contentType = contentType;
            _etag = etag;
        }

        public RavenCommand<long> GetCommand(DocumentConventions conventions, JsonOperationContext context, HttpCache cache)
        {
            return new PutAttachmentCommand(_documentId, _name, _stream, _contentType, _etag);
        }

        private class PutAttachmentCommand : RavenCommand<long>
        {
            private readonly string _documentId;
            private readonly string _name;
            private readonly Stream _stream;
            private readonly string _contentType;
            private readonly long? _etag;

            public PutAttachmentCommand(string documentId, string name, Stream stream, string contentType, long? etag)
            {
                if (string.IsNullOrWhiteSpace(documentId))
                    throw new ArgumentNullException(nameof(documentId));
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentNullException(nameof(name));

                _documentId = documentId;
                _name = name;
                _stream = stream;
                _contentType = contentType;
                _etag = etag;
            }

            public override HttpRequestMessage CreateRequest(ServerNode node, out string url)
            {
                url = $"{node.Url}/databases/{node.Database}/attachments?id={Uri.EscapeUriString(_documentId)}&name={Uri.EscapeUriString(_name)}";
                if (string.IsNullOrWhiteSpace(_contentType) == false)
                    url += $"&contentType={Uri.EscapeUriString(_contentType)}";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethods.Put,
                    Content = new StreamContent(_stream)
                };

                AddEtagIfNotNull(_etag, request);

                return request;
            }

            public override void SetResponse(BlittableJsonReaderObject response, bool fromCache)
            {
            }

            public override bool IsReadRequest => false;
        }
    }
}
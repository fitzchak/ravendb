﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Raven.Client;
using Raven.Client.Documents.Replication.Messages;
using Sparrow;
using Sparrow.Json;
using Sparrow.Json.Parsing;

namespace Raven.Server.Documents
{
    public class Document
    {
        public static readonly Document ExplicitNull = new Document();

        private ulong? _hash;
        private bool _metadataEnsured;

        public long Etag;
        public LazyStringValue Key;
        public LazyStringValue LoweredKey;
        public long StorageId;
        public BlittableJsonReaderObject Data;
        public float? IndexScore;
        public ChangeVectorEntry[] ChangeVector;
        public DateTime LastModified;
        public DocumentFlags Flags;
        public short TransactionMarker;
        public IEnumerable<Attachment> Attachments;

        public unsafe ulong DataHash
        {
            get
            {
                if (_hash.HasValue == false)
                    _hash = Hashing.XXHash64.Calculate(Data.BasePointer, (ulong)Data.Size);

                return _hash.Value;
            }
        }

        public void EnsureMetadata(float? indexScore = null)
        {
            if (_metadataEnsured)
                return;

            _metadataEnsured = true;

            DynamicJsonValue mutatedMetadata;
            BlittableJsonReaderObject metadata;
            if (Data.TryGet(Constants.Documents.Metadata.Key, out metadata))
            {
                if (metadata.Modifications == null)
                    metadata.Modifications = new DynamicJsonValue(metadata);

                mutatedMetadata = metadata.Modifications;
            }
            else
            {
                Data.Modifications = new DynamicJsonValue(Data)
                {
                    [Constants.Documents.Metadata.Key] = mutatedMetadata = new DynamicJsonValue()
                };
            }

            mutatedMetadata[Constants.Documents.Metadata.Etag] = Etag;
            mutatedMetadata[Constants.Documents.Metadata.Id] = Key;
            //mutatedMetadata[Constants.Documents.Metadata.ChangeVector] = ChangeVector;
            if (indexScore.HasValue)
                mutatedMetadata[Constants.Documents.Metadata.IndexScore] = indexScore;

            _hash = null;
        }

        public void RemoveAllPropertiesExceptMetadata()
        {
            foreach (var property in Data.GetPropertyNames())
            {
                if (string.Equals(property, Constants.Documents.Metadata.Key, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (Data.Modifications == null)
                    Data.Modifications = new DynamicJsonValue(Data);

                Data.Modifications.Remove(property);
            }

            _hash = null;
        }

        public bool Expired(DateTime currentDate)
        {
            string expirationDate;
            BlittableJsonReaderObject metadata;
            if (Data.TryGet(Constants.Documents.Metadata.Key, out metadata) &&
                metadata.TryGet(Constants.Documents.Expiration.ExpirationDate, out expirationDate))
            {
                var expirationDateTime = DateTime.ParseExact(expirationDate, new[] { "o", "r" }, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                if (expirationDateTime < currentDate)
                    return true;
            }
            return false;
        }

        public bool IsMetadataEqualTo(BlittableJsonReaderObject obj)
        {
            if (obj == null)
                return false;

            BlittableJsonReaderObject myMetadata;
            BlittableJsonReaderObject objMetadata;
            Data.TryGet(Constants.Documents.Metadata.Key, out myMetadata);
            obj.TryGet(Constants.Documents.Metadata.Key, out objMetadata);

            if (myMetadata == null && objMetadata == null)
                return true;

            if (myMetadata == null || objMetadata == null)
                return false;

            return ComparePropertiesExceptionStartingWithAt(myMetadata, objMetadata, isMetadata: true);
        }

        public bool IsEqualTo(BlittableJsonReaderObject obj)
        {
            return ComparePropertiesExceptionStartingWithAt(Data, obj);
        }

        private static bool ComparePropertiesExceptionStartingWithAt(BlittableJsonReaderObject myMetadata,
            BlittableJsonReaderObject objMetadata, bool isMetadata = false)
        {
            var properties = new HashSet<string>(myMetadata.GetPropertyNames());
            foreach (var propertyName in objMetadata.GetPropertyNames())
            {
                properties.Add(propertyName);
            }

            foreach (var property in properties)
            {
                if (isMetadata && property[0] == '@' && 
                    property.Equals(Constants.Documents.Metadata.Collection, StringComparison.OrdinalIgnoreCase) == false)
                    continue;

                object myProperty;
                object objProperty;

                if (myMetadata.TryGetMember(property, out myProperty) == false)
                    return false;

                if (objMetadata.TryGetMember(property, out objProperty) == false)
                    return false;

                if (Equals(myProperty, objProperty) == false)
                    return false;
            }
            return true;
        }
    }
}
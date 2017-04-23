using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Raven.Client.Documents.Replication.Messages;
using Raven.Server.ServerWide.Context;
using Sparrow;
using Sparrow.Binary;
using Voron;
using Voron.Data.BTrees;

namespace Raven.Server.Utils
{
    public class ChangeVectorUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ChangeVectorToString(Dictionary<Guid, long> changeVector)
        {
            var sb = new StringBuilder();
            foreach (var kvp in changeVector)
                sb.Append($"{kvp.Key}:{kvp.Value};");

            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ChangeVectorToString(ChangeVectorEntry[] changeVector)
        {
            var sb = new StringBuilder();
            foreach (var kvp in changeVector)
                sb.Append($"{kvp.DbId}:{kvp.Etag};");

            return sb.ToString();
        }

        public static unsafe void WriteChangeVectorTo(DocumentsOperationContext context, Dictionary<Guid, long> changeVector, Tree tree)
        {
            Guid dbId;
            long etagBigEndian;
            Slice keySlice;
            Slice valSlice;
            using (Slice.External(context.Allocator, (byte*)&dbId, sizeof(Guid), out keySlice))
            using (Slice.External(context.Allocator, (byte*)&etagBigEndian, sizeof(long), out valSlice))
            {
                foreach (var kvp in changeVector)
                {
                    dbId = kvp.Key;
                    etagBigEndian = Bits.SwapBytes(kvp.Value);
                    tree.Add(keySlice, valSlice);
                }
            }
        }

        public static unsafe void WriteChangeVectorTo(ByteStringContext context, Dictionary<Guid, long> changeVector, Tree tree)
        {
            Guid dbId;
            long etagBigEndian;
            using (Slice.External(context, (byte*)&dbId, sizeof(Guid), out Slice keySlice))
            using (Slice.External(context, (byte*)&etagBigEndian, sizeof(long), out Slice valSlice))
            {
                foreach (var kvp in changeVector)
                {
                    dbId = kvp.Key;
                    etagBigEndian = Bits.SwapBytes(kvp.Value);
                    tree.Add(keySlice, valSlice);
                }
            }
        }

        public static unsafe ChangeVectorEntry[] ReadChangeVectorFrom(Tree tree)
        {
            var changeVector = new ChangeVectorEntry[tree.State.NumberOfEntries];
            using (var iter = tree.Iterate(false))
            {
                if (iter.Seek(Slices.BeforeAllKeys) == false)
                    return changeVector;
                var buffer = new byte[sizeof(Guid)];
                int index = 0;
                do
                {
                    var read = iter.CurrentKey.CreateReader().Read(buffer, 0, sizeof(Guid));
                    if (read != sizeof(Guid))
                        throw new InvalidDataException($"Expected guid, but got {read} bytes back for change vector");

                    changeVector[index].DbId = new Guid(buffer);
                    changeVector[index].Etag = iter.CreateReaderForCurrent().ReadBigEndianInt64();
                    index++;
                } while (iter.MoveNext());
            }
            return changeVector;
        }

        public static void UpdateChangeVectorWithNewEtag(DocumentsOperationContext context, Guid dbId, long newEtag, ref ArraySegment<ChangeVectorEntry> changeVector)
        {
            var length = changeVector.Count;
            for (int i = 0; i < length; i++)
            {
                if (changeVector.Array[i].DbId == dbId)
                {
                    changeVector.Array[i].Etag = newEtag;
                    return;
                }
            }

            Array.Resize(ref changeVector.Array, length + 1);
            changeVector[length].DbId = dbId;
            changeVector[length].Etag = newEtag;
        }

        public static ArraySegment<ChangeVectorEntry> MergeVectors(DocumentsOperationContext context, ArraySegment<ChangeVectorEntry> vectorA, ArraySegment<ChangeVectorEntry> vectorB)
        {
            Array.Sort(vectorA.Array, 0, vectorA.Count);
            Array.Sort(vectorB.Array, 0, vectorB.Count);
            int ia = 0, ib = 0;
            var merged = new List<ChangeVectorEntry>();
            while (ia < vectorA.Count && ib < vectorB.Count)
            {
                int res = vectorA.Array[ia].CompareTo(vectorB.Array[ib]);
                if (res == 0)
                {
                    merged.Add(new ChangeVectorEntry
                    {
                        DbId = vectorA.Array[ia].DbId,
                        Etag = Math.Max(vectorA.Array[ia].Etag, vectorB.Array[ib].Etag)
                    });
                    ia++;
                    ib++;
                }
                else if (res < 0)
                {
                    merged.Add(vectorA.Array[ia]);
                    ia++;
                }
                else
                {
                    merged.Add(vectorB.Array[ib]);
                    ib++;
                }
            }
            for (; ia < vectorA.Count; ia++)
            {
                merged.Add(vectorA.Array[ia]);
            }
            for (; ib < vectorB.Count; ib++)
            {
                merged.Add(vectorB.Array[ib]);
            }

            var result = context.GetNextChangeVectorBuffer(merged.Count);
            var a; a = merged.ToArray();
            return result;
        }

        public static ChangeVectorEntry[] MergeVectors(IReadOnlyList<ChangeVectorEntry[]> changeVectors)
        {
            var mergedVector = new Dictionary<Guid, long>();

            foreach (var changeVector in changeVectors)
            {
                foreach (var changeVectorEntry in changeVector)
                {
                    if (!mergedVector.ContainsKey(changeVectorEntry.DbId))
                    {
                        mergedVector[changeVectorEntry.DbId] = changeVectorEntry.Etag;
                    }
                    else
                    {
                        mergedVector[changeVectorEntry.DbId] = Math.Max(mergedVector[changeVectorEntry.DbId],
                            changeVectorEntry.Etag);
                    }
                }
            }

            return mergedVector.Select(kvp => new ChangeVectorEntry
            {
                DbId = kvp.Key,
                Etag = kvp.Value
            }).ToArray();
        }
    }
}
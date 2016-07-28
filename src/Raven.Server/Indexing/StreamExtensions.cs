//-----------------------------------------------------------------------
// <copyright file="StreamExtension.cs" company="Hibernating Rhinos LTD">
//     Copyright (c) Hibernating Rhinos LTD. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;

namespace Raven.Abstractions.Extensions
{
    /// <summary>
    /// Extensions for working with streams
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Allocates a byte array and reads an entire block from the stream
        /// </summary>
        public static byte[] ReadEntireBlock(this Stream stream, int count)
        {
            var buffer = new byte[count];
            stream.ReadEntireBlock(buffer, 0, count);

            return buffer;
        }

        /// <summary>
        /// Reads an entire block from the stream
        /// </summary>
        public static void ReadEntireBlock(this Stream stream, byte[] buffer, int start, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = stream.Read(buffer, start + totalRead, count - totalRead);
                if (read == 0)
                    throw new EndOfStreamException();
                totalRead += read;
            }
        }
    }
}

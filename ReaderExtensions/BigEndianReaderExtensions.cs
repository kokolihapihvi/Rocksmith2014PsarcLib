using System;
using System.IO;

namespace Rocksmith2014PsarcLib.ReaderExtensions
{
    public static class BigEndianReaderExtensions
    {
        /// <summary>
        /// Read bytes in big endian (reverse byte order)
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="span"></param>
        private static void ReadBytesBe(this BinaryReader reader, Span<byte> span)
        {
            reader.Read(span);
            span.Reverse();
        }

        /// <summary>
        /// Read a 2 byte unsigned int in big endian (reverse byte order)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ushort ReadUInt16Be(this BinaryReader reader)
        {
            Span<byte> span = stackalloc byte[2];
            reader.ReadBytesBe(span);
            
            return BitConverter.ToUInt16(span);
        }

        /// <summary>
        /// Read a 3 byte unsigned int in big endian (reverse byte order)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static uint ReadUInt24Be(this BinaryReader reader)
        {
            //3 bytes
            var bytes = new byte[4];
            
            Span<byte> span = stackalloc byte[3];
            reader.ReadBytesBe(span);

            span.CopyTo(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Read a 4 byte unsigned int in big endian (reverse byte order)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static uint ReadUInt32Be(this BinaryReader reader)
        {
            Span<byte> span = stackalloc byte[4];
            reader.ReadBytesBe(span);
            
            return BitConverter.ToUInt32(span);
        }

        /// <summary>
        /// Read a 5 byte unsigned int in big endian (reverse byte order)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ulong ReadUInt40Be(this BinaryReader reader)
        {
            //5 bytes
            var bytes = new byte[8];
            
            Span<byte> span = stackalloc byte[5];
            reader.ReadBytesBe(span);

            span.CopyTo(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}

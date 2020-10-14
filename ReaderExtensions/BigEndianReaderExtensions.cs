using System;
using System.IO;

namespace Rocksmith2014PsarcLib.ReaderExtensions
{
    public static class BigEndianReaderExtensions
    {
        public static byte[] ReadBytesBE(this BinaryReader _reader, int count)
        {
            byte[] bytes = _reader.ReadBytes(count);
            Array.Reverse(bytes);
            return bytes;
        }

        public static uint ReadUInt16BE(this BinaryReader _reader)
        {
            return BitConverter.ToUInt16(_reader.ReadBytesBE(2), 0);
        }

        public static uint ReadUInt24BE(this BinaryReader _reader)
        {
            //3 bytes
            byte[] bytes = new byte[4];

            Array.Copy(_reader.ReadBytesBE(3), bytes, 3);

            return BitConverter.ToUInt32(bytes, 0);
        }

        public static uint ReadUInt32BE(this BinaryReader _reader)
        {
            return BitConverter.ToUInt32(_reader.ReadBytesBE(4), 0);
        }

        public static ulong ReadUInt40BE(this BinaryReader _reader)
        {
            //5 bytes
            byte[] bytes = new byte[8];

            Array.Copy(_reader.ReadBytesBE(5), bytes, 5);

            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}

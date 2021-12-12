using System.Buffers;
using Rocksmith2014PsarcLib.Psarc.Models;
using System.IO;
using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.ReaderExtensions
{
    public static class StructReaderExtensions
    {
        /// <summary>
        /// Reads count structs from the stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] ReadStructArray<T>(this BinaryReader reader, int count) where T : struct
        {
            var arr = new T[count];

            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = reader.ReadStruct<T>();
            }

            return arr;
        }

        /// <summary>
        /// Reads first 4 bytes as n, then reads n structs from the stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T[] ReadStructArray<T>(this BinaryReader reader) where T : struct
        {
            var count = reader.ReadInt32();

            return ReadStructArray<T>(reader, count);
        }

        /// <summary>
        /// Reads a struct from the stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ReadStruct<T>(this BinaryReader reader) where T : struct
        {
            T _struct;

            //Some structs need more advanced logic, for example dynamic length arrays
            if (typeof(IBinarySerializable).IsAssignableFrom(typeof(T)))
            {
                _struct = new T();
                return (T)(_struct as IBinarySerializable).Read(reader);
            }

            var size = Marshal.SizeOf(typeof(T));

            var readBuffer = ArrayPool<byte>.Shared.Rent(size);
            reader.Read(readBuffer, 0, size);

            var handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);

            _struct = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());

            handle.Free();
            
            ArrayPool<byte>.Shared.Return(readBuffer);

            return _struct;
        }
    }
}

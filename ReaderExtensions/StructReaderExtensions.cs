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
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static T[] ReadStructArray<T>(this BinaryReader _reader, int count) where T : struct
        {
            T[] arr = new T[count];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = _reader.ReadStruct<T>();
            }

            return arr;
        }

        /// <summary>
        /// Reads first 4 bytes as n, then reads n structs from the stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static T[] ReadStructArray<T>(this BinaryReader _reader) where T : struct
        {
            int count = _reader.ReadInt32();

            return ReadStructArray<T>(_reader, count);
        }

        /// <summary>
        /// Reads a struct from the stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static T ReadStruct<T>(this BinaryReader _reader) where T : struct
        {
            T _struct;

            //Some structs need more advanced logic, for example dynamic length arrays
            if (typeof(IBinarySerializable).IsAssignableFrom(typeof(T)))
            {
                _struct = new T();
                return (T)(_struct as IBinarySerializable).Read(_reader);
            }

            int size = Marshal.SizeOf(typeof(T));
            byte[] readBuffer = _reader.ReadBytes(size);

            GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);

            _struct = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());

            handle.Free();

            return _struct;
        }
    }
}

using System.IO;

namespace Rocksmith2014PsarcLib.ReaderExtensions
{
    public static class ArrayReaderExtensions
    {
        /// <summary>
        /// Read an array of count floats
        /// </summary>
        /// <param name="_reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static float[] ReadFloatArray(this BinaryReader _reader, int count)
        {
            float[] arr = new float[count];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = _reader.ReadSingle();
            }

            return arr;
        }

        /// <summary>
        /// Read an array of count ints
        /// </summary>
        /// <param name="_reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] ReadIntArray(this BinaryReader _reader, int count)
        {
            int[] arr = new int[count];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = _reader.ReadInt32();
            }

            return arr;
        }

        /// <summary>
        /// Read an array of count shorts
        /// </summary>
        /// <param name="_reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static short[] ReadShortArray(this BinaryReader _reader, int count)
        {
            short[] arr = new short[count];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = _reader.ReadInt16();
            }

            return arr;
        }
    }
}

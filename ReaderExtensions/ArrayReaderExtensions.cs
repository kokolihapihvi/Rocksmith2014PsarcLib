using System.IO;

namespace Rocksmith2014PsarcLib.ReaderExtensions
{
    public static class ArrayReaderExtensions
    {
        /// <summary>
        /// Read an array of count floats
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static float[] ReadFloatArray(this BinaryReader reader, int count)
        {
            var arr = new float[count];

            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = reader.ReadSingle();
            }

            return arr;
        }

        /// <summary>
        /// Read an array of count ints
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] ReadIntArray(this BinaryReader reader, int count)
        {
            var arr = new int[count];

            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = reader.ReadInt32();
            }

            return arr;
        }

        /// <summary>
        /// Read an array of count shorts
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static short[] ReadShortArray(this BinaryReader reader, int count)
        {
            var arr = new short[count];

            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = reader.ReadInt16();
            }

            return arr;
        }
    }
}

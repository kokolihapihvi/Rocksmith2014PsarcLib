using System.IO;
using System.Text;

namespace Rocksmith2014PsarcLib.ReaderExtensions
{
    public static class GeneralReaderExtensions
    {
        /// <summary>
        /// Reads a zero terminated string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="size"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadString(this BinaryReader reader, int size, Encoding encoding = null)
        {
            var bytes = reader.ReadBytes(size);

            encoding ??= Encoding.UTF8;

            return encoding.GetString(bytes).TrimEnd((char)0);
        }
    }
}

using Org.BouncyCastle.Utilities.Zlib;
using Rocksmith2014PsarcLib.Psarc.Asset;
using Rocksmith2014PsarcLib.Psarc.Models;
using Rocksmith2014PsarcLib.Psarc.Models.Json;
using Rocksmith2014PsarcLib.ReaderExtensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Rocksmith2014PsarcLib.Psarc
{
    public class PsarcFile : IDisposable
    {
        public string FilePath { get; }

        public PsarcFileHeader Header { get; }

        public PsarcTOC TOC { get; }

        internal FileStream FileStream { get; }

        internal BinaryReader Reader { get; }

        public PsarcFile(string filePath)
        {
            FilePath = filePath;

            // Setup file stream
            FileStream = File.OpenRead(filePath);

            // Setup reader
            Reader = new BinaryReader(FileStream);

            // Read header
            Header = new PsarcFileHeader(this);

            // Load/decrypt table of contents
            TOC = new PsarcTOC(this);

            // Read manifest
            ReadManifest();
        }

        public PsarcFile(FileInfo fileInfo) : this(fileInfo.FullName)
        {
        }

        /// <summary>
        /// Reads the filepath of all assets inside the psarc
        /// </summary>
        private void ReadManifest()
        {
            TOC.Entries[0].Path = "NamesBlock.bin";

            var asset = InflateEntry<TextPsarcAsset>(TOC.Entries[0]);

            var names = asset.Lines;

            for (var i = 0; i < names.Length; i++)
            {
                TOC.Entries[i + 1].Path = names[i];
            }
        }

        /// <summary>
        /// Uncompresses a ZIP compressed byte array
        /// </summary>
        /// <param name="zipped"></param>
        /// <returns></returns>
        private byte[] UnzipBlock(byte[] zipped)
        {
            using var inStream = new MemoryStream(zipped);
            using var zOutputStream = new ZInputStream(inStream);
            using var outStream = new MemoryStream();

            zOutputStream.CopyTo(outStream);

            return outStream.ToArray();
        }

        private unsafe void UnzipBlock(BinaryReader from, Stream to, int size)
        {
            // Skip the header bytes, because DeflateStream does not recognize them for some reason
            var readsize = size - 2;
            Reader.BaseStream.Seek(2, SeekOrigin.Current);

            Span<byte> span = stackalloc byte[readsize];

            from.Read(span);

            fixed (byte* pBuffer = &span[0])
            {
                using var inStream = new UnmanagedMemoryStream(pBuffer, span.Length);
                using var decompressStream = new DeflateStream(inStream, CompressionMode.Decompress);

                decompressStream.CopyTo(to);
            }
        }

        /// <summary>
        /// Inflates entry into a stream, uncompressing if needed
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="stream"></param>
        public void InflateEntry(PsarcTOCEntry entry, Stream stream)
        {
            const int ZipHeader = 0x78DA;
            var blockSize = (int)Header.BlockSize;

            var lastBlock = (int)(Math.Ceiling(entry.Length / (float)Header.BlockSize) + entry.StartBlock - 1);

            Reader.BaseStream.Seek((long)entry.Offset, SeekOrigin.Begin);

            Span<byte> buffer = stackalloc byte[blockSize];

            for (var block = entry.StartBlock; block <= lastBlock; block++)
            {
                // Size of this zip block (0 for uncompressed)
                var zipblockSize = (int)TOC.ZipBlockSizes[block];

                if (zipblockSize == 0) // raw. full cluster used.
                {
                    Reader.Read(buffer);
                    stream.Write(buffer);
                }
                else
                {
                    var header = Reader.ReadUInt16Be();
                    // Seek 2 bytes backwards to include the header (or first 2 data bytes) still in the stream
                    Reader.BaseStream.Seek(-2, SeekOrigin.Current);

                    if (header == ZipHeader) // compressed
                    {
                        UnzipBlock(Reader, stream, zipblockSize);
                    }
                    else // uncompressed block
                    {
                        stream.Write(Reader.ReadBytes(zipblockSize), 0, zipblockSize);
                    }
                }
            }

            stream.Seek(0, SeekOrigin.Begin);
            stream.Flush();
        }

        /// <summary>
        /// Inflates entry into an asset object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <returns></returns>
        public T InflateEntry<T>(PsarcTOCEntry entry) where T : PsarcAsset, new()
        {
            if (entry == null) return null;

            var asset = new T();

            // Rent a buffer for inflating
            var inflatebuffer = ArrayPool<byte>.Shared.Rent((int)entry.Length);

            using var ms = new MemoryStream(inflatebuffer);
            // Resize the stream, as the buffer may be larger
            ms.SetLength((int)entry.Length);

            try
            {
                InflateEntry(entry, ms);

                asset.ReadFrom(ms);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error inflating entry {entry.Path}");
                Console.WriteLine(e);
                throw;
            }
            finally // Always return rented buffer
            {
                ArrayPool<byte>.Shared.Return(inflatebuffer);
            }

            return asset;
        }

        /// <summary>
        /// Inflates the first entry that matches the pattern <paramref name="where"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public T InflateEntry<T>(Func<PsarcTOCEntry, bool> where) where T : PsarcAsset, new()
        {
            var entry = TOC.Entries.FirstOrDefault(where);

            return InflateEntry<T>(entry);
        }

        public List<T> InflateEntries<T>(Func<PsarcTOCEntry, bool> where) where T : PsarcAsset, new()
        {
            var inflatedEntries = new List<T>();

            var entries = TOC.Entries.Where(where);

            foreach (var entry in entries)
            {
                inflatedEntries.Add(InflateEntry<T>(entry));
            }

            return inflatedEntries;
        }

        #region Convenience Zone

        public DdsAsset ExtractAlbumArt(SongArrangement.ArrangementAttributes attr)
        {
            string artPath = $"gfxassets/album_art/{attr.AlbumArt.Substring(14)}_256.dds";

            var entry = TOC.Entries.FirstOrDefault(a => a.Path == artPath);

            if (entry == null) throw new KeyNotFoundException();

            return InflateEntry<DdsAsset>(entry);
        }

        /// <summary>
        /// Extract toolkitinfo, returns default values if not found
        /// </summary>
        /// <returns></returns>
        public ToolkitInfo ExtractToolkitInfo()
        {
            var tkAsset = InflateEntry<ToolkitInfoAsset>(entry => entry.Path?.Equals("toolkit.version") ?? false);

            if (tkAsset != null) return tkAsset.ToolkitInfo;

            return new ToolkitInfo();
        }

        /// <summary>
        /// Extracts all json files and assumes they are song arrangements, should work for all song psarc files
        /// </summary>
        /// <returns></returns>
        public List<SongArrangement> ExtractArrangementManifests()
        {
            var list = InflateEntries<JsonObjectAsset<SongArrangement>>(asset => asset.Path.EndsWith(".json"));

            return list.Select(l => l.JsonObject).ToList();
        }

        #endregion

        #region Disposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Reader.Dispose();
                    FileStream.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PsarcFile()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

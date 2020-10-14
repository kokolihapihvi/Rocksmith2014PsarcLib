using Org.BouncyCastle.Utilities.Zlib;
using Rocksmith2014PsarcLib.Psarc.Asset;
using Rocksmith2014PsarcLib.Psarc.Models;
using Rocksmith2014PsarcLib.Psarc.Models.Json;
using Rocksmith2014PsarcLib.ReaderExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rocksmith2014PsarcLib.Psarc
{
    public class PsarcFile : IDisposable
    {
        public string FilePath { get; private set; }

        public PsarcFileHeader Header { get; private set; }

        public PsarcTOC TOC { get; private set; }

        internal FileStream _fileStream;

        internal BinaryReader _reader;

        public PsarcFile(string filePath)
        {
            FilePath = filePath;

            // Setup file stream
            _fileStream = File.OpenRead(filePath);

            // Setup reader
            _reader = new BinaryReader(_fileStream);

            // Read header
            Header = new PsarcFileHeader(this);

            // Load/decrypt table of contents
            TOC = new PsarcTOC(this);

            // Read manifest
            ReadManifest();
        }
        public PsarcFile(FileInfo fileInfo) : this(fileInfo.FullName) { }

        /// <summary>
        /// Reads the filepath of all assets inside the psarc
        /// </summary>
        private void ReadManifest()
        {
            TOC.Entries[0].Path = "NamesBlock.bin";

            var asset = InflateEntry<TextPsarcAsset>(TOC.Entries[0]);

            var names = asset.Lines;

            for (int i = 0; i < names.Length; i++)
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
            using (var inStream = new MemoryStream(zipped))
            {
                using (var zOutputStream = new ZInputStream(inStream))
                {
                    using (var outStream = new MemoryStream())
                    {
                        zOutputStream.CopyTo(outStream);

                        return outStream.ToArray();
                    }
                }
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
            int blockSize = (int)Header.BlockSize;

            int lastBlock = (int)(Math.Ceiling(entry.Length / (float)Header.BlockSize) + entry.StartBlock - 1);

            _reader.BaseStream.Seek((long)entry.Offset, SeekOrigin.Begin);

            for (uint block = entry.StartBlock; block <= lastBlock; block++)
            {
                if (TOC.ZipBlockSizes[block] == 0U) // raw. full cluster used.
                {
                    stream.Write(_reader.ReadBytes(blockSize), 0, blockSize);
                }
                else
                {
                    var header = _reader.ReadUInt16BE();
                    _reader.BaseStream.Seek(-2, SeekOrigin.Current);

                    var array = _reader.ReadBytes((int)TOC.ZipBlockSizes[block]);

                    if (header == ZipHeader) // compressed
                    {
                        array = UnzipBlock(array);
                    }

                    stream.Write(array, 0, array.Length);
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

            T asset = new T();

            using (var ms = new MemoryStream(new byte[entry.Length], true))
            {
                InflateEntry(entry, ms);

                asset.ReadFrom(ms);
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
            List<T> inflatedEntries = new List<T>();

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
            var entry = TOC.Entries.FirstOrDefault(a => a.Path == "gfxassets/album_art/" + attr.AlbumArt.Substring(14) + "_256.dds");

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
                    _reader.Dispose();
                    _fileStream.Dispose();
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

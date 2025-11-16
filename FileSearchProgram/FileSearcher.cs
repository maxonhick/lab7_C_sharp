using MyLogger;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearchUtility
{
    /// <summary>
    /// Provides file search funstion with compression capabilities
    /// </summary>
    public class FileSearcher
    {
        private readonly Logger _logger;

        /// <summary>
        /// Initialize a new instance of FileSearcher
        /// </summary>
        /// <param name="logger">Logger instance for logging operations</param>
        public FileSearcher(Logger logger) { _logger = logger; }

        /// <summary>
        /// Searches for files with specified pattern in directory and subdirectories
        /// </summary>
        /// <param name="searchPath">Root directory path to search in</param>
        /// <param name="searchPattern">File search patter</param>
        /// <returns>List of found file paths</returns>
        public List<string> SearchFiles(string searchPath, string searchPattern)
        {
            var foundFiles = new List<string>();

            try
            {
                _logger.Info($"Starting file search in '{searchPath}' with pattern '{searchPattern}'");

                if (!Directory.Exists(searchPath))
                {
                    _logger.Warning($"Search path does not exists {searchPath}");
                    return foundFiles;
                }

                foundFiles.AddRange(Directory.GetFiles(searchPath, searchPattern, SearchOption.AllDirectories));
                _logger.Info($"Found {foundFiles.Count} files matching pattern");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error during file search operation");
            }

            return foundFiles;
        }

        /// <summary>
        /// Display file content to console using FileStream
        /// </summary>
        /// <param name="filePath">Path of the file to display</param>
        public void DisplayFileContent(string filePath)
        {
            try
            {
                _logger.Info($"Displaing content of file {filePath}");

                if (!File.Exists(filePath))
                {
                    _logger.Warning($"File not found {filePath}");
                    return;
                }

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    Console.WriteLine($"     Content of {filePath}");
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("     End of file content");
                }

                _logger.Info($"Successfully displayed content of {filePath}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to display content of file {filePath}");
            }
        }

        /// <summary>
        /// Compresses a file using GZip compression
        /// </summary>
        /// <param name="sourceFilePath">Path to the source file</param>
        /// <param name="compressedFilePath">Path for the compressed file</param>
        public void CompressFile(string sourceFilePath, string compressedFilePath)
        {
            try
            {
                _logger.Info($"Compressing file {sourceFilePath} to {compressedFilePath}");

                if (!File.Exists(sourceFilePath))
                {
                    throw new FileNotFoundException($"Source file not found: {sourceFilePath}");
                }

                using (var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
                using (var targetStream = File.Create(compressedFilePath))
                using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                {
                    sourceStream.CopyTo(compressionStream);
                }

                var originalSize = new FileInfo(sourceFilePath).Length;
                var compressedSize = new FileInfo(compressedFilePath).Length;
                var compressionRatio = (1 - (double)compressedSize / originalSize) * 100;

                _logger.Info($"File compression completed. Original: {originalSize} bytes, Compressed: {compressedSize} bytes, Ratio: {compressionRatio:F2}%");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to compress file: {sourceFilePath}");
                throw;
            }
        }

        /// <summary>
        /// Decompresses a GZip compressed file
        /// </summary>
        /// <param name="compressedFilePath">Path to the compressed file</param>
        /// <param name="decompressedFilePath">Path for the decompressed file</param>
        public void DecompressFile(string compressedFilePath, string decompressedFilePath)
        {
            try
            {
                _logger.Info($"Decompressing file {compressedFilePath} to {decompressedFilePath}");

                if (!File.Exists(compressedFilePath))
                {
                    throw new FileNotFoundException($"Compressed file not found: {compressedFilePath}");
                }

                using (var sourceStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read))
                using (var decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                using (var targetStream = File.Create(decompressedFilePath))
                {
                    decompressionStream.CopyTo(targetStream);
                }

                _logger.Info($"File decompression completed: {decompressedFilePath}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to decompress file: {compressedFilePath}");
                throw;
            }
        }
    }
}

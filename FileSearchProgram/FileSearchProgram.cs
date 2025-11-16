using MyLogger;

namespace FileSearchUtility
{
    /// <summary>
    /// Main program class for file search and compression demonstration
    /// </summary>
    class FileSearchProgram
    {
        /// <summary>
        /// Main entry point for file search demonstration
        /// </summary>
        static void Main()
        {
            var logger = new Logger(new LoggerConfig
            {
                WriteToConsole = true,
                WriteToFile = true,
                LogFilePath = $"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}_FileSearch.log",
                MinLogLevel = LogLevel.DEBUG
            });

            try
            {
                logger.Info("Starting file search and compression demo");

                var fileSearcher = new FileSearcher(logger);
                Console.WriteLine("    File Search and Compression Utility");

                Console.Write("Enter search path: ");
                string searchPath = Console.ReadLine();

                Console.Write("Enter file search pattern (e.g., *.txt, *.xml): ");
                string searchPattern = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(searchPath))
                {
                    searchPath = "."; 
                }

                if (string.IsNullOrWhiteSpace(searchPattern))
                {
                    searchPattern = "*.*"; 
                }

                List<string> foundFiles = fileSearcher.SearchFiles(searchPath, searchPattern);

                if (foundFiles.Count == 0)
                {
                    Console.WriteLine("No files found matching the specified criteria.");
                    logger.Info("No files found matching search criteria");
                    return;
                }

                Console.WriteLine($"\nFound {foundFiles.Count} file(s):");
                for (int i = 0; i < foundFiles.Count; i++)
                {
                    var fileInfo = new System.IO.FileInfo(foundFiles[i]);
                    Console.WriteLine($"{i + 1}. {foundFiles[i]} ({fileInfo.Length} bytes)");
                }

                Console.Write($"\nEnter file number to display content (1-{foundFiles.Count}): ");
                if (int.TryParse(Console.ReadLine(), out int fileNumber) && fileNumber >= 1 && fileNumber <= foundFiles.Count)
                {
                    string selectedFile = foundFiles[fileNumber - 1];

                    fileSearcher.DisplayFileContent(selectedFile);

                    Console.Write("Do you want to compress this file? (y/n): ");
                    string compressChoice = Console.ReadLine()?.ToLower();

                    if (compressChoice == "y" || compressChoice == "yes")
                    {
                        string compressedFile = selectedFile + ".gz";
                        fileSearcher.CompressFile(selectedFile, compressedFile);

                        Console.WriteLine($"File compressed successfully: {compressedFile}");

                        Console.Write("Do you want to decompress the file? (y/n): ");
                        string decompressChoice = Console.ReadLine()?.ToLower();

                        if (decompressChoice == "y" || decompressChoice == "yes")
                        {
                            string decompressedFile = selectedFile + ".decompressed";
                            fileSearcher.DecompressFile(compressedFile, decompressedFile);
                            Console.WriteLine($"File decompressed successfully: {decompressedFile}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid file number selected.");
                    logger.Warning("User entered invalid file number");
                }

                logger.Info("File Search and Compression Demo completed successfully");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "File Search and Compression Demo failed");
            }
        }
    }
}
using System;
using System.IO;
using System.Text;

namespace MyLogger
{
    /// <summary>
    /// Configuration class for logger settings
    /// </summary>
    public class LoggerConfig
    {
        public string LogFilePath { get; set; } = "app.log";
        public LogLevel MinLogLevel { get; set; } = LogLevel.INFO;
        public bool WriteToConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = true;
    }

    /// <summary>
    /// Enumeration of available log levels
    /// </summary>
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        CRITICAL
    }

    /// <summary>
    /// Main logger class for writing log messages to console and/or file
    /// </summary>
    public class Logger
    {
        private readonly string _logFilePath;
        private readonly LogLevel _minLogLevel;
        private readonly object _lockObject = new object();
        private readonly bool _writeToConsole;
        private readonly bool _writeToFile;

        /// <summary>
        /// Initializes a new instance of Logger with specified parameters
        /// </summary>
        /// <param name="logFilePath">Path to the log file</param>
        /// <param name="minLogLevel">Minimum log level to record</param>
        /// <param name="writeToConsole">Whether to write logs to console</param>
        /// <param name="writeToFile">Whether to write logs to file</param>
        public Logger(string logFilePath = "app.log", LogLevel minLogLevel = LogLevel.INFO,
                      bool writeToConsole = true, bool writeToFile = true)
        {
            _logFilePath = logFilePath;
            _minLogLevel = minLogLevel;
            _writeToConsole = writeToConsole;
            _writeToFile = writeToFile;

            if (_writeToFile)
            {
                var directory = Path.GetDirectoryName(_logFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of Logger with configuration object
        /// </summary>
        /// <param name="config">Logger configuration object</param>
        public Logger(LoggerConfig config)
        {
            _logFilePath = config.LogFilePath;
            _minLogLevel = config.MinLogLevel;
            _writeToConsole = config.WriteToConsole;
            _writeToFile = config.WriteToFile;

            if (_writeToFile)
            {
                var directory = Path.GetDirectoryName(_logFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }

        /// <summary>
        /// Writes a debug level log message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Debug(string message)
        {
            Log(LogLevel.DEBUG, message);
        }

        /// <summary>
        /// Writes an info level log message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Info(string message)
        {
            Log(LogLevel.INFO, message);
        }

        /// <summary>
        /// Writes a warning level log message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Warning(string message)
        {
            Log(LogLevel.WARNING, message);
        }

        /// <summary>
        /// Writes an error level log message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Error(string message)
        {
            Log(LogLevel.ERROR, message);
        }

        /// <summary>
        /// Writes an error level log message with exception details
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="message">Optional message to include with exception</param>
        public void Error(Exception exception, string message = null)
        {
            var fullMessage = message != null ? $"{message}: {exception}" : exception.ToString();
            Log(LogLevel.ERROR, fullMessage);
        }

        /// <summary>
        /// Writes a critical level log message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Critical(string message)
        {
            Log(LogLevel.CRITICAL, message);
        }

        /// <summary>
        /// Writes a critical level log message with exception details
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="message">Optional message to include with exception</param>
        public void Critical(Exception exception, string message = null)
        {
            var fullMessage = message != null ? $"{message}: {exception}" : exception.ToString();
            Log(LogLevel.CRITICAL, fullMessage);
        }

        /// <summary>
        /// Internal method to handle log message processing
        /// </summary>
        /// <param name="level">Log level of the message</param>
        /// <param name="message">The message to log</param>
        private void Log(LogLevel level, string message)
        {
            if (level < _minLogLevel)
                return;

            var logEntry = FormatLogEntry(level, message);

            lock (_lockObject)
            {
                if (_writeToConsole)
                {
                    WriteToConsole(level, logEntry);
                }

                if (_writeToFile)
                {
                    WriteToFile(logEntry);
                }
            }
        }

        /// <summary>
        /// Formats a log entry with timestamp and log level
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Log message</param>
        /// <returns>Formatted log entry string</returns>
        private string FormatLogEntry(LogLevel level, string message)
        {
            return $"{DateTime.Now:yyyy-MM-dd_HH:mm:ss.fff} [{level,-8}] {message}";
        }

        /// <summary>
        /// Writes a log entry to the console with appropriate color coding
        /// </summary>
        /// <param name="level">Log level for color determination</param>
        /// <param name="logEntry">Formatted log entry to write</param>
        private void WriteToConsole(LogLevel level, string logEntry)
        {
            var originalColor = Console.ForegroundColor;

            try
            {
                Console.ForegroundColor = GetConsoleColor(level);
                Console.WriteLine(logEntry);
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }

        /// <summary>
        /// Gets the console color for a specific log level
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Console color for the log level</returns>
        private ConsoleColor GetConsoleColor(LogLevel level)
        {
            return level switch
            {
                LogLevel.DEBUG => ConsoleColor.Gray,
                LogLevel.INFO => ConsoleColor.White,
                LogLevel.WARNING => ConsoleColor.Yellow,
                LogLevel.ERROR => ConsoleColor.Red,
                LogLevel.CRITICAL => ConsoleColor.DarkRed,
                _ => ConsoleColor.White
            };
        }

        /// <summary>
        /// Writes a log entry to the log file
        /// </summary>
        /// <param name="logEntry">Formatted log entry to write</param>
        private void WriteToFile(string logEntry)
        {
            try
            {
                using (var writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears the contents of the log file
        /// </summary>
        public void ClearLogFile()
        {
            if (_writeToFile && File.Exists(_logFilePath))
            {
                lock (_lockObject)
                {
                    File.WriteAllText(_logFilePath, string.Empty);
                }
            }
        }
    }
}

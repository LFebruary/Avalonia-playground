// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Playground.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Playground.Constants.LoggingConstants;

namespace Playground
{
    enum LogType
    {
        AppStartup,
        AppShutdown,
        ComPortStart,
        ComPortStop,
        SocketCreate,
        SocketStop,
        UnexpectedException,
        FarfulcrumException,
        CaughtException,
        ComPortAverageReadingsRate,
        SocketAverageTransmissionRate,
        SocketConnectionEstablished,
    }

    public static partial class LoggingService
    {
        private static readonly List<Tuple<LogType, DateTime, string>> _logs = [];
        internal static void WriteLog(LogType type, string? data = null) => _logs.Add(Tuple.Create(type, DateTime.Now, data ?? string.Empty));
        internal static void WriteLog(LogType type, int data) => _logs.Add(Tuple.Create(type, DateTime.Now, data.ToString()));

        private static readonly StringBuilder _stringBuilder = new();

        internal static void StartLoggingSession()
        {
            if (Directory.Exists(LogFolderPath) == false)
            {
                _ = Directory.CreateDirectory(LogFolderPath);
            }

            if (File.Exists(LogFilePath) == false)
            {
                using StreamWriter? streamWriter = File.CreateText(LogFilePath);
                streamWriter.Close();
            }
        }

        private static string _CreateLogString(bool intentionallyDumpingLogs = false)
        {
            string result = string.Empty;

            if (_logs.Count == 0)
            {
                return NoLogsAvailableToWrite;
            }

            result += "\n";

            if (intentionallyDumpingLogs)
            {
                result += DumpingLogs;
            }
            else
            {
                result += ShuttingDownApp;
            }


            result += "\n";

            _logs.ForEach(element =>
            {
                result += "\n";
                result += element.Item2.ToString(DateTimeFormat);
                result += " \t";


                switch (element.Item1)
                {
                    case LogType.AppStartup:
                        result += AppStarted;
                        break;
                    case LogType.AppShutdown:
                        result += AppShutdown;
                        break;
                    case LogType.ComPortStart:
                        result += StartedListeningCOMPort(element.Item3);
                        break;
                    case LogType.ComPortStop:
                        result += StoppedListeningCOMPort(element.Item3);
                        break;
                    case LogType.SocketCreate:
                        result += SocketStartedListening(element.Item3);
                        break;
                    case LogType.SocketStop:
                        result += SocketStoppedListening(element.Item3);
                        break;
                    case LogType.UnexpectedException:
                        result += UnexpectedException(element.Item3);
                        break;
                    case LogType.FarfulcrumException:
                        result += FarfulcrumException(element.Item3);
                        break;
                    case LogType.CaughtException:
                        result += CaughtException(element.Item3);
                        break;
                    case LogType.ComPortAverageReadingsRate:
                        result += AverageReadingsPerMinute(element.Item3);
                        break;
                    case LogType.SocketAverageTransmissionRate:
                        result += AverageTransmissionsPerMinute(element.Item3);
                        break;
                    case LogType.SocketConnectionEstablished:
                        result += EstablishedSocketConnection(element.Item3);
                        break;
                }
            });

            result += "\n";

            return result;
        }

        internal static int _socketConnectionsBetweenLastDump = 0;

        private static readonly string _folderPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string LogFolderPath => $"{_folderPath}logs";
        private static string LogFilePath => $"{LogFolderPath}\\LOG_{DateTime.Now:yyyy_MM_dd}.txt";

        internal static int _comPortReadingsBetweenDump = 0;

        internal static void DumpLogs(bool dumpingLogs = false)
        {
            if (Directory.Exists(LogFolderPath) == false)
            {
                _ = Directory.CreateDirectory(LogFolderPath);
            }

            _ = _stringBuilder.Append(_CreateLogString(dumpingLogs));

            _logs.Clear();

            CustomDebug.WriteLine(_stringBuilder.ToString());
            File.AppendAllText(LogFilePath, _stringBuilder.ToString());
            _ = _stringBuilder.Clear();
        }
    }
}

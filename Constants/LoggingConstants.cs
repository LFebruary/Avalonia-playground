using System;

namespace Playground.Constants
{
    public static partial class LoggingConstants
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        public const string AppStarted              = "App started";
        public const string AppShutdown             = "App closed";
        public const string NoLogsAvailableToWrite  = "No previously persisted logs to write...";
        public const string ShuttingDownApp         = "Shutting down app...";
        
        public static string AverageReadingsPerMinute(object readings)              => $"Average readings COM Port readings: {readings} per minute";
        public static string AverageTransmissionsPerMinute(object transmissions)    => $"Average socket transmissions: {transmissions} per minute";
        public static string CaughtException(object exceptionMessage)               => $"Caught exception ({exceptionMessage})";
        public static string DumpingLogs                                            => $"Dumping logs at {DateTime.Now}";
        public static string EstablishedSocketConnection(object remoteIP)           => $"Socket connection established to remote ip {remoteIP}";
        public static string FarfulcrumException(object exceptionMessage)           => $"Farfulcrum exception ({exceptionMessage})";
        public static string StartedListeningCOMPort(object comPort)                => $"Started listening to COM port {comPort}";
        public static string StoppedListeningCOMPort(object comPort)                => $"Stopped listening to COM port {comPort}";
        public static string SocketStartedListening(object ipAddress)               => $"Listening socket started listening at {ipAddress}";
        public static string SocketStoppedListening(object ipAddress)               => $"Listening socket stopped listening at {ipAddress}";
        public static string UnexpectedException(object exceptionMessage)           => $"Unexpected exception ({exceptionMessage})";
        
        
        
        
    }
}

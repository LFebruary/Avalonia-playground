namespace Playground.Constants
{
    public static partial class ErrorMessageConstants
    {
        #region Miscellaneous error messages
        public const string StabilityIndicatorAndIdenticalReadingsSelected = "Both the stability indicator and sequence of identical readings got the same selection. Reselect one of the options and try again.";
        public const string NoStabilityIndicatorSnippet         = "Stability indicator active, but no character snippet provided. Character snippet is required to proceed.";
        public const string StabilityIndicatorPositionInvalid   = "Stability indicator can not start at position lower than one.";
        public const string InvalidIdenticalReadingQuantity     = "Number of readings for sequence of identical readings can not be lower than one.";
        public const string ScaleStringInvalidPositions         = "Scale string settings have to indicators' positions greater than or equal to zero.";
        public const string ScaleStringEndPositionInvalid       = "Scale string's end position can not be less than starting position.";
        public const string ScaleStringRquiredLengthInvalid     = "Scale string required length setting requires a length greater than zero";
        #endregion

        #region Serial error messages
        public const string CouldNotConnectToSerialPort         = "Could not connect to serial port: ";
        public const string InvalidBaudRateSelected             = "An invalid baud rate has been specified. Reselect baud rate and try again.";
        public const string InvalidDataBitsSelected             = "An invalid databits value has been specified. Reselect databits and try again.";
        public const string InvalidParity                       = "Invalid value for parity encountered.";
        public const string InvalidStopbits                     = "Invalid value for stop bits encountered.";
        public const string InvalidStopbitsSelected             = "An invalid stop bits value has been specified. Reselect stop bits and try again.";
        public const string NoParitySelected                    = "Parity is required to start listening to COM Port. Please select parity and try again.";
        public const string SelectedPortBlank                   = "Selected port is blank.";
        public const string SelectedPortDoesNotExist            = "Selected port does not exist in ports associated with computer.";
        public const string SerialPortAlreadyActive             = "Serial port listener already instantiated. First stop listening to current port before changing ports.";
        public const string SerialPortAlreadyOpen               = "Serial port is already open, re-select port and try again.";
        public const string SerialPortInUseByAnotherProcess     = "Another process on the system already has the specified COM port open. Re-select port and try again.";
        public const string SerialPortNeverInstantiated         = "Serial port listener was never instantiated.";
        public const string SerialPortTimeout                   = "Serial port did not return a value in time";
        #endregion

        #region Socket error messages
        public const string CouldNotDetermineLocalIPAddress     = "Could not determine local IP address to broadcast serial readings on.";
        public const string SocketCouldNotSendData              = "Exception occurred during socket data send.";
        public const string SocketListenerNotInstantiated       = "Listening socket was never instantiated";
        public const string SocketListenerNotStarted            = "Could not get socket listener started.";
        public const string SocketLocalIPUndetermined           = "Could not determine local ip address via socket";
        #endregion
    }
}

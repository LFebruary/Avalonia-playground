using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Playground.Constants
{
    public static partial class SerialConstants
    {
        #region Baud rate
        public static readonly int DefaultBaudRate = 9600;
        public static readonly IList<int> BaudRates = new ReadOnlyCollection<int>(
            new List<int> {
                        110,
                        300,
                        600,
                        1200,
                        2400,
                        4800,
                        9600,
                        19200
            });
        #endregion

        #region Databits
        public static readonly int DefaultDatabits = 8;
        public static readonly IList<int> DataBits = new ReadOnlyCollection<int>(
            new List<int> {
                6,
                7,
                8
            });
        #endregion

        #region Flow control
        public const string FlowControlCtsRts = "CTS/RTS";
        public const string FlowControlDsrDtr = "DSR/DTS";
        public const string FlowControlXonXoff = "XON/XOFF";
        public const string FlowControlNone = "NONE";
        public const string FlowControlDefault = FlowControlNone;
        #endregion

        #region Parity
        public const string DefaultParity = NoParity;
        public const string EvenParity    = "Even";
        public const string OddParity     = "Odd";
        public const string NoParity      = "None";
        public static readonly IList<string> ParityValues = new ReadOnlyCollection<string>(
            new List<string> {
                EvenParity,
                OddParity,
                NoParity
            });
        #endregion

        #region Scale string
        public static readonly int DefaultScaleStringWeightStartPosition = 11;
        public static readonly int DefaultScaleStringWeightEndPosition = 19;
        public static readonly int DefaultScaleStringMinimumLength = 17;
        #endregion

        #region Stability
        public const string DefaultStabilityIndicatorSnippet = "ST";

        public static readonly int DefaultStabilityIndicatorStartPosition = 1;
        public static readonly int DefaultIdenticalReadingQuantity        = 5;
        #endregion

        #region Stop bits
        public static readonly int DefaultStopbits = 1;
        public static readonly IList<int> StopBits = new ReadOnlyCollection<int>(
            new List<int> {
                1,
                2
            });
        #endregion
    }
}

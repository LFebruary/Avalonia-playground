namespace Playground.Constants
{
    public static partial class TooltipConstants
    {
        internal static string StabilityIndicatorSnipTooltip(bool active, string snippet)
            => $"Uses specified character snippet {(string.IsNullOrEmpty(snippet) && active ? "" : $"({snippet})")} to determine if scale reading is stable.";

        internal static string IdenticalReadingsTooltip(bool active, int quantity)
            => $"Declares scale reading stable only if {(quantity > 0 && active ? quantity : "specified number of")} identical readings have been encountered.";

        public const string WeightStartPosition     = "Starting position of weight in value received from serial port.";
        public const string WeightEndPosition       = "Ending position of weight in value received from serial port.";
        public const string RequiredLength          = "Characters required from scale to be accepted as a valid reading.";
        public const string CharacterSnippet        = "The snippet that the scale provides to indicate whether the reading is stable.";
        public const string NumberOfReadings        = "The sequential number of identical readings needed to indicate whether the reading is stable.";
        public const string StabilityIndicatorStart = "The position in the value received from the serial port where the character snippet is to be expected.";
        public const string RequiredLengthToggle    = "Whether the value returned from the serial port has to conform to a specified length.";
        public const string ShowQRCode              = "Display QR code that will automatically configure settings for ViTrax Mobile device.";
    }
}

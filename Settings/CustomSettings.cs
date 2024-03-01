// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Playground.Constants;
using Playground.Properties;
using System;
using System.Collections.Specialized;
using System.IO.Ports;
using static Playground.Constants.SerialConstants;

namespace Playground
{
    public static partial class CustomSettings
    {
        #region String settings
        public enum StringSetting
        {
            ComPort,
            Parity,
            FlowControl,
            StabilityIndicatorSnippet
        }

        public enum FlowControl
        {
            Ctr_Rts,
            Dsr_Dtr,
            Xon_Xoff,
            None
        }

        public static FlowControl? GetFlowControl()
        {
            return GetSetting(StringSetting.FlowControl) switch
            {
                FlowControlCtsRts => FlowControl.Ctr_Rts,
                FlowControlDsrDtr => FlowControl.Dsr_Dtr,
                FlowControlXonXoff => FlowControl.Xon_Xoff,
                FlowControlNone => FlowControl.None,
                _ => null,
            };
        }

        public static Parity? GetParity()
        {
            return GetSetting(StringSetting.Parity) switch
            {
                EvenParity => Parity.Even,
                OddParity => Parity.Odd,
                NoParity => Parity.None,
                _ => null,
            };
        }

        private static string _DefaultIfInvalid(string value, string defaultValue) => string.IsNullOrWhiteSpace(value) ? defaultValue : value;

        public static string GetSetting(StringSetting setting)
        {
            return setting switch
            {
                StringSetting.ComPort => Settings.Default.SerialPort,

                StringSetting.Parity => _DefaultIfInvalid(
                    ParityValues.Contains(Settings.Default.Parity)
                        ? Settings.Default.Parity
                        : DefaultParity,
                    DefaultParity),

                StringSetting.FlowControl => _DefaultIfInvalid(Settings.Default.Flow_control, FlowControlNone),
                StringSetting.StabilityIndicatorSnippet => _DefaultIfInvalid(Settings.Default.Stability_indicator_snippet, DefaultStabilityIndicatorSnippet),
                _ => throw new ArgumentOutOfRangeException(nameof(setting)),
            };
        }

        public static void SetSetting(this StringSetting setting, string value)
        {
            switch (setting)
            {
                case StringSetting.ComPort:
                    Settings.Default.SerialPort = value;
                    break;
                case StringSetting.Parity:
                    Settings.Default.Parity = value;
                    break;
                case StringSetting.FlowControl:
                    Settings.Default.Flow_control = value;
                    break;
                case StringSetting.StabilityIndicatorSnippet:
                    Settings.Default.Stability_indicator_snippet = value;
                    break;
            }
        }
        #endregion

        #region Integer settings
        public enum IntSetting
        {
            BaudRate,
            Databits,
            Stopbits,
            StabilityIndicatorStartPosition,
            IdenticalReadingQuantity,
            ScaleStringWeightStartPosition,
            ScaleStringWeightEndPosition,
            ScaleStringRequiredLength,
            BroadcastPort,
            SerialTimeoutMs
        }

        private static int _DefaultIfInvalid(int value, int defaultValue) => value <= -1 ? defaultValue : value;

        public static int GetSetting(IntSetting setting)
        {
            return setting switch
            {
                IntSetting.BaudRate => _DefaultIfInvalid(
                    BaudRates.Contains(Settings.Default.BaudRate)
                        ? Settings.Default.BaudRate
                        : DefaultBaudRate,
                    DefaultBaudRate),

                IntSetting.Databits => _DefaultIfInvalid(
                    DataBits.Contains(Settings.Default.Databits)
                        ? Settings.Default.Databits
                        : DefaultDatabits,
                    DefaultDatabits),

                IntSetting.Stopbits => _DefaultIfInvalid(
                    SerialConstants.StopBits.Contains(Settings.Default.Stop_bits)
                        ? Settings.Default.Stop_bits
                        : DefaultStopbits,
                    DefaultStopbits),


                IntSetting.StabilityIndicatorStartPosition => _DefaultIfInvalid(Settings.Default.Stability_indicator_starting_position, DefaultStabilityIndicatorStartPosition),
                IntSetting.IdenticalReadingQuantity => _DefaultIfInvalid(Settings.Default.Number_of_identical_readings, DefaultIdenticalReadingQuantity),
                IntSetting.ScaleStringWeightStartPosition => _DefaultIfInvalid(Settings.Default.Scale_string_weight_start_position, DefaultScaleStringWeightStartPosition),
                IntSetting.ScaleStringWeightEndPosition => _DefaultIfInvalid(Settings.Default.Scale_string_weight_end_position, DefaultScaleStringWeightEndPosition),
                IntSetting.ScaleStringRequiredLength => _DefaultIfInvalid(Settings.Default.Scale_string_minimum_length, DefaultScaleStringMinimumLength),
                IntSetting.BroadcastPort => _DefaultIfInvalid(Settings.Default.BroadcastPort, 5050),
                IntSetting.SerialTimeoutMs => _DefaultIfInvalid(Settings.Default.Serial_timeout_ms, 1000),
                _ => throw new ArgumentOutOfRangeException(nameof(setting)),
            };
        }

        public static void SetSetting(this IntSetting setting, int value)
        {
            switch (setting)
            {
                case IntSetting.BaudRate:
                    Settings.Default.BaudRate = value;
                    break;
                case IntSetting.Databits:
                    Settings.Default.Databits = value;
                    break;
                case IntSetting.Stopbits:
                    Settings.Default.Stop_bits = value;
                    break;
                case IntSetting.StabilityIndicatorStartPosition:
                    Settings.Default.Stability_indicator_starting_position = value;
                    break;
                case IntSetting.IdenticalReadingQuantity:
                    Settings.Default.Number_of_identical_readings = value;
                    break;
                case IntSetting.ScaleStringWeightStartPosition:
                    Settings.Default.Scale_string_weight_start_position = value;
                    break;
                case IntSetting.ScaleStringWeightEndPosition:
                    Settings.Default.Scale_string_weight_end_position = value;
                    break;
                case IntSetting.ScaleStringRequiredLength:
                    Settings.Default.Scale_string_minimum_length = value;
                    break;
                case IntSetting.BroadcastPort:
                    Settings.Default.BroadcastPort = value;
                    break;
                case IntSetting.SerialTimeoutMs:
                    Settings.Default.Serial_timeout_ms = value;
                    break;
            }
        }

        private static int MaxReadings => GetSetting(IntSetting.IdenticalReadingQuantity);
        #endregion

        #region Bool settings
        public enum BoolSetting
        {
            StabilityIndicatorActive,
            SequenceOfIdenticalReadingsActive,
            ScaleStringRequiredLength,
            TakeFullScaleString
        }

        public static bool GetSetting(BoolSetting setting)
        {
            return setting switch
            {
                BoolSetting.StabilityIndicatorActive => Settings.Default.Stability_indicator_active,
                BoolSetting.SequenceOfIdenticalReadingsActive => Settings.Default.Sequence_of_identical_readings_active,
                BoolSetting.ScaleStringRequiredLength => Settings.Default.Scale_string_must_conform_to_length,
                BoolSetting.TakeFullScaleString => Settings.Default.Take_full_scale_string,
                _ => throw new ArgumentOutOfRangeException(nameof(setting)),
            };
        }

        public static void SetSetting(this BoolSetting setting, bool value)
        {
            switch (setting)
            {
                case BoolSetting.StabilityIndicatorActive:
                    Settings.Default.Stability_indicator_active = value;
                    break;
                case BoolSetting.SequenceOfIdenticalReadingsActive:
                    Settings.Default.Sequence_of_identical_readings_active = value;
                    break;
                case BoolSetting.ScaleStringRequiredLength:
                    Settings.Default.Scale_string_must_conform_to_length = value;
                    break;
                case BoolSetting.TakeFullScaleString:
                    Settings.Default.Take_full_scale_string = value;
                    break;
            }
        }
        #endregion

        #region String collection settings
        public enum StringCollectionSetting
        {
            CollectionOfReceivedValues
        }
        //Need to be careful with this one...do not want to have too many readings being saved
        //Maybe a way to limit it to only the amount of specified readings
        public static StringCollection GetSetting(StringCollectionSetting setting)
        {
            return setting switch
            {
                StringCollectionSetting.CollectionOfReceivedValues => Settings.Default.Received_values,
                _ => throw new ArgumentOutOfRangeException(nameof(setting)),
            };
        }

        private static void _SetSetting(this StringCollectionSetting setting, StringCollection value)
        {
            switch (setting)
            {
                case StringCollectionSetting.CollectionOfReceivedValues:
                    Settings.Default.Received_values = value;
                    break;
            }
        }

        public static void Reset(this StringCollectionSetting setting)
        {
            switch (setting)
            {
                case StringCollectionSetting.CollectionOfReceivedValues:
                    Settings.Default.Received_values = [];
                    break;
            }
        }

        public static void Add(this StringCollectionSetting setting, string value)
        {
            switch (setting)
            {
                case StringCollectionSetting.CollectionOfReceivedValues:
                    if (GetSetting(BoolSetting.SequenceOfIdenticalReadingsActive) && MaxReadings > 0)
                    {
                        if (Settings.Default.Received_values?.Count == MaxReadings == true)
                        {
                            Settings.Default.Received_values.RemoveAt(Settings.Default.Received_values.Count - 1);

                        }
                        else if ((Settings.Default.Received_values?.Count > MaxReadings) == true)
                        {
                            int difference = Settings.Default.Received_values.Count - MaxReadings;
                            for (int i = 0; i < difference; i++)
                            {
                                Settings.Default.Received_values.RemoveAt(Settings.Default.Received_values.Count - 1);
                            }
                        }

                        if (Settings.Default.Received_values == null)
                        {
                            Settings.Default.Received_values =
                            [
                                value
                            ];
                        }
                        else
                        {
                            Settings.Default.Received_values.Insert(0, value);
                        }

                    }

                    break;
            }
        }

        public static void ClearCollectionOfReceivedValues()
        {
            _SetSetting(StringCollectionSetting.CollectionOfReceivedValues, []);
        }
        #endregion
    }
}

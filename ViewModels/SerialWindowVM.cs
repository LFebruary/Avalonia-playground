// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Playground.Constants;
using Playground.SerialReadSocketSend;
using Playground.Views;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using static Playground.CustomSettings;

namespace Playground.ViewModels
{
    public class SerialWindowVM : BaseViewModel
    {
        #region Constructor
        public SerialWindowVM(SerialWindow window) : base(window)
        {
            SelectedFlowControl = GetFlowControl();
            SelectedParity = GetParity();

            _backgroundSocketWorker.DoWork -= _BackgroundSocketWorker_DoWork;
            _backgroundSocketWorker.DoWork += _BackgroundSocketWorker_DoWork;

            _backgroundSocketWorker.RunWorkerCompleted -= _BackgroundSocketWorker_RunWorkerCompleted;
            _backgroundSocketWorker.RunWorkerCompleted += _BackgroundSocketWorker_RunWorkerCompleted;
        }
        #endregion

        #region Constants
        private const string _invalidValue = "NAN";
        #endregion

        #region Computed properties

        public bool BroadcastInputsEnabled => BroadcastingSerialValues == false;
        public string LastProcessedValue => _ProcessReading();
        public DateTime? LastReceivedValueTimestamp => string.IsNullOrWhiteSpace(LastReceivedValue) ? null : DateTime.Now;
        public string ListenToSerialButtonText => $"{(ListeningOnSerialPort ? "Stop" : "Start")} listening on serial port";
        public string ListenToSerialCaptionText => ListeningOnSerialPort == false
                    ? "Currently not listening to any serial port"
                    : SerialPortTools.IsPortOpen
                        ? $"Currently listening to serial port {SelectedComPort} with: Baudrate {SelectedBaudRate} \t Databits {SelectedDataBits} \t Stop bits {SelectedStopBits}"
                        : "Serial port is not open, but window attempting to listen to it. Restart program and try again.";
        public bool SerialInputsEnabled => ListeningOnSerialPort == false;
        public string SocketConnectionButtonText => $"{(BroadcastingSerialValues ? "Stop" : "Start")} broadcasting serial readings";
        public string SocketConnectionCaptionText => BroadcastingSerialValues
            ? $"Currently broadcasting values over\nsocket connection via: {SocketTools.IPAddress}:{SocketTools.Port}"
            : "Values are not being broadcasted over\nsocket connection.";

        public static int Port => SocketTools.Port;
        private static StringCollection ReceivedValues => GetSetting(StringCollectionSetting.CollectionOfReceivedValues);

        #region Tooltips
        public string StabilityIndicatorTooltip => TooltipConstants.StabilityIndicatorSnipTooltip(StabilityIndicatorActive, StabilityIndicatorSnippet);
        public string IdenticalReadingsTooltip => TooltipConstants.IdenticalReadingsTooltip(SequenceOfIdenticalReadingsActive, NumberOfIdenticalReadings);
        #endregion

        #endregion

        #region Read only properties
        private readonly BackgroundWorker _backgroundSocketWorker = new()
        {
            WorkerReportsProgress = false,
            WorkerSupportsCancellation = true
        };
        #endregion

        #region Read-write properties

        #region Basic COM port settings
        private IEnumerable<string> _comPorts = SerialPort.GetPortNames();
        public IEnumerable<string> ComPorts
        {
            get => _comPorts;
            set => SetProperty(ref _comPorts, value);
        }

        private string _selectedComPort = GetSetting(StringSetting.ComPort);
        public string SelectedComPort
        {
            get => _selectedComPort;
            set => SetProperty(ref _selectedComPort, value, () => CustomSettings.SetSetting(StringSetting.ComPort, SelectedComPort.Trim()));
        }

        private int _selectedBaudRate = GetSetting(IntSetting.BaudRate);
        public int SelectedBaudRate
        {
            get => _selectedBaudRate;
            set => SetProperty(ref _selectedBaudRate, value, () => CustomSettings.SetSetting(IntSetting.BaudRate, SelectedBaudRate));
        }

        private IEnumerable<int> _baudRates = SerialConstants.BaudRates;
        public IEnumerable<int> BaudRates
        {
            get => _baudRates;
            set => SetProperty(ref _baudRates, value);
        }

        private int _selectedDataBits = GetSetting(IntSetting.Databits);
        public int SelectedDataBits
        {
            get => _selectedDataBits;
            set => SetProperty(ref _selectedDataBits, value, () => CustomSettings.SetSetting(IntSetting.Databits, SelectedDataBits));
        }

        private IEnumerable<int> _dataBits = SerialConstants.DataBits;
        public IEnumerable<int> DataBits
        {
            get => _dataBits;
            set => SetProperty(ref _dataBits, value);
        }

        private int _selectedStopBits = GetSetting(IntSetting.Stopbits);
        public int SelectedStopBits
        {
            get => _selectedStopBits;
            set => SetProperty(ref _selectedStopBits, value, () => CustomSettings.SetSetting(IntSetting.Stopbits, SelectedStopBits));
        }

        private IEnumerable<int> _stopBits = SerialConstants.StopBits;
        public IEnumerable<int> StopBits
        {
            get => _stopBits;
            set => SetProperty(ref _stopBits, value);
        }
        #endregion

        #region Flow control
        private FlowControl? _selectedFlowControl;
        private FlowControl? SelectedFlowControl
        {
            get => _selectedFlowControl;
            set => SetProperty(ref _selectedFlowControl, value, () =>
            {
                SerialPortTools.SetFlowControl(SelectedFlowControl);
                switch (_selectedFlowControl)
                {
                    case FlowControl.Ctr_Rts:
                        CtsRtsSelected = true;
                        DsrDtrSelected = false;
                        XonXoffSelected = false;
                        NoneSelected = false;
                        CustomSettings.SetSetting(StringSetting.FlowControl, SerialConstants.FlowControlCtsRts);
                        break;
                    case FlowControl.Dsr_Dtr:
                        CtsRtsSelected = false;
                        DsrDtrSelected = true;
                        XonXoffSelected = false;
                        NoneSelected = false;
                        CustomSettings.SetSetting(StringSetting.FlowControl, SerialConstants.FlowControlDsrDtr);
                        break;
                    case FlowControl.Xon_Xoff:
                        CtsRtsSelected = false;
                        DsrDtrSelected = false;
                        XonXoffSelected = true;
                        NoneSelected = false;
                        CustomSettings.SetSetting(StringSetting.FlowControl, SerialConstants.FlowControlXonXoff);
                        break;
                    case FlowControl.None:
                        CtsRtsSelected = false;
                        DsrDtrSelected = false;
                        XonXoffSelected = true;
                        NoneSelected = true;
                        CustomSettings.SetSetting(StringSetting.FlowControl, SerialConstants.FlowControlNone);
                        break;
                }
            });
        }

        private bool _ctsRtsSelected;
        public bool CtsRtsSelected
        {
            get => _ctsRtsSelected;
            set => SetProperty(ref _ctsRtsSelected, value);
        }

        private bool _dsrDtrSelected;
        public bool DsrDtrSelected
        {
            get => _dsrDtrSelected;
            set => SetProperty(ref _dsrDtrSelected, value);
        }

        private bool _xonXoffSelected;
        public bool XonXoffSelected
        {
            get => _xonXoffSelected;
            set => SetProperty(ref _xonXoffSelected, value);
        }

        private bool _noneSelected;
        public bool NoneSelected
        {
            get => _noneSelected;
            set => SetProperty(ref _noneSelected, value);
        }
        #endregion

        #region Miscellaneous
        private string _lastReceivedValue = string.Empty;
        public string LastReceivedValue
        {
            get => _lastReceivedValue;
            set
            {
                if (_lastReceivedValue != value)
                {
                    _lastReceivedValue = value;
                    OnPropertyChanged();
                }

                if (ListeningOnSerialPort)
                {
                    CustomSettings.Add(StringCollectionSetting.CollectionOfReceivedValues, LastReceivedValue);
                }

                OnPropertyChanged(nameof(LastProcessedValue));
                OnPropertyChanged(nameof(LastReceivedValueTimestamp));
            }
        }

        private string _output = string.Empty;
        public string Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }

        private bool _takeFullWeightString = GetSetting(BoolSetting.TakeFullScaleString);
        public bool TakeFullWeightString
        {
            get => _takeFullWeightString;
            set => SetProperty(ref _takeFullWeightString, value);
        }

        private int _timeoutMilliseconds = GetSetting(IntSetting.SerialTimeoutMs);
        public int TimeoutMilliseconds
        {
            get => _timeoutMilliseconds;
            set => SetProperty(ref _timeoutMilliseconds, value, () => CustomSettings.SetSetting(IntSetting.SerialTimeoutMs, value));
        }
        #endregion

        #region Parity
        private Parity? _selectedParity;

        public Parity? SelectedParity
        {
            get => _selectedParity;
            set => SetProperty(ref _selectedParity, value, () =>
            {
                switch (_selectedParity)
                {
                    case Parity.Even:
                        EvenParityChecked = true;
                        OddParityChecked = false;
                        NoneParityChecked = false;
                        CustomSettings.SetSetting(StringSetting.Parity, SerialConstants.EvenParity);
                        break;
                    case Parity.Odd:
                        EvenParityChecked = false;
                        OddParityChecked = true;
                        NoneParityChecked = false;
                        CustomSettings.SetSetting(StringSetting.Parity, SerialConstants.OddParity);
                        break;
                    case Parity.None:
                        EvenParityChecked = false;
                        OddParityChecked = false;
                        NoneParityChecked = true;
                        CustomSettings.SetSetting(StringSetting.Parity, SerialConstants.NoParity);
                        break;
                    default:
                        break;
                }
            });
        }

        private bool _evenParityChecked;
        public bool EvenParityChecked
        {
            get => _evenParityChecked;
            set => SetProperty(ref _evenParityChecked, value);
        }

        private bool _oddParityChecked;
        public bool OddParityChecked
        {
            get => _oddParityChecked;
            set => SetProperty(ref _oddParityChecked, value);
        }

        private bool _noneParityChecked;
        public bool NoneParityChecked
        {
            get => _noneParityChecked;
            set => SetProperty(ref _noneParityChecked, value);
        }
        #endregion

        #region Scale stability
        private bool _stabilityIndicatorActive = GetSetting(BoolSetting.StabilityIndicatorActive);
        public bool StabilityIndicatorActive
        {
            get => _stabilityIndicatorActive;
            set => SetProperty(ref _stabilityIndicatorActive, value, () =>
            {
                CustomSettings.SetSetting(BoolSetting.SequenceOfIdenticalReadingsActive, !value);
                CustomSettings.SetSetting(BoolSetting.StabilityIndicatorActive, value);

                SequenceOfIdenticalReadingsActive = !value;
                OnPropertyChanged(nameof(StabilityIndicatorTooltip));
                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }

        private bool _sequenceOfIdenticalReadingsActive = GetSetting(BoolSetting.SequenceOfIdenticalReadingsActive);
        public bool SequenceOfIdenticalReadingsActive
        {
            get => _sequenceOfIdenticalReadingsActive;
            set => SetProperty(ref _sequenceOfIdenticalReadingsActive, value, () =>
            {
                CustomSettings.SetSetting(BoolSetting.StabilityIndicatorActive, !value);
                CustomSettings.SetSetting(BoolSetting.SequenceOfIdenticalReadingsActive, value);

                StabilityIndicatorActive = !value;
                OnPropertyChanged(nameof(IdenticalReadingsTooltip));
                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }

        private string _stabilityIndicatorSnippet = GetSetting(StringSetting.StabilityIndicatorSnippet);
        public string StabilityIndicatorSnippet
        {
            get => _stabilityIndicatorSnippet;
            set => SetProperty(ref _stabilityIndicatorSnippet, value, () =>
            {
                CustomSettings.SetSetting(StringSetting.StabilityIndicatorSnippet, value);
                OnPropertyChanged(nameof(StabilityIndicatorTooltip));
                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }

        private int _stabilityIndicatorStartingPosition = GetSetting(IntSetting.StabilityIndicatorStartPosition);
        public int StabilityIndicatorStartingPosition
        {
            get => _stabilityIndicatorStartingPosition;
            set => SetProperty(ref _stabilityIndicatorStartingPosition, value, () =>
            {
                CustomSettings.SetSetting(IntSetting.StabilityIndicatorStartPosition, StabilityIndicatorStartingPosition);
                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }

        private int _numberOfIdenticalReadings = GetSetting(IntSetting.IdenticalReadingQuantity);
        public int NumberOfIdenticalReadings
        {
            get => _numberOfIdenticalReadings;
            set => SetProperty(ref _numberOfIdenticalReadings, value, () =>
            {
                CustomSettings.SetSetting(IntSetting.IdenticalReadingQuantity, NumberOfIdenticalReadings);
                OnPropertyChanged(nameof(IdenticalReadingsTooltip));
                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }
        #endregion

        #region Scale string settings
        private int _weightStartPosition = GetSetting(IntSetting.ScaleStringWeightStartPosition);
        public int WeightStartPosition
        {
            get => _weightStartPosition;
            set => SetProperty(ref _weightStartPosition, value, () =>
            {
                CustomSettings.SetSetting(IntSetting.ScaleStringWeightStartPosition, WeightStartPosition);
                if (WeightStartPosition > WeightEndPosition)
                {
                    WeightEndPosition = WeightStartPosition;
                }

                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }

        private int _weightEndPosition = GetSetting(IntSetting.ScaleStringWeightEndPosition);
        public int WeightEndPosition
        {
            get => _weightEndPosition;
            set => SetProperty(ref _weightEndPosition, Math.Max(value, 1), () =>
            {
                CustomSettings.SetSetting(IntSetting.ScaleStringWeightEndPosition, WeightEndPosition);
                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }

        private bool _stringRequiredLengthActive = GetSetting(BoolSetting.ScaleStringRequiredLength);
        public bool StringRequiredLengthActive
        {
            get => _stringRequiredLengthActive;
            set => SetProperty(ref _stringRequiredLengthActive, value, () =>
            {
                CustomSettings.SetSetting(BoolSetting.ScaleStringRequiredLength, StringRequiredLengthActive);
                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }

        private int _scaleStringRequiredLength = GetSetting(IntSetting.ScaleStringRequiredLength);
        public int ScaleStringRequiredLength
        {
            get => _scaleStringRequiredLength;
            set => SetProperty(ref _scaleStringRequiredLength, value, () =>
            {
                CustomSettings.SetSetting(IntSetting.ScaleStringRequiredLength, ScaleStringRequiredLength);
                OnPropertyChanged(nameof(LastProcessedValue));
            });
        }
        #endregion

        #region Serial properties

        private bool _listeningOnSerialPort = false;
        public bool ListeningOnSerialPort
        {
            get => _listeningOnSerialPort;
            set => SetProperty(ref _listeningOnSerialPort, value, () =>
            {
                OnPropertyChanged(nameof(ListenToSerialButtonText));
                OnPropertyChanged(nameof(ListenToSerialCaptionText));
                OnPropertyChanged(nameof(SerialInputsEnabled));

                if (ListeningOnSerialPort == false)
                {
                    SerialPortTools.ValueUpdatedCallback = (_) => { };
                    SerialPortTools.StopListeningForcefully();
                    LastReceivedValue = string.Empty;
                    BroadcastingSerialValues = false;
                }
            });
        }
        #endregion

        #region Socket properties
        private bool _broadcastingSerialValues = false;
        public bool BroadcastingSerialValues
        {
            get => _broadcastingSerialValues;
            set => SetProperty(ref _broadcastingSerialValues, value, () =>
            {
                if (BroadcastingSerialValues)
                {
                    SocketTools.StartServer();
                }
                else
                {
                    SocketTools.StopServer();
                }

                OnPropertyChanged(nameof(SocketConnectionCaptionText));
                OnPropertyChanged(nameof(SocketConnectionButtonText));
                OnPropertyChanged(nameof(BroadcastingSerialValues));
            });
        }
        #endregion

        #endregion

        #region Commands
        public void BroadcastSerialValuesCommand()
        {
            if (ListeningOnSerialPort)
            {
                BroadcastingSerialValues = !BroadcastingSerialValues;
            }
        }

        public async void ListenOnSerialPortCommand()
        {
            async Task<bool> ErrorAction(string errorMessage, string action) => await ShowDialog(Dialogtype.Error, errorMessage, action, "Cancel");

            bool tempValue = !ListeningOnSerialPort;
            if (tempValue)
            {

                if (string.IsNullOrWhiteSpace(SelectedComPort))
                {
                    bool reselectSerialPort = await ErrorAction(
                        ErrorMessageConstants.SelectedPortBlank,
                        "Re-select port");

                    if (reselectSerialPort)
                    {
                        ((SerialWindow)_parentView).FocusPortPicker();
                    }

                    return;
                }
                else if (SerialPort.GetPortNames().Contains(SelectedComPort) == false)
                {
                    bool reselectSerialPort = await ErrorAction(
                        ErrorMessageConstants.SelectedPortDoesNotExist,
                        "Re-select port");

                    if (reselectSerialPort)
                    {
                        ((SerialWindow)_parentView).FocusPortPicker();
                    }

                    return;
                }
                else if (SelectedBaudRate <= 0 || BaudRates.Contains(SelectedBaudRate) == false)
                {
                    bool reselectBaudRate = await ErrorAction(
                        ErrorMessageConstants.InvalidBaudRateSelected,
                        "Re-select baud rate");

                    if (reselectBaudRate)
                    {
                        ((SerialWindow)_parentView).FocusBaudRatePicker();
                    }

                    return;
                }
                else if (SelectedDataBits <= 0 || DataBits.Contains(SelectedDataBits) == false)
                {
                    bool reselectDatabits = await ErrorAction(
                        ErrorMessageConstants.InvalidDataBitsSelected,
                        "Re-select databits");

                    if (reselectDatabits)
                    {
                        ((SerialWindow)_parentView).FocusDatabitsPicker();
                    }

                    return;
                }
                else if (SelectedStopBits <= 0 || StopBits.Contains(SelectedStopBits) == false)
                {
                    bool reselectStopbits = await ErrorAction(
                        ErrorMessageConstants.InvalidStopbitsSelected,
                        "Re-select stop bits");

                    if (reselectStopbits)
                    {
                        ((SerialWindow)_parentView).FocusStopBitsPicker();
                    }

                    return;
                }
                else if (SelectedParity == null)
                {
                    _ = await ShowDialog(Dialogtype.Error, ErrorMessageConstants.NoParitySelected);
                    return;
                }
                else if ((StabilityIndicatorActive == false && SequenceOfIdenticalReadingsActive == false)
                || (StabilityIndicatorActive && SequenceOfIdenticalReadingsActive))
                {
                    _ = await ShowDialog(Dialogtype.Error, ErrorMessageConstants.StabilityIndicatorAndIdenticalReadingsSelected);
                    return;
                }
                else if (StabilityIndicatorActive && string.IsNullOrWhiteSpace(StabilityIndicatorSnippet))
                {
                    bool configStabilityIndicator = await ErrorAction(
                        ErrorMessageConstants.NoStabilityIndicatorSnippet,
                        "Configure stability indicator snippet");

                    if (configStabilityIndicator)
                    {
                        ((SerialWindow)_parentView).FocusStabilityIndicatorSnippet();
                    }

                    return;
                }
                else if (StabilityIndicatorStartingPosition <= 0)
                {
                    bool selectStartPos = await ErrorAction(
                        ErrorMessageConstants.StabilityIndicatorPositionInvalid,
                        "Change starting position");

                    if (selectStartPos)
                    {
                        ((SerialWindow)_parentView).FocusStartingPosition();
                    }

                    return;
                }
                else if (SequenceOfIdenticalReadingsActive && (NumberOfIdenticalReadings <= 0))
                {
                    bool correctNumberOfReadings = await ErrorAction(
                        ErrorMessageConstants.InvalidIdenticalReadingQuantity,
                        "Change number of sequential readings");

                    if (correctNumberOfReadings)
                    {
                        ((SerialWindow)_parentView).FocusNumberOfReadings();
                    }

                    return;
                }
                else if (WeightStartPosition < 0 || WeightEndPosition < 0)
                {
                    bool correctScaleStringPositions = await ErrorAction(
                        ErrorMessageConstants.ScaleStringInvalidPositions,
                        "Correct scale string positions");

                    if (correctScaleStringPositions)
                    {
                        if (WeightStartPosition < 0)
                        {
                            ((SerialWindow)_parentView).FocusScaleStringStartPosition();
                        }
                        else
                        {
                            ((SerialWindow)_parentView).FocusScaleStringEndPosition();
                        }
                    }

                    return;
                }
                else if ((TakeFullWeightString == false) && (WeightStartPosition != WeightEndPosition) && (WeightEndPosition <= WeightStartPosition))
                {
                    bool correctEndPosition = await ErrorAction(
                        ErrorMessageConstants.ScaleStringEndPositionInvalid,
                        "Correct scale string end position");

                    if (correctEndPosition)
                    {
                        ((SerialWindow)_parentView).FocusScaleStringEndPosition();
                    }

                    return;
                }
                else if (StringRequiredLengthActive && (ScaleStringRequiredLength <= 0))
                {
                    bool selectRequiredLength = await ErrorAction(
                        ErrorMessageConstants.ScaleStringRquiredLengthInvalid,
                        "Correct required length");

                    if (selectRequiredLength)
                    {
                        ((SerialWindow)_parentView).FocusRequiredLength();
                    }

                    return;
                }

                try
                {
                    SerialPortTools.GetPortAndStartListening();
                    SerialPortTools.ValueUpdatedCallback = _ValueUpdatedCallback;
                    SerialPortTools.ThreadExceptionCallback = _ThreadExceptionCallback;
                    LoggingService.WriteLog(LogType.ComPortStart, SelectedComPort);
                }
                catch (CustomException exception)
                {
                    LoggingService.WriteLog(LogType.FarfulcrumException, exception.Message);
                    await _ThreadExceptionCallback(exception);
                    return;
                }
            }

            ListeningOnSerialPort = tempValue;
        }


        public async void ShowSocketQRCodeCommand()
        {
            if (BroadcastingSerialValues && ListeningOnSerialPort)
            {
                QRCodeGenerator qrGenerator = new();
                QRCodeData data = qrGenerator.CreateQrCode($"{SocketTools.IPAddress}:{SocketTools.Port}", QRCodeGenerator.ECCLevel.Q);
                await ShowDialog(new BitmapByteQRCode(data));
            }
            else
            {
                _ = await ShowDialog(Dialogtype.Error, "Can not show QR code when socket broadcast is inactive.");
            }
        }
        #endregion

        #region Methods

        #region Background worker methods
        private void _BackgroundSocketWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (_backgroundSocketWorker.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else if (SocketTools.SocketAvailable)
            {
                SocketTools.SendData(_ProcessedValue());
            }
        }

        private async void _BackgroundSocketWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _ = await ShowDialog(Dialogtype.Error, e.Error.Message);
            }
        }
        #endregion

        private string _ProcessReading()
        {
            if (string.IsNullOrWhiteSpace(LastReceivedValue))
            {
                return string.Empty;
            }


            if (ListeningOnSerialPort == false)
            {
                return string.Empty;
            }
            if (StabilityIndicatorActive)
            {
                if (LastReceivedValue.Contains(StabilityIndicatorSnippet) == false)
                {
                    return _invalidValue;
                }

                string refSnippet = LastReceivedValue.Substring(StabilityIndicatorStartingPosition - 1, StabilityIndicatorSnippet.Length);
                if (refSnippet != StabilityIndicatorSnippet)
                {
                    return _invalidValue;
                }
            }
            else if (SequenceOfIdenticalReadingsActive)
            {
                if (NumberOfIdenticalReadings > ReceivedValues.Count)
                {
                    return _invalidValue;
                }
                else if (string.IsNullOrWhiteSpace(LastReceivedValue))
                {
                    return _invalidValue;
                }
                else
                {
                    string identicalReadingToLookFor = LastReceivedValue;
                    for (int i = 0; i < NumberOfIdenticalReadings; i++)
                    {
                        if (ReceivedValues[i] == identicalReadingToLookFor)
                        {
                            continue;
                        }
                        else
                        {
                            return _invalidValue;
                        }
                    }
                }
            }
            if (WeightStartPosition >= 1 && WeightStartPosition > WeightEndPosition)
            {
                return _invalidValue;
            }
            else if (WeightStartPosition > LastReceivedValue.Length)
            {
                return _invalidValue;
            }
            else if (StringRequiredLengthActive && ScaleStringRequiredLength > 0 && LastReceivedValue.Length != ScaleStringRequiredLength)
            {
                return _invalidValue;
            }
            else
            {
                string userSpecifiedString = LastReceivedValue.Length >= WeightEndPosition
                    ? LastReceivedValue[(WeightStartPosition - 1)..WeightEndPosition]
                    : LastReceivedValue[(WeightStartPosition - 1)..LastReceivedValue.Length];

                return userSpecifiedString.Any(char.IsLetter)
                    ? $"{userSpecifiedString} ({new string(userSpecifiedString.Where(i => char.IsNumber(i) || i == '.' || i == ',').ToArray())})"
                    : userSpecifiedString;
            }
        }

        private string _ProcessedValue()
        {
            if (LastProcessedValue == _invalidValue)
            {
                return _invalidValue;
            }

            string sendVal = LastProcessedValue;

            //We check if value has a comma
            bool usesComma = sendVal.Contains(',');

            //If value has a comma, we replace all dots with nulls
            sendVal = usesComma ? sendVal.Replace(".", null)
                : sendVal.Replace(",", null);

            //If more than one comma or dot is found, then we return "NAN"
            if (usesComma && sendVal.Count(i => i == ',') > 1)
            {
                sendVal = _invalidValue;
            }
            else if (usesComma == false && sendVal.Count(i => i == '.') > 1)
            {
                sendVal = _invalidValue;
            }

            //If value is not "NAN" and is parsable as float, we then send value to socket
            return sendVal != _invalidValue && float.TryParse(
                sendVal,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out float result)
                ? sendVal
                : _invalidValue;
        }

        private void _ValueUpdatedCallback(string value)
        {
            LastReceivedValue = value;

            if (BroadcastingSerialValues
                && SocketTools.SocketAvailable
                && _backgroundSocketWorker.CancellationPending == false)
            {
                _backgroundSocketWorker.RunWorkerAsync();
            }
        }
        #endregion

        #region Tasks
        private async Task _ThreadExceptionCallback(CustomException exception)
        {
            LoggingService.WriteLog(LogType.FarfulcrumException, exception.Message);
            ListeningOnSerialPort = false;
            BroadcastingSerialValues = false;
            _ = await ShowDialog(Dialogtype.Error, exception.Message);
        }
        #endregion
    }
}

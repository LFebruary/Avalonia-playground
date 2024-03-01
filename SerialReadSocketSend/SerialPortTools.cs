// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Avalonia.Threading;
using Playground.Constants;
using System;
using System.IO.Ports;
using System.Threading.Tasks;
using static Playground.Constants.SerialConstants;
using static Playground.CustomSettings;

namespace Playground.SerialReadSocketSend
{
    public static partial class SerialPortTools
    {
        #region Computed properties
        /// <summary>
        /// Property that determines if SerialPort is opened
        /// </summary>
        internal static bool IsPortOpen => _serialPort?.IsOpen == true;

        /// <summary>
        /// Baud rate to use with SerialPort
        /// </summary>
        internal static int LastUsedBaudRate => GetSetting(IntSetting.BaudRate);

        /// <summary>
        /// Data bits to use with SerialPort
        /// </summary>
        internal static int LastUsedDataBits => GetSetting(IntSetting.Databits);

        /// <summary>
        /// Parity to use with SerialPort
        /// </summary>
        private static Parity? LastUsedParity => GetSetting(StringSetting.Parity) switch
        {
            NoParity => Parity.None,
            EvenParity => Parity.Even,
            OddParity => Parity.Odd,
            _ => null,
        };

        /// <summary>
        /// Port ID to use for SerialPort connection
        /// </summary>
        internal static string LastUsedPortID => GetSetting(StringSetting.ComPort);

        /// <summary>
        /// Stop bits to use for SerialPort connection
        /// </summary>
        internal static StopBits? LastUsedStopBits => GetSetting(IntSetting.Stopbits) switch
        {
            1 => System.IO.Ports.StopBits.One,
            2 => System.IO.Ports.StopBits.Two,
            _ => null,
        };
        #endregion

        #region Read-write properties
        private static string? _lastSerialReading;

        /// <summary>
        /// Last reading retrieved from the SerialPort
        /// </summary>
        internal static string LastSerialReading
        {
            get => _lastSerialReading ?? "";
            set
            {
                _lastSerialReading = value;
                LoggingService._comPortReadingsBetweenDump++;
                ValueUpdatedCallback?.Invoke(_lastSerialReading);
            }
        }

        /// <summary>
        /// Callback to be invoked if an expected exception is thrown
        /// </summary>
        public static Func<CustomException, Task> ThreadExceptionCallback { get; set; }
            = async (_) => await Task.Run(() => { });
        #endregion

        #region Fields
        /// <summary>
        /// Flag that indicates whether should be read from the SerialPort or not
        /// </summary>
        private static bool _readSwitch;
#nullable enable
        /// <summary>
        /// SerialPort instance to be used throughout program
        /// </summary>
        private static SerialPort? _serialPort = null;
#nullable restore

        private static Action<string>? _valueUpdatedCallback = (_) => { };
        /// <summary>
        /// Callback to be invoked every time the value from the SerialPort is updated
        /// </summary>
        public static Action<string>? ValueUpdatedCallback
        {
            get => _valueUpdatedCallback;
            set
            {
                if (_valueUpdatedCallback != value)
                {
                    _valueUpdatedCallback = value;
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// This gets the port id and last saved settings in CustomSettings to instantiate a SerialPort and start reading from it
        /// </summary>
        public static void GetPortAndStartListening()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                throw new CustomException(ErrorMessageConstants.SerialPortAlreadyActive);
            }
            else if (LastUsedParity != null && LastUsedParity is Parity castedParity && LastUsedStopBits != null && LastUsedStopBits is StopBits castedStopBits)
            {
                _serialPort = new SerialPort(LastUsedPortID, LastUsedBaudRate, castedParity, LastUsedDataBits, castedStopBits)
                {
                    ReadTimeout = GetSetting(IntSetting.SerialTimeoutMs)
                };

                _readSwitch = true;
            }
            else if (LastUsedParity is null or not Parity)
            {
                throw new CustomException(ErrorMessageConstants.InvalidParity);
            }
            else if (LastUsedStopBits is null or not System.IO.Ports.StopBits)
            {
                throw new CustomException(ErrorMessageConstants.InvalidStopbits);
            }

            if (_serialPort != null)
            {

                if (_serialPort.IsOpen)
                {
                    throw new CustomException(ErrorMessageConstants.SerialPortAlreadyOpen);
                }
                try
                {
                    _serialPort.Open();
                    if (IsPortOpen == false)
                    {
                        throw new CustomException($"{ErrorMessageConstants.CouldNotConnectToSerialPort}{_serialPort.PortName}");
                    }

                    _StartReadingSerialPortWithTimeout();

                }
                catch (UnauthorizedAccessException e)
                {
                    LoggingService.WriteLog(LogType.CaughtException, e.Message);
                    throw new CustomException(ErrorMessageConstants.SerialPortInUseByAnotherProcess, e);
                }
            }
        }

        /// <summary>
        /// This takes a FlowControl enumeration and sets it appropriately on the created SerialPort instance.
        /// </summary>
        /// <param name="selectedFlowControl">Flow control that should be configured on the SerialPort</param>
        internal static void SetFlowControl(FlowControl? selectedFlowControl)
        {
            if (_serialPort == null)
            {
                return;
            }

            switch (selectedFlowControl)
            {
                case FlowControl.Ctr_Rts:
                    _serialPort.Handshake = Handshake.RequestToSend;
                    _serialPort.DtrEnable = false;
                    break;
                case FlowControl.Dsr_Dtr:
                    _serialPort.Handshake = Handshake.None;
                    _serialPort.DtrEnable = true;
                    break;
                case FlowControl.Xon_Xoff:
                    _serialPort.Handshake = Handshake.XOnXOff;
                    _serialPort.DtrEnable = false;
                    break;
                case FlowControl.None:
                    _serialPort.Handshake = Handshake.None;
                    _serialPort.DtrEnable = false;
                    break;
                default:
                    _serialPort.Handshake = Handshake.None;
                    _serialPort.DtrEnable = false;
                    break;
            }
        }

        /// <summary>
        /// This method starts reading from created serial port and sets the static last read value on every read
        /// </summary>
        private static async void _StartReadingSerialPortWithTimeout()
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _ = Task.Run(async () =>
                  {
                      while (_readSwitch)
                      {
                          try
                          {
                              if (_serialPort == null)
                              {
                                  await Dispatcher.UIThread.InvokeAsync(() => ThreadExceptionCallback.Invoke(new CustomException(ErrorMessageConstants.SerialPortNeverInstantiated)));
                              }
                              else
                              {
                                  string message = _serialPort.ReadLine();

                                  LastSerialReading = message;

                                  _serialPort.DiscardInBuffer();
                              }

                          }
                          catch (TimeoutException timeoutException)
                          {
                              LoggingService.WriteLog(LogType.CaughtException, timeoutException.Message);
                              await Dispatcher.UIThread.InvokeAsync(() => ThreadExceptionCallback.Invoke(new CustomException(ErrorMessageConstants.SerialPortTimeout, timeoutException)));
                          }
                      }
                  });
            });
        }

        /// <summary>
        /// This stops listening on SerialPort in a forceful manner, by setting the SerialPort to null and toggling read flag
        /// </summary>
        internal static void StopListeningForcefully()
        {
            _serialPort?.Close();
            _serialPort = null;
            LoggingService.WriteLog(LogType.ComPortStop, GetSetting(StringSetting.ComPort));

            _readSwitch = false;
        }

        /// <summary>
        /// This stops listening on SerialPort by merely closing the SerialPort and toggling read flag
        /// </summary>
        internal static void StopListeningOnPort()
        {
            if (_serialPort?.IsOpen == true)
            {
                _serialPort.Close();
                LoggingService.WriteLog(LogType.ComPortStop, GetSetting(StringSetting.ComPort));
            }

            _readSwitch = false;
        }
        #endregion
    }
}

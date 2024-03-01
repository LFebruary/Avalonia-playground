// AvaloniaPlayground https://github.com/LFebruary/Avalonia-playground 
// (c) 2024 Lyle February 
// Released under the MIT License

using Playground.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Playground.SerialReadSocketSend
{
    internal static partial class SocketTools
    {
        #region Computed properties
        /// <summary>
        /// IPAddress on which the socket will listen <br/>
        /// This method first tries to get an IPAddress for the ethernet connection, if not then a wireless lan connection
        /// </summary>
        private static IPAddress EndPointIPAddress => System.Net.IPAddress.Parse(_GetLocalIPV4Addresses(NetworkInterfaceType.Ethernet).FirstOrDefault()
            ?? _GetLocalIPV4Addresses(NetworkInterfaceType.Wireless80211).FirstOrDefault()
            ?? throw new CustomException(ErrorMessageConstants.CouldNotDetermineLocalIPAddress));

        /// <summary>
        /// This merely converts the EndPoint IP address to a string for display on main window
        /// </summary>
        internal static string IPAddress => EndPointIPAddress.ToString();

        /// <summary>
        /// Port on which the values will be broadcasted
        /// </summary>
        internal static int Port => CustomSettings.GetSetting(CustomSettings.IntSetting.BroadcastPort);

        /// <summary>
        /// This checks to see if the listening socket has been instantiated and is open to send values over
        /// </summary>
        internal static bool SocketAvailable => _listener is not null && _listeningFlag;
        #endregion

        #region Fields
        /// <summary>
        /// Socket that is being lisstened on
        /// </summary>
        private static Socket? _listener;

        /// <summary>
        /// Flag that indicates whether any values are being transmitted over the socket
        /// </summary>
        private static bool _listeningFlag = false;
        #endregion

        #region Methods
        /// <summary>
        /// Callback to invoke every time a client socket connection is accepted
        /// </summary>
        /// <param name="asyncResult">Current listening socket and string value to transmit wrapped in an AsyncResult</param>
        private static void _DoAcceptSocketCallback(IAsyncResult asyncResult)
        {
            if (asyncResult.AsyncState is null || SocketAvailable == false)
            {
                return;
            }

            (Socket? listener, string? stringValue) = ((Socket?, string?))asyncResult.AsyncState;

            if (listener is null || stringValue is null)
            {
                return;
            }

            try
            {
                using Socket handler = listener.EndAccept(asyncResult);

                LoggingService.WriteLog(LogType.SocketConnectionEstablished, handler.RemoteEndPoint?.ToString());
                LoggingService._socketConnectionsBetweenLastDump++;

                byte[] msg = Encoding.ASCII.GetBytes(stringValue);

                if (handler is not null && handler.Connected)
                {
                    _ = handler.Send(msg);

                    handler.Shutdown(SocketShutdown.Receive);

                    handler.Close();
                }
            }
            catch (ObjectDisposedException objectDisposedException)
            {
                LoggingService.WriteLog(LogType.CaughtException, objectDisposedException.Message);
                return;
            }
        }

        /// <summary>
        /// Returns a list of IPV4 addresses associated with device program is running on
        /// </summary>
        /// <param name="interfaceType">Specific interface to find addresses for</param>
        /// <returns>List of string ip addresses for specified interface</returns>
        private static List<string> _GetLocalIPV4Addresses(NetworkInterfaceType interfaceType)
        {
            List<string> ipAddressList = [];
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ((item.Name.Contains("Virtual") == false) && item.NetworkInterfaceType == interfaceType && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddressList.Add(ip.Address.ToString());
                        }
                    }
                }
            }

            return ipAddressList;
        }

        /// <summary>
        /// Transmits provided string value over socket to any attached client
        /// </summary>
        /// <param name="transmitValue">String to send to any client connected to listening socket</param>
        internal static void SendData(string transmitValue)
        {
            if (_listener is null || _listener.LocalEndPoint is null)
            {
                throw new CustomException(ErrorMessageConstants.SocketListenerNotInstantiated);
            }

            if (transmitValue is null || _listener is null)
            {
                return;
            }

            if (transmitValue is string stringValue)
            {
                try
                {
                    _ = _listener.BeginAccept(new AsyncCallback(_DoAcceptSocketCallback), (_listener, stringValue));
                }
                catch (SocketException ex)
                {
                    LoggingService.WriteLog(LogType.CaughtException, ex.Message);
                    throw new CustomException(ErrorMessageConstants.SocketCouldNotSendData, ex);
                }
            }
        }

        /// <summary>
        /// This instantiates the listening socket, binds it to an appropriate endpoint, sets backlog and listening flag
        /// </summary>
        internal static void StartServer()
        {
            try
            {
                _listener = new(EndPointIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                LoggingService.WriteLog(LogType.SocketCreate, $"{IPAddress}:{Port}");

                _listener.Bind(new IPEndPoint(EndPointIPAddress, Port));

                _listener.Listen(20);

                _listeningFlag = true;
            }
            catch (SocketException exception)
            {
                LoggingService.WriteLog(LogType.CaughtException, exception.Message);
                throw new CustomException(ErrorMessageConstants.SocketListenerNotStarted, exception);
            }
        }

        /// <summary>
        /// This closes the socket and stops any connections from being made to the listening socket
        /// </summary>
        internal static void StopServer()
        {
            if (_listener is not null)
            {
                LoggingService.WriteLog(LogType.SocketStop, $"{IPAddress}:{Port}");
                _listeningFlag = false;
                _listener.Close();
                _listener = null;
            }
        }
        #endregion
    }
}

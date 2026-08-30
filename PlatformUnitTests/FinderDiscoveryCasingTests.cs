using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xunit;

namespace PlatformUnitTests
{
    /// <summary>
    /// Tests that AlpacaDiscovery.Finder accepts UDP discovery responses whose JSON key is spelled
    /// with any casing of "AlpacaPort" and returns the correct port number.
    ///
    /// Each test starts an in-process UDP responder on a dynamically allocated port, triggers
    /// a discovery cycle, and asserts that the expected Alpaca port is returned.
    /// </summary>
    public class FinderDiscoveryCasingTests
    {
        private const int EXPECTED_ALPACA_PORT = 12345;
        private const double DISCOVERY_DURATION_SECONDS = 2.0;

        private readonly ITestOutputHelper output;

        public FinderDiscoveryCasingTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // -----------------------------------------------------------------
        // Theory: the same port value must be found regardless of the casing
        // used for the JSON key name.
        // -----------------------------------------------------------------

        [Theory]
        [InlineData("AlpacaPort",  EXPECTED_ALPACA_PORT)]
        [InlineData("alpacaport",  EXPECTED_ALPACA_PORT)]
        [InlineData("ALPACAPORT",  EXPECTED_ALPACA_PORT)]
        [InlineData("aLPACApORT",  EXPECTED_ALPACA_PORT)]
        public void AlpacaPort_JsonKeyCasing_DeviceDiscoveredWithCorrectPort(string keyName, int expectedPort)
        {
            string jsonResponse = $@"{{  ""{keyName}"": {expectedPort}  }}";
            output.WriteLine($"JSON response under test: {jsonResponse}");

            int discoveryPort = AllocateFreeUdpPort();
            output.WriteLine($"Using discovery port: {discoveryPort}");

            using (var responder = new UdpAlpacaResponder(discoveryPort, jsonResponse, output))
            {
                responder.Start();
                Thread.Sleep(100); // Allow responder to bind before the broadcast is sent

                List<AlpacaDevice> devices;
                using (var discovery = new AlpacaDiscovery())
                {
                    discovery.StartDiscovery(
                        numberOfPolls: 1,
                        pollInterval: 100,
                        discoveryPort: discoveryPort,
                        discoveryDuration: DISCOVERY_DURATION_SECONDS,
                        resolveDnsName: false,
                        useIpV4: true,
                        useIpV6: false);

                    // Wait for the discovery period plus a safety margin
                    var deadline = DateTime.UtcNow.AddSeconds(DISCOVERY_DURATION_SECONDS + 0.2);
                    while (!discovery.DiscoveryComplete && DateTime.UtcNow < deadline)
                        Thread.Sleep(50);

                    devices = discovery.GetAlpacaDevices();
                }

                output.WriteLine($"Discovered {devices.Count} Alpaca device(s):");
                foreach (AlpacaDevice d in devices)
                    output.WriteLine($"  {d.IpAddress}:{d.Port}");

                Assert.NotEmpty(devices);
                Assert.Contains(devices, d => d.Port == expectedPort);
            }
        }

        // -----------------------------------------------------------------
        // Helper: find a free UDP port by binding to port 0 then releasing it.
        // -----------------------------------------------------------------
        private static int AllocateFreeUdpPort()
        {
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                sock.Bind(new IPEndPoint(IPAddress.Any, 0));
                return ((IPEndPoint)sock.LocalEndPoint).Port;
            }
        }
    }

    // =====================================================================
    // Simulated Alpaca device responder
    // =====================================================================

    /// <summary>
    /// A minimal UDP server that simulates an Alpaca device.
    /// Listens on <paramref name="port"/> for an Alpaca discovery broadcast,
    /// then replies to the sender with <paramref name="jsonResponse"/>.
    /// </summary>
    internal sealed class UdpAlpacaResponder : IDisposable
    {
        // The Finder sends "alpacadiscovery1"; we match on the common prefix.
        private const string DISCOVERY_MESSAGE_PREFIX = "alpacadiscovery";

        private readonly int port;
        private readonly string jsonResponse;
        private readonly ITestOutputHelper output;

        private UdpClient udpClient;
        private Thread thread;
        private volatile bool running;

        public UdpAlpacaResponder(int port, string jsonResponse, ITestOutputHelper output)
        {
            this.port = port;
            this.jsonResponse = jsonResponse;
            this.output = output;
        }

        /// <summary>Binds the socket and starts the background listener thread.</summary>
        public void Start()
        {
            udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            // Short receive timeout so the loop checks the running flag regularly.
            udpClient.Client.ReceiveTimeout = 500;

            running = true;
            thread = new Thread(ListenLoop) { IsBackground = true, Name = $"UdpAlpacaResponder:{port}" };
            thread.Start();
        }

        /// <summary>Signals the listener to stop and waits for it to exit.</summary>
        public void Stop()
        {
            running = false;
            try { udpClient?.Close(); } catch { }
            thread?.Join(2000);
        }

        public void Dispose() => Stop();

        // ------------------------------------------------------------------

        private void ListenLoop()
        {
            byte[] responseBytes = Encoding.ASCII.GetBytes(jsonResponse);

            while (running)
            {
                try
                {
                    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = udpClient.Receive(ref remoteEndPoint);
                    string message = Encoding.ASCII.GetString(data);
                    output.WriteLine($"UdpAlpacaResponder [{port}] received '{message}' from {remoteEndPoint}");

                    if (message.ToLowerInvariant().Contains(DISCOVERY_MESSAGE_PREFIX))
                    {
                        output.WriteLine($"UdpAlpacaResponder [{port}] sending '{jsonResponse}' to {remoteEndPoint}");
                        udpClient.Send(responseBytes, responseBytes.Length, remoteEndPoint);
                    }
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    // Normal receive timeout — loop and check running flag.
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted ||
                                                  ex.SocketErrorCode == SocketError.OperationAborted)
                {
                    break; // Socket was closed externally.
                }
                catch (ObjectDisposedException)
                {
                    break; // Socket was disposed.
                }
            }

            output.WriteLine($"UdpAlpacaResponder [{port}] stopped.");
        }
    }
}

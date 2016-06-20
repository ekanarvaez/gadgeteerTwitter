using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace GadgeteerTwitter
{
    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

    public class SocketServer
    {
        public const int DEFAULT_SERVER_PORT = 80;
        private Socket socket;
        private int port;
        private IPAddress ip;
        public event DataReceivedEventHandler DataReceived;

        public SocketServer()
            : this(DEFAULT_SERVER_PORT)
        { }

        public SocketServer(byte[] ip, int port)
            : this(port)
        {
            this.ip = new IPAddress(ip);
        }

        public SocketServer(int port)
        {
            this.port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public IPAddress IpServer
        {
            get
            {
                if (this.ip == null)
                {
                    this.ip = IPAddress.Any;
                }
                return this.ip;
            }
            set
            {
                this.ip = value;
            }
        }
        public void Start()
        {
            IPEndPoint localEndPoint = new IPEndPoint(this.IpServer, port);
            socket.Bind(localEndPoint);
            socket.Listen(Int32.MaxValue);

            new Thread(StartServerInternal).Start();
        }

        private void StartServerInternal()
        {
            while (true)
            {
                // Wait for a request from a client.
                Socket clientSocket = socket.Accept();

                // Process the client request.
                var request = new ProcessClientRequest(this, clientSocket);
                request.Process();
            }
        }

        private void OnDataReceived(DataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(this, e);
        }

        private class ProcessClientRequest
        {
            private Socket clientSocket;
            private SocketServer socket;

            public ProcessClientRequest(SocketServer socket, Socket clientSocket)
            {
                this.socket = socket;
                this.clientSocket = clientSocket;
            }

            public void Process()
            {
                // Handle the request in a new thread.
                new Thread(ProcessRequest).Start();
            }

            private void ProcessRequest()
            {
                const int c_microsecondsPerSecond = 1000000;

                using (clientSocket)
                {
                    while (true)
                    {
                        try
                        {
                            if (clientSocket.Poll(5 * c_microsecondsPerSecond, SelectMode.SelectRead))
                            {
                                // If the butter is zero-lenght, the connection has been closed or terminated.
                                if (clientSocket.Available == 0)
                                    break;

                                byte[] buffer = new byte[clientSocket.Available];
                                int bytesRead = clientSocket.Receive(buffer, clientSocket.Available, SocketFlags.None);

                                byte[] data = new byte[bytesRead];
                                buffer.CopyTo(data, 0);

                                DataReceivedEventArgs args = new DataReceivedEventArgs(clientSocket.LocalEndPoint, clientSocket.RemoteEndPoint, data);
                                socket.OnDataReceived(args);

                                if (args.ResponseData != null)
                                    clientSocket.Send(args.ResponseData);

                                if (args.Close)
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}

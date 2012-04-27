using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;

namespace TCWO_CSLibrary.Network
{
    public abstract class Server<T> where T : Connection, new()
    {
        Socket serverSocket;

        public Server(string listenAddress, int listenPort)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(listenAddress), listenPort));
        }

        public void Start()
        {
            serverSocket.Listen(100);
            StartAccept();
        }

        public void Stop()
        {
            serverSocket.Close(10000);
        }

        void StartAccept()
        {
            T connection = new T();
            serverSocket.BeginAccept(new AsyncCallback(EndAccept), connection);
        }

        void EndAccept(IAsyncResult ar)
        {
            Socket client = serverSocket.EndAccept(ar);
            T connection = (T)ar.AsyncState;
            connection.Server = true;
            connection.Client = client;
            connection.Engage();
            StartAccept();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCWO_CSLibrary.Network
{
    public abstract class Connection
    {
        public bool Server;
        
        // Salsa20 Layer
        Salsa20 salsa;
        ICryptoTransform salsaEncryptor;
        ICryptoTransform salsaDecryptor;

        public Connection()
        {
            salsa = new Salsa20();
            salsa.KeySize = 128;
            salsa.GenerateKey();
            salsa.GenerateIV();
            salsaEncryptor = salsa.CreateEncryptor();
        }

        public void Connect(string ip, int port)
        {
            Server = false;
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Client.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(ar => 
                {
                    Engage();
                }), null);
        }

        // Child Implementations
        protected abstract void onConnect();

        protected abstract void log(string message);

        public Socket Client { get; set; }

        byte[] ReadNextBlock()
        {
            byte[] block = new byte[8192];
            int recv = Client.Receive(block, 0, 8192, SocketFlags.None);
            Array.Resize<byte>(ref block, recv);
            return block;
        }

        public void Engage()
        {
            try
            {
                if (Server)
                {
                    // Send my salsa key
                    byte[] my_salsa = new byte[24];
                    Array.Copy(salsa.Key, 0, my_salsa, 0, 16);
                    Array.Copy(salsa.IV, 0, my_salsa, 16, 8);
                    Client.Send(my_salsa);
                    log("Sent MySalsa.");
                    // Wait for client key
                    byte[] remote_salsa = ReadNextBlock();
                    Salsa20 salsa2 = new Salsa20();
                    salsa2.Key = new byte[16];
                    salsa2.IV = new byte[8];
                    Array.Copy(remote_salsa, 0, salsa2.Key, 0, 16);
                    Array.Copy(remote_salsa, 16, salsa2.IV, 0, 8);
                    salsaDecryptor = salsa2.CreateDecryptor();
                    log("Received RemoteSalsa.");
                    onConnect();
                }
                else
                {
                    // Wait for server salsa
                    byte[] remote_salsa = ReadNextBlock();
                    Salsa20 salsa2 = new Salsa20();
                    salsa2.Key = new byte[16];
                    salsa2.IV = new byte[8];
                    Array.Copy(remote_salsa, 0, salsa2.Key, 0, 16);
                    Array.Copy(remote_salsa, 16, salsa2.IV, 0, 8);
                    salsaDecryptor = salsa2.CreateDecryptor();
                    log("Received RemoteSalsa.");
                    // Send my salsa encrypted
                    byte[] my_salsa = new byte[24];
                    Array.Copy(salsa.Key, 0, my_salsa, 0, 16);
                    Array.Copy(salsa.IV, 0, my_salsa, 16, 8);
                    Client.Send(my_salsa);
                    log("Sent MySalsa.");
                    onConnect();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR during handshake: " + e.Message + "\n" + e.StackTrace);
            }
        }
    }
}

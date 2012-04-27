using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCWO_CSLibrary.Network;

namespace TCWO_WorldServer
{
    public class WorldServer : Server<WorldClientConnection>
    {
        public WorldServer() : base("127.0.0.1", 10501)
        {
            Console.WriteLine("World Server created.");
        }
    }

    public class WorldClientConnection : Connection
    {
        protected override void onConnect()
        {
            Console.WriteLine("Connection established.");
        }

        protected override void log(string message)
        {
            Console.WriteLine(message);
        }
    }
}

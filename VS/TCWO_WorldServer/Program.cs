using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCWO_WorldServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The Clone Wars Online - World Server");
            Console.WriteLine("");

            new WorldServer().Start();

            while (true) Console.ReadLine();
        }
    }
}

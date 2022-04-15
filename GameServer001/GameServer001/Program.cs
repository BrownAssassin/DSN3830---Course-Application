using System;
using System.Collections.Generic;
using System.Text;
using GameServer001.Servers;

namespace GameServer001
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 6688);
            server.Start();

            Console.ReadKey();
        }
    }
}

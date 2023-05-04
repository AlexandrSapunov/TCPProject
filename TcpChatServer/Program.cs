using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpChatServer
{

    class Program
    {
        static void Main(string[] args)
        {
            Server.Server server = new Server.Server("localhost",8080);
            server.Start();

            while (true)
            {
                Console.WriteLine("enter message:");
                string msg = Console.ReadLine();
                server.SendMessage(msg);
            }

        }
    }
}

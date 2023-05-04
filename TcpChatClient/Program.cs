using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter name");
            string name = Console.ReadLine();
            Console.WriteLine($"Enter ip");
            string ip = Console.ReadLine();
            int port = 8080;
            try
            {
                Client.Client client = new Client.Client(ip,port,name);
                client.Connect();

                while (true)
                {
                    Console.WriteLine("Enter cmd");
                    string cmd = Console.ReadLine();
                    client.SendMessage(cmd);

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Не удалось подключиться:{ex.Message}");
            }
        }
    }
}

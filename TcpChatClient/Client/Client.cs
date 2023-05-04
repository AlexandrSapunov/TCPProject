using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpChatClient.Client
{
    public class Client
    {
        public ClientInfo user = new ClientInfo();
        TcpClient client;
        NetworkStream stream;
        Thread thread;


        public Client(string ip, int port, string name)
        {
            user.Ip = ip;
            user.Port = port;
            user.Name = name;
            client = new TcpClient();
            thread = new Thread(GetMessageClient);
        }

        public void Connect()
        {
            client.Connect(user.Ip, user.Port);
            stream = client.GetStream();
            thread.Start();
            SendMessage(user.Name);
        }

        public void Connect(string ip,int port)
        {
            client.Connect(ip, port);
            stream = client.GetStream();
            thread.Start();
            SendMessage(user.Name);
        }

        public void CloseConnect()
        {
            client.Close();
        }

        public void SendMessage(string message)
        {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
        }

        private void GetMessageClient()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes);
                    Console.WriteLine(msg);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}

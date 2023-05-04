using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpChatServer.Server
{
    public class Client
    {
        public string Ip { get; set; }//ip адрес
        public string Name { get; set; }//имя пользователя
        private TcpClient connection; 
        private NetworkStream stream; //поток получения данных от сервера
        private Thread thread;

        public bool IsEnable { get; private set; }

        public Client(TcpClient tcp)
        {
            IsEnable = true;
            connection = tcp;
            Ip = ((IPEndPoint)tcp.Client.RemoteEndPoint).Address.ToString();
            stream = tcp.GetStream();
            thread = new Thread(LoopReciveMessage);
            thread.Start();
        }

        private void LoopReciveMessage()
        {
            Name = GetName();
            Server.Clients.Add(this);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{Name} joined the chat!");
            Console.ResetColor();

            while (IsEnable)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    if (bytes == 0)
                        continue;
                    string message = Encoding.UTF8.GetString(buffer, 0, bytes);
                    Server.SendMessage(message,Name);
                    ShowMessage(message);
                }
                catch(Exception ex)
                {

                    if (IsEnable)
                        Disconnect();
                }
            }
        }
        public void SendMessage(Client author,string message)
        {
            Send($"{author.Name}:{message}");
        }

        public void SendMessage(string serverName, string message)
        {
            Send($"{serverName}: {message}");
        }

        public void Disconnect(string text = null)
        {
            if (text != null)
                Send(text);

            IsEnable = false;
            Console.WriteLine($"{Name} disconnect. Reason: {text}");
            
            connection.Close();
            connection.Dispose();
        }
        private void ShowMessage(string msg)
        {
            Console.WriteLine($"{Name}:{msg}");
        }
        private string GetName()
        {
            byte[] bufferName = new byte[1024];
            int bytesName = stream.Read(bufferName, 0, bufferName.Length);
            string name = Encoding.UTF8.GetString(bufferName, 0, bytesName);

            return name;
        }
        private void Send(string text)
        {
            try
            {
                byte[] bufferSend = Encoding.UTF8.GetBytes(text);
                stream.Write(bufferSend, 0, bufferSend.Length);
                stream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

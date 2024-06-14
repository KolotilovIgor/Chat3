using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Chat3;

namespace Client
{
    public class Program
    {
        public static event Action<string> MessageSent;

        static void Main(string[] args)
        {
            MessageSent += ConfirmMessageSent;
            SentMessage(args[0], args[1]);
        }

        public static void ConfirmMessageSent(string message)
        {
            Console.WriteLine($"Подтверждение: сообщение '{message}' отправлено.");
        }

        public static void SentMessage(string From, string ip)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            string messageText = "привет";

            Message message = new Message() { Text = messageText, NicknameFrom = From, NicknameTo = "Server", DateTime = DateTime.Now };
            string json = message.SerializeMessageToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, iPEndPoint);
            MessageSent?.Invoke(messageText);

            Console.WriteLine("Введите 'Exit' для выхода.");
            if (Console.ReadLine().ToLower() == "exit")
            {
                Environment.Exit(0);
            }
        }
    }
}

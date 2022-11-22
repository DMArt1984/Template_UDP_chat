using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDP_chat
{
    class Program
    {
        static string remoteAddress = "127.0.0.1"; // хост для отправки данных
        static int remotePort = 2001; // порт для отправки данных
        static int localPort = 2002; // локальный порт для прослушивания входящих подключений

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите локальный порт для прослушивания: "); // локальный порт
                localPort = Int32.Parse(Console.ReadLine());

                Console.Write("Введите удаленный адрес для подключения: ");
                remoteAddress = Console.ReadLine(); // адрес, к которому мы подключаемся

                Console.Write("Введите удаленный порт для подключения: ");
                remotePort = Int32.Parse(Console.ReadLine()); // порт, к которому мы подключаемся

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                SendMessage(); // отправка сообщений
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // что-то пошло не так!
            }
        }
        private static void SendMessage()
        {
            UdpClient sender = new UdpClient(); // создаем клиента для отправки сообщений
            try
            {
                while (true)
                {
                    Console.Write("Вот, что я скажу: ");
                    string message = Console.ReadLine(); // сообщение для отправки
                    byte[] data = Encoding.Unicode.GetBytes(message); // сообщение в байты
                    sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // что-то пошло не так!
            }
            finally
            {
                sender.Close();
            }
        }

        private static void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine("Некто на другой стороне: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // что-то пошло не так!
            }
            finally
            {
                receiver.Close();
            }
        }
    }

}

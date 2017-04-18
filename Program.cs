using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace chat
{
  class Program
    {
        const int MESSAGE_SIZE = 1024;
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }
        static async Task MainAsync()
        {
            var uri = new Uri("ws://localhost:9000/", UriKind.Absolute);
            
            var socket = new ClientWebSocket();
            await socket.ConnectAsync(uri, CancellationToken.None);

            await Task.WhenAll(ReceiveMessages(socket), SendMessages(socket));
        }

        static async Task SendMessages(ClientWebSocket socket)
        {
            while(true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                await SendMessage(socket, input);
            }
        }

        static async Task SendMessage(ClientWebSocket socket, String message)
        {
            await socket.SendAsync(
                new ArraySegment<Byte>(Encoding.UTF8.GetBytes(message)),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }

        static async Task ReceiveMessages(ClientWebSocket socket)
        {
            while(true)
            {
                ArraySegment<Byte> buffer = new ArraySegment<Byte>(new Byte[MESSAGE_SIZE]);
                await socket.ReceiveAsync(buffer, CancellationToken.None);

                Console.WriteLine(TrimResponse(buffer));
            }
        }

        static String TrimResponse(ArraySegment<Byte> buffer)
        {
            var lastByte = MESSAGE_SIZE - 1;
            while(buffer.Array[lastByte - 1] == 0)
                lastByte--;
            var messageArray = new Byte[lastByte];
            Array.Copy(buffer.Array, messageArray, lastByte);

            var message = Encoding.UTF8.GetString(messageArray);

            return message;
        }

    }
}

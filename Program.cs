using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => Try());
        }
        // static async Task MainAsync()
        // {
        //     Console.WriteAscii("Hello World!");
        //     var socket = new ClientWebSocket();
        //     var uri = new Uri("ws://localhost:9000");

        //     await socket.ConnectAsync(uri, CancellationToken.None);

        //     var message = "This is a message";

        //     var buffer = new ArraySegment<Byte>(Encoding.UTF8.GetBytes(message));

        //     return await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        // }

        static async Task<String> Try()
        {
            using (HttpClient client = new HttpClient())
                return await client.GetStringAsync("http://localhost:9000");
        }
    }
}

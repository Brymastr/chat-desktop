using System;
using System.Threading.Tasks;

namespace chat
{
    class Program
    {
        // private const string SERVER = "ws://localhost:10000";
        // private const string SERVER = "ws://45.63.34.98:10000";
        private const string SERVER = "ws://chat.dorsaydevelopment.ca:10000";
        static Client client;

        static void Main(string[] args)
        {
            client = new Client();

            AsyncMain().Wait();
        }

        static async Task AsyncMain()
        {
            await client.Connect(new Uri(SERVER));
        }

    }
}

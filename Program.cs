using System;
using System.Threading.Tasks;

namespace chat
{
    class Program
    {
        static Client client;

        static void Main(string[] args)
        {
            client = new Client();

            AsyncMain().Wait();
        }

        static async Task AsyncMain()
        {
            await client.Connect(new Uri("ws://localhost:10000"));
        }

    }
}

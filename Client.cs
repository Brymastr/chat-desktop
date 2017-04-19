using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Client
{
    private ClientWebSocket socket;
    static private Guid clientId;
    public Uri serverUri {get; set;}
    const int MESSAGE_SIZE = 1024;
    private string username;


    public Client()
    {
        clientId = Guid.NewGuid();
        socket = new ClientWebSocket();
    }

    public async Task Connect(Uri uri)
    {
        Console.Write("Name: ");
        username = Console.ReadLine();

        serverUri = uri;
        await socket.ConnectAsync(uri, CancellationToken.None);

        await SendMessage(username + " connected");
        
        await Task.WhenAll(ReceiveMessages(), SendMessages());
    }

    private async Task SendMessages()
    {
        while(true)
        {
            var input = SerializeMessage(Console.ReadLine());
            await SendMessage(input);
        }
    }

    private string SerializeMessage(string input)
    {
        var message = new Message 
        {
            name = username,
            id = clientId,
            text = input
        };

        return JsonConvert.SerializeObject(message);
    }

    private Message DeserializeMessage(string jsonInput)
    {
        try 
        {
            var message = (Message) JsonConvert.DeserializeObject(jsonInput, typeof(Message));

            return message.id == clientId ? null : message;
        } 
        catch(Exception) 
        {
            return new Message
            {
                text = jsonInput
            };
        }

            
    }

    private async Task SendMessage(string message)
    {
         await socket.SendAsync(
            new ArraySegment<Byte>(Encoding.UTF8.GetBytes(message)),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None
        );
    }

    private async Task ReceiveMessages()
    {
        while(true)
        {
            ArraySegment<Byte> buffer = new ArraySegment<Byte>(new Byte[MESSAGE_SIZE]);
            await socket.ReceiveAsync(buffer, CancellationToken.None);
            var message = DeserializeMessage(TrimResponse(buffer));
            if(message != null) Writer.Write(message);
        }
    }

    private String TrimResponse(ArraySegment<Byte> buffer)
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
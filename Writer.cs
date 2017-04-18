using Console = Colorful.Console;

public static class Writer
{
    public static void Write(Message message)
    {
        if(message.name == null)
        {
            Console.WriteLine($"{message.text}");
        }
        else
        {
            Console.WriteLine($"{message.name}: {message.text}");
        }
    }
}
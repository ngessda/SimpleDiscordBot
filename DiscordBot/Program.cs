namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Bot("YOUR TOKEN")
                .Start()
                .GetAwaiter()
                .GetResult();
        }
    }
}

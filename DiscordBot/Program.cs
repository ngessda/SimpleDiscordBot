namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Bot("ODkzNjE5MTY1NjgyODkyODUy.YVeFsw.4s9hgJ4aVPHXB3r4Xn93rO2Zwqs")
                .Start()
                .GetAwaiter()
                .GetResult();
        }
    }
}

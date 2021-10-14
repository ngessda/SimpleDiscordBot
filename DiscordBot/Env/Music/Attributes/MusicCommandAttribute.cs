namespace DiscordBot.Env.Music.Attributes
{
    using DiscordBot.Env.Music.Enums;
    using DiscordBot.Utils.Attributes;
    using DiscordBot.Utils.Enums;

    public class MusicCommandAttribute : CommandsEnvironmentAttribute
    {
        public CommandType CommandType { get; private set; }

        public MusicCommandAttribute(CommandType type) : base(CommandEnvironment.Music)
        {
            CommandType = type;
        }
    }
}

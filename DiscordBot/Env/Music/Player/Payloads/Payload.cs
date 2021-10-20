namespace DiscordBot.Env.Music.Player.Payloads
{
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DSharpPlus.CommandsNext;

    public abstract class Payload
    {
        public PayloadType Type { get; private set; } = PayloadType.None;

        public CommandContext Context { get; private set; }

        public Payload(CommandContext context, PayloadType type)
        {
            Context = context;
            Type = type;
        }
    }
}

namespace DiscordBot.Env.Music.Player.Payloads
{
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DSharpPlus.CommandsNext;

    public abstract class Payload
    {
        public PayloadType Type { get; private set; } = PayloadType.None;
        public CommandContext Context { get; private set; } = null;

        public Payload(CommandContext ctx, PayloadType type)
        {
            Context = ctx;
            Type = type;
        }
    }
}

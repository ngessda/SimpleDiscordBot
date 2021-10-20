namespace DiscordBot.Env.Music.Player.Payloads
{
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DSharpPlus.CommandsNext;

    public class PausePayload : Payload
    {
        public PausePayload(CommandContext ctx) : base(ctx, PayloadType.Pause)
        {
        }
    }
}

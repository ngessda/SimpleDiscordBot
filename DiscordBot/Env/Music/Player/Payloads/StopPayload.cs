namespace DiscordBot.Env.Music.Player.Payloads
{
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DSharpPlus.CommandsNext;

    public class StopPayload : Payload
    {
        public StopPayload(CommandContext ctx) : base(ctx, PayloadType.Stop)
        {
        }
    }
}

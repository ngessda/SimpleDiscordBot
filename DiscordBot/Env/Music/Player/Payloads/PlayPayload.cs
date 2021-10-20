namespace DiscordBot.Env.Music.Player.Payloads
{
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DSharpPlus.CommandsNext;

    public class PlayPayload : Payload
    {
        public PlayPayload(CommandContext ctx) : base(ctx, PayloadType.Play)
        {
        }
    }
}

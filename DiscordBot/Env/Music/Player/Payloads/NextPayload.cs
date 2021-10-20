namespace DiscordBot.Env.Music.Player.Payloads
{
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DSharpPlus.CommandsNext;

    public class NextPayload : Payload
    {
        public NextPayload(CommandContext ctx) : base(ctx, PayloadType.Next)
        {
        }
    }
}

namespace DiscordBot.Env.Music.Player.Payloads
{
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DSharpPlus.CommandsNext;

    public class BackPayload : Payload
    {
        public BackPayload(CommandContext ctx) : base(ctx, PayloadType.Back)
        {
        }
    }
}

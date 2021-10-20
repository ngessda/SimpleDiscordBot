namespace DiscordBot.Env.Music.Services
{
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Lavalink;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetConnectionService : IGetConnectionService
    {
        public Task<LavalinkGuildConnection> GetConnection(CommandContext ctx)
        {
            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values?.First();
            var voice = ctx.Member.VoiceState.Channel;
            var connection = node.GetGuildConnection();
        }
    }
}

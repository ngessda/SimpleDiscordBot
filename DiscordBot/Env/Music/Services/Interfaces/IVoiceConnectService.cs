namespace DiscordBot.Env.Music.Services.Interfaces
{
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Lavalink;
    using System;
    using System.Threading.Tasks;

    public interface IVoiceConnectService
    {
        public Task<LavalinkGuildConnection> EstablishConnection(CommandContext ctx, IServiceProvider services);
        public Task<LavalinkGuildConnection> GetGuildConnection(CommandContext ctx, IServiceProvider services);
    }
}

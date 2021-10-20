namespace DiscordBot.Env.Music.Services.Interfaces
{
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Lavalink;
    using System.Threading.Tasks;

    public interface IGetConnectionService
    {
        public Task<LavalinkGuildConnection> GetConnection(CommandContext ctx);
    }
}

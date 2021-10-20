namespace DiscordBot.Env.Music.Services.Interfaces
{
    using DSharpPlus.Lavalink;
    using System.Threading.Tasks;

    public interface IMusicSearchService
    {
        public Task<LavalinkLoadResult> GetTracks(LavalinkGuildConnection connection, string query);
    }
}

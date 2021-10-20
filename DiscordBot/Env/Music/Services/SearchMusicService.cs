namespace DiscordBot.Env.Music.Services
{
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.Lavalink;
    using System;
    using System.Threading.Tasks;

    public class SearchMusicService : IMusicSearchService
    {
        public Task<LavalinkLoadResult> GetTracks(LavalinkGuildConnection connection, string query)
        {
            throw new NotImplementedException();
        }
    }
}

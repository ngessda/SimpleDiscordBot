namespace DiscordBot.Env.Music.Services
{
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.Lavalink;
    using System.Threading.Tasks;

    public class MusicSearchService : IMusicSearchService
    {
        public async Task<LavalinkLoadResult> GetQueryResult(LavalinkGuildConnection conn, string query)
        {
            var loadResult = await conn.GetTracksAsync(query);
            if(loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                return null;
            }
            return loadResult;
        }
    }
}

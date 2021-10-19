namespace DiscordBot.Env.Music.Services.Interfaces
{
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Lavalink;
    using System.Threading.Tasks;

    public interface IChooseTrackService
    {
        public Task<LavalinkTrack> ChooseTrack(CommandContext ctx, LavalinkLoadResult loadResult);
    }
}

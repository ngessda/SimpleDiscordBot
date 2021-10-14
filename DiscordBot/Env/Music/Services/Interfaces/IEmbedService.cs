namespace DiscordBot.Env.Music.Services.Interfaces
{
    using DiscordBot.Env.Music.Enums;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Entities;
    using DSharpPlus.Lavalink;

    public interface IEmbedService
    {
        public DiscordEmbed CreateEmbed(EmbedType type);
        public DiscordEmbed CreateNowPlayingEmbed(LavalinkTrack track);
        public DiscordEmbed CreateAddedInQueueEmbed(CommandContext ctx, LavalinkTrack track);
        public DiscordEmbed CreateNoQueryResultEmbed(string query);
    }
}

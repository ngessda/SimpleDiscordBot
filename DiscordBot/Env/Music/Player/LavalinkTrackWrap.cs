namespace DiscordBot.Env.Music.Player
{
    using DSharpPlus.Entities;
    using DSharpPlus.Lavalink;

    public class LavalinkTrackWrap
    {
        public LavalinkTrack Track { get; private set; }

        public DiscordMessage AssociatedMessage { get; set; }

        public LavalinkTrackWrap(LavalinkTrack track)
        {
            Track = track;
        }
    }
}

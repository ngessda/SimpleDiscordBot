namespace DiscordBot.Env.Music.Player
{
    using DSharpPlus.Entities;
    using DSharpPlus.Lavalink;

    public class LavalinkWrappedTrack
    {
        public LavalinkTrack Track { get; private set; }

        public DiscordMessage AssociatedMessage { get; set; }

        public LavalinkWrappedTrack(LavalinkTrack track)
        {
            Track = track;
        }
    }
}

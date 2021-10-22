namespace DiscordBot.Env.Music.Player
{
    using DSharpPlus.Lavalink;

    public class LavalinkWrappedTrack
    {
        public LavalinkTrack Track { get; private set; }

        public ulong AssociatedMessageId { get; set; }

        public LavalinkWrappedTrack(LavalinkTrack track)
        {
            Track = track;
        }
    }
}

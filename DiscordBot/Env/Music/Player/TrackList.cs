namespace DiscordBot.Env.Music.Player
{
    using DSharpPlus.Lavalink;
    using System;
    using System.Collections.Generic;

    public class TrackList
    {
        private List<LavalinkTrack> tracks = new List<LavalinkTrack>();

        public int Position { get; private set; } = -1;

        public int Length => tracks.Count;

        public LavalinkTrack CurrentTrack
        {
            get
            {
                if(Length == 0 || Position == -1)
                {
                    return null;
                }
                return tracks[Position];
            }
        }

        public void Add(LavalinkTrack track)
        {
            tracks.Add(track);
            if(Position == -1)
            {
                Position = Length - 1;
            }
        }

        public bool HasNext()
        {
            return Position + 1 < Length;
        }

        public bool HasPrevious()
        {
            return Position - 1 > -1;
        }

        public void Clear()
        {
            ResetPosition();
            tracks.Clear();
        }

        public LavalinkTrack GetNextTrack()
        {
            if (HasNext())
            {
                Position++;
                return CurrentTrack;
            }
            return null;
        }

        public LavalinkTrack GetPreviousTrack()
        {
            if (HasPrevious())
            {
                Position--;
                return CurrentTrack;
            }
            return null;
        }

        public void ResetPosition()
        {
            Position = -1;
        }
    }
}

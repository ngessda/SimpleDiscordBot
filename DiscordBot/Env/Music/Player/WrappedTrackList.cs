namespace DiscordBot.Env.Music.Player
{
    using DSharpPlus.Lavalink;
    using System;
    using System.Collections.Generic;

    public class TrackList
    {
        private List<LavalinkTrackWrap> tracks = new List<LavalinkTrackWrap>();

        public int Position { get; private set; } = -1;

        public int Length => tracks.Count;

        public LavalinkTrackWrap CurrentTrackWrap
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

        public void Add(LavalinkTrackWrap track)
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

        public LavalinkTrackWrap GetNextTrackWrap()
        {
            if (HasNext())
            {
                Position++;
                return CurrentTrackWrap;
            }
            return null;
        }

        public LavalinkTrackWrap GetPreviousTrackWrap()
        {
            if (HasPrevious())
            {
                Position--;
                return CurrentTrackWrap;
            }
            return null;
        }

        public void ResetPosition()
        {
            Position = -1;
        }
    }
}

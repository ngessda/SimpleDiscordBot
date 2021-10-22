namespace DiscordBot.Env.Music.Player
{
    using System.Collections.Generic;

    public class WrappedTrackList
    {
        private List<LavalinkWrappedTrack> tracks = new List<LavalinkWrappedTrack>();

        public int Position { get; private set; } = -1;

        public int Length => tracks.Count;

        public LavalinkWrappedTrack CurrentTrackWrap
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

        public void Add(LavalinkWrappedTrack track)
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

        public LavalinkWrappedTrack GetNextTrackWrap()
        {
            if (HasNext())
            {
                Position++;
                return CurrentTrackWrap;
            }
            return null;
        }

        public LavalinkWrappedTrack GetPreviousTrackWrap()
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

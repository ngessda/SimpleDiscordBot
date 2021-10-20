namespace DiscordBot.Env.Music.Player
{
    using DiscordBot.Env.Music.Player.Enums;
    using DiscordBot.Env.Music.Player.Payloads;
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DiscordBot.Env.Music.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MusicPlayer
    {
        private delegate Task PayloadAction();
        private delegate Task TrackListAction();
        private event PayloadAction OnAppend;
        private event TrackListAction OnUpdate;
        private IServiceProvider _services;
        private TrackList _tracks = new TrackList();
        private Queue<Payload> payloads = new Queue<Payload>();
        public PlayerState State { get; private set; } = PlayerState.None;

        public MusicPlayer(IServiceProvider services)
        {
            _services = services;
            OnAppend += ExecutePayloads;
        }

        public async Task AppendPayload(Payload payload)
        {
            lock (payloads)
            {
                payloads.Enqueue(payload);
            }
            await OnAppend?.Invoke();
        }

        private async Task ExecutePayloads()
        {
            while(payloads.Count > 0)
            {
                Payload payload;
                lock (payloads)
                {
                    payload = payloads.Dequeue();
                }
                if(payload.Type == PayloadType.Query)
                {
                    await HandleQueryPayload(payload as QueryPayload);
                }
            }
        }

        private async Task HandleQueryPayload(QueryPayload payload)
        {
            
        }
    }
}

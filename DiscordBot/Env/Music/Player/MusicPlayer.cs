namespace DiscordBot.Env.Music.Player
{
    using DiscordBot.Env.Music.Enums;
    using DiscordBot.Env.Music.Player.Payloads;
    using DiscordBot.Env.Music.Player.Payloads.Enums;
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Lavalink;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MusicPlayer
    {
        private delegate Task PayloadAction();
        private delegate Task TrackListAction(CommandContext ctx, LavalinkGuildConnection connection);
        private event PayloadAction OnPayloadAppend;
        private event TrackListAction OnTrackListUpdated;
        private IServiceProvider _services;
        private TrackList _tracks = new TrackList();
        private Queue<Payload> _payloads = new Queue<Payload>();
        public PlayerState State { get; private set; } = PlayerState.None;

        public MusicPlayer(IServiceProvider services)
        {
            _services = services;
            OnPayloadAppend += ExecutePayloads;
            OnTrackListUpdated += PlayTrack;
        }

        public async Task AppendPayload(Payload payload)
        {
            lock (_payloads)
            {
                _payloads.Enqueue(payload);
            }
            await OnPayloadAppend?.Invoke();
        }

        private async Task ExecutePayloads()
        {
            while(_payloads.Count > 0)
            {
                Payload payload;
                lock (_payloads)
                {
                    payload = _payloads.Dequeue();
                }
                var connService = _services.GetService(typeof(IVoiceConnectService)) as IVoiceConnectService;
                var conn = await connService.GetGuildConnection(payload.Context, _services);
                if(conn == null)
                {
                    await connService.EstablishConnection(payload.Context, _services);
                }
                if(payload.Type == PayloadType.Query)
                {
                    await HandleQueryPayload(payload as QueryPayload);
                }
                else if(payload.Type == PayloadType.Play)
                {
                    await HandlePlayPayload(payload as PlayPayload);
                }
            }
        }

        private async Task HandleQueryPayload(QueryPayload payload)
        {
            var embed = _services.GetService(typeof(IEmbedService)) as IEmbedService;
            var connection = await (_services.GetService(typeof(IVoiceConnectService)) as IVoiceConnectService)
                .GetGuildConnection(payload.Context, _services);
            var loadResult = await (_services.GetService(typeof(IMusicSearchService)) as IMusicSearchService)
                .GetQueryResult(connection, payload.Query);
            if(loadResult == null)
            {
                await payload.Context.Channel.SendMessageAsync(
                    embed.CreateNoQueryResultEmbed(payload.Query)
                    );
                return;
            }
            var track = await (_services.GetService(typeof(IChooseTrackService)) as IChooseTrackService)
                .ChooseTrack(payload.Context, loadResult);
            if(track == null)
            {
                await payload.Context.Channel.SendMessageAsync(
                    embed.CreateEmbed(EmbedType.TrackIsntChosen)
                    );
                return;
            }
            lock (_tracks)
            {
                _tracks.Add(track);
            }
            await OnTrackListUpdated?.Invoke(payload.Context, connection);
        }

        private async Task PlayTrack(CommandContext ctx, LavalinkGuildConnection connection)
        {
            if(State == PlayerState.Playing)
            {
                return;
            }
            State = PlayerState.Playing;
            await connection.PlayAsync(_tracks.CurrentTrack);
            await ctx.Channel.SendMessageAsync(
                (_services.GetService(typeof(IEmbedService)) as IEmbedService)
                .CreateNowPlayingEmbed(_tracks.CurrentTrack)
                );
        }

        private async Task HandlePlayPayload(PlayPayload payload)
        {
            var embed = _services.GetService(typeof(IEmbedService)) as IEmbedService;
            if(State == PlayerState.Playing)
            {
                return;
            }
            if(State == PlayerState.Stopped || State == PlayerState.None)
            {
                await payload.Context.Channel.SendMessageAsync(
                    embed.CreateEmbed(EmbedType.EmptyQueue)
                    );
                return;
            }
            var conn = await (_services.GetService(typeof(IVoiceConnectService)) as IVoiceConnectService)
                .GetGuildConnection(payload.Context, _services);
            await conn.ResumeAsync();
        }
    }
}

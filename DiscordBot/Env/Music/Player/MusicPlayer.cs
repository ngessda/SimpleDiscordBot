namespace DiscordBot.Env.Music.Player
{
    using DiscordBot.Env.Music.Enums;
    using DiscordBot.Env.Music.Player.Enums;
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
        private WrappedTrackList _wrappedTracks = new WrappedTrackList();
        private Queue<Payload> _payloads = new Queue<Payload>();
        private ShouldPlay _shouldPlay = new ShouldPlay();
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
                    conn = await connService.EstablishConnection(payload.Context, _services);
                    HandleConnectionEvents(payload.Context, conn);
                }
                if(payload.Type == PayloadType.Query)
                {
                    await HandleQueryPayload(payload as QueryPayload);
                }
                else if(payload.Type == PayloadType.Play)
                {
                    await HandlePlayPayload(payload as PlayPayload);
                }
                else if(payload.Type == PayloadType.Pause)
                {
                    await HandlePausePayload(payload as PausePayload);
                }
                else if(payload.Type == PayloadType.Stop)
                {
                    await HandleStopPayload(payload as StopPayload);
                }
                else if(payload.Type == PayloadType.Next)
                {
                    await HandleNextPayload(payload as NextPayload);
                }
                else if(payload.Type == PayloadType.Back)
                {
                    await HandleBackPayload(payload as BackPayload);
                }
            }
        }

        private void HandleConnectionEvents(CommandContext ctx, LavalinkGuildConnection connection)
        {
            connection.PlaybackStarted += async (s, e) =>
            {
                State = PlayerState.Playing;
                var msg = await ctx.Channel.SendMessageAsync(
                (_services.GetService(typeof(IEmbedService)) as IEmbedService)
                .CreateNowPlayingEmbed(_wrappedTracks.CurrentTrackWrap.Track)
                );
                _wrappedTracks.CurrentTrackWrap.AssociatedMessage = msg;
            };
            connection.PlaybackFinished += async (k, t) =>
            {
                State = PlayerState.None;
                await _wrappedTracks.CurrentTrackWrap.AssociatedMessage.DeleteAsync();
                if (_shouldPlay.Result)
                {
                    LavalinkTrack track;
                    lock (_wrappedTracks)
                    {
                        if (!_wrappedTracks.HasNext())
                        {
                            return;
                        }
                        track = _wrappedTracks.GetNextTrackWrap().Track;
                    }
                    if (track == null)
                    {
                        return;
                    }
                    else
                    {
                        await PlayTrack(track, connection);
                    }
                }
                _shouldPlay.Reset();
            };
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
            lock (_wrappedTracks)
            {
                _wrappedTracks.Add(new LavalinkWrappedTrack(track));
            }
            await payload.Context.Channel.SendMessageAsync(
                embed.CreateAddedInQueueEmbed(payload.Context, track)
                );
            await OnTrackListUpdated?.Invoke(payload.Context, connection);
        }

        private async Task PlayTrack(CommandContext ctx, LavalinkGuildConnection connection)
        {
            if(State == PlayerState.Playing)
            {
                return;
            }
            await connection.PlayAsync(_wrappedTracks.CurrentTrackWrap.Track);
            State = PlayerState.Playing;
        }

        private async Task PlayTrack(LavalinkTrack track, LavalinkGuildConnection connection)
        {
            if(State == PlayerState.Playing)
            {
                return;
            }
            await connection.PlayAsync(track);
            State = PlayerState.Playing;
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

        private async Task HandlePausePayload(PausePayload payload)
        {
            if(State == PlayerState.Paused || State == PlayerState.Stopped || State == PlayerState.None)
            {
                return;
            }
            else
            {
                var conn = await (_services.GetService(typeof(IVoiceConnectService)) as IVoiceConnectService)
                .GetGuildConnection(payload.Context, _services);
                await conn.PauseAsync();
                State = PlayerState.Paused;
            }
        }

        private async Task HandleNextPayload(NextPayload payload)
        {
            if(State == PlayerState.Stopped)
            {
                return;
            }
            else
            {
                LavalinkTrack track;
                lock (_wrappedTracks)
                {
                    track = _wrappedTracks.GetNextTrackWrap().Track;
                }
                if(track == null)
                {
                    return;
                }
                _shouldPlay.SetReason(Reason.NextTrack);
                var conn = await (_services.GetService(typeof(IVoiceConnectService)) as IVoiceConnectService)
                .GetGuildConnection(payload.Context, _services);
                await conn.PlayAsync(track);
            }
        }

        private async Task HandleBackPayload(BackPayload payload)
        {
            if (State == PlayerState.Stopped)
            {
                return;
            }
            else
            {
                LavalinkTrack track;
                lock (_wrappedTracks)
                {
                    track = _wrappedTracks.GetPreviousTrackWrap().Track;
                }
                if (track == null)
                {
                    return;
                }
                _shouldPlay.SetReason(Reason.PreviosTrack);
                var conn = await (_services.GetService(typeof(IVoiceConnectService)) as IVoiceConnectService)
                .GetGuildConnection(payload.Context, _services);
                await conn.PlayAsync(track);
            }
        }

        private async Task HandleStopPayload(StopPayload payload)
        {
            if(State == PlayerState.Stopped)
            {
                return;
            }
            else
            {
                var conn = await (_services.GetService(typeof(IVoiceConnectService)) as IVoiceConnectService)
                .GetGuildConnection(payload.Context, _services);
                await conn.StopAsync();
                State = PlayerState.Stopped;
                _wrappedTracks.Clear();
            }
        }
    }
}

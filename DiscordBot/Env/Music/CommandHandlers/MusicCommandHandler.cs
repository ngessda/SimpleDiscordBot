namespace DiscordBot.Env.Music.CommandHandlers
{
    using DiscordBot.Env.Music.Attributes;
    using DiscordBot.Env.Music.Enums;
    using DiscordBot.Env.Music.Player;
    using DiscordBot.Env.Music.Player.Payloads;
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.CommandsNext;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MusicCommandHandler
    {
        private IServiceProvider _services;
        private MusicPlayer _player;

        public MusicCommandHandler(CommandsNextExtension commands)
        {
            _services = commands.Services;
            _player = new MusicPlayer(_services);
            HandleCommands(commands);
        }

        private void HandleCommands(CommandsNextExtension commands)
        {
            commands.CommandExecuted += async (s, e) =>
            {
                var cmd = e.Command;
                var ctx = e.Context;
                var args = ctx.RawArguments;
                var type = GetCommandType(cmd);
                if (!InMusicEnvironment(cmd))
                {
                    return;
                }

                if(type == CommandType.Join)

                if(type == CommandType.Join)
                {
                    await HandleJoinCommand(ctx);
                }
                else if(type == CommandType.Leave)
                {
                    await HandleLeaveCommand(ctx);
                }
                else if (type == CommandType.Play)
                {
                    await HandlePlayCommand(ctx, args);
                }
                else if(type == CommandType.Pause)
                {
                    await HandlePauseCommand(ctx);
                }
                else if(type == CommandType.Stop)
                {
                    await HandleStopCommand(ctx);
                }
                else if(type == CommandType.Next)
                {
                    await HandleNextCommand(ctx);
                }
                else if(type == CommandType.Back)
                {
                    await HandleBackCommand(ctx);
                }
            };
        }

        private CommandType GetCommandType(Command command)
        {
            var attr = command.CustomAttributes
                .Where(a => a is MusicCommandAttribute)
                .First() as MusicCommandAttribute;
            return attr.CommandType;
        }

        private bool InMusicEnvironment(Command command)
        {
            return command.CustomAttributes.Any(a => a is MusicCommandAttribute);
        }

        private async Task HandleJoinCommand(CommandContext ctx)
        {
            var connectService = _services.GetService(typeof(IVoiceConnectService)) as IVoiceConnectService;
            await connectService.EstablishConnection(ctx, _services);
        }

        private async Task HandleLeaveCommand(CommandContext ctx)
        {
            var disconnectService = _services.GetService(typeof(IVoiceDisconnectService)) as IVoiceDisconnectService;
            await disconnectService.CloseConnection(ctx, _services);
        }

        private async Task HandlePlayCommand(CommandContext ctx, IReadOnlyList<string> args)
        {
            var query = args?.First();
            if(query == null)
            {
                await _player.AppendPayload(new PlayPayload(ctx));
            }
            else
            {
                await _player.AppendPayload(new QueryPayload(ctx, query));
            }
        }

        private async Task HandlePauseCommand(CommandContext ctx)
        {
            await _player.AppendPayload(new PausePayload(ctx));
        }

        private async Task HandleNextCommand(CommandContext ctx)
        {
            await _player.AppendPayload(new NextPayload(ctx));
        }

        private async Task HandleBackCommand(CommandContext ctx)
        {
            await _player.AppendPayload(new BackPayload(ctx));
        }

        private async Task HandleStopCommand(CommandContext ctx)
        {
            await _player.AppendPayload(new StopPayload(ctx));
        }
    }
}

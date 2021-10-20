namespace DiscordBot.Env.Music.CommandHandlers
{
    using DiscordBot.Env.Music.Attributes;
    using DiscordBot.Env.Music.Enums;
    using DiscordBot.Env.Music.Player;
    using DiscordBot.Env.Music.Player.Payloads;
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.CommandsNext;
    using System;
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
                if (!InMusicEnvironment(cmd))
                {
                    return;
                }

                var type = GetCommandType(cmd);

                if(type == CommandType.Join)
                {
                    await HandleJoinCommand(ctx);
                }
                else if(type == CommandType.Leave)
                {
                    await HandleLeaveCommand(ctx);
                }
                else if(type == CommandType.Play)
                {
                    await HandlePlayCommand(ctx, cmd);
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

        private async Task HandlePlayCommand(CommandContext ctx, Command cmd)
        {
            var query = cmd.Overloads?.First().Arguments?.First().DefaultValue as string;
            await _player.AppendPayload(new QueryPayload(ctx, query));
        }
    }
}

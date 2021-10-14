namespace DiscordBot.Env.Music.CommandHandlers
{
    using DiscordBot.Env.Music.Attributes;
    using DiscordBot.Env.Music.Enums;
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.CommandsNext;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class MusicCommandHandler
    {
        private IServiceProvider _services;

        public MusicCommandHandler(CommandsNextExtension commands)
        {
            _services = commands.Services;
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
                
                if(GetCommandType(cmd) == CommandType.Join)
                {
                    await HandleJoinCommand(ctx);
                }
                else if(GetCommandType(cmd) == CommandType.Leave)
                {
                    await HandleLeaveCommand(ctx);
                }
            };
        }

        private CommandType GetCommandType(Command command)
        {
            var attr = Attribute
                .GetCustomAttribute(command.GetType().GetTypeInfo(), typeof(MusicCommandAttribute)) as MusicCommandAttribute;
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
    }
}

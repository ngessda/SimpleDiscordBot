namespace DiscordBot.Env.Music.CommandHandlers
{
    using DiscordBot.Env.Music.Attributes;
    using DiscordBot.Env.Music.Enums;
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
                else if(GetCommandType(cmd) == CommandType.Join)
                {
                    await HandleJoinCommand(ctx);
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

        }
    }
}

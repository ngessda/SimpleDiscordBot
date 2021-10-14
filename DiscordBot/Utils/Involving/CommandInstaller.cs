namespace DiscordBot.Utils.Involving
{
    using DiscordBot.Utils.Involving.Interfaces;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using System;
    using System.Linq;
    using System.Reflection;

    public class CommandInstaller : ICommandInstaller
    {
        private string[] _prefixes = new[] { "!", "-" };
        private Assembly _baseAssembly;

        public CommandInstaller()
        {
            _baseAssembly = Assembly.GetExecutingAssembly();
        }

        public CommandInstaller(Assembly baseAssembly)
        {
            _baseAssembly = baseAssembly;
        }

        public CommandsNextExtension SetupAndGetCommands(DiscordClient client, IServiceProvider services)
        {
            CommandsNextExtension commands = null;

            EnableAndConfigureCommands(client, ref commands, services);
            RegisterCommands(commands);

            return commands;
        }

        public void SetupCommands(DiscordClient client, ref CommandsNextExtension commands, IServiceProvider services)
        {
            EnableAndConfigureCommands(client, ref commands, services);
            RegisterCommands(commands);
        }

        private void EnableAndConfigureCommands(DiscordClient client, ref CommandsNextExtension commands, IServiceProvider services)
        {
            commands = client.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = _prefixes,
                Services = services
            });
        }

        private void RegisterCommands(CommandsNextExtension commands)
        {
            var commandsList = _baseAssembly.GetTypes()
                .Where(t => t.Namespace.EndsWith("Commands") && !t.Name.StartsWith("<") && t.Name.EndsWith("Commands"))
                .ToList();
            foreach(var c in commandsList)
            {
                commands.RegisterCommands(c);
            }
        }
    }
}

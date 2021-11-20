namespace DiscordBot.Utils.Involving
{
    using DiscordBot.Utils.Involving.Interfaces;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using System;
    using System.Linq;
    using System.Reflection;

    public class CommandsHandlerInstaller : ICommandHandlerInstaller
    {
        private Assembly _baseAssembly;

        public CommandsHandlerInstaller()
        {
            _baseAssembly = Assembly.GetExecutingAssembly();
        }

        public CommandsHandlerInstaller(Assembly baseAssembly)
        {
            _baseAssembly = baseAssembly;
        }
        public void Setup(CommandsNextExtension commands)
        {
            var extList = _baseAssembly.GetTypes()
            .Where(t => t.Namespace.EndsWith("CommandHandlers") && t.IsClass && t.Name.EndsWith("CommandHandler"))
            .ToList();
            foreach (var extType in extList)
            {
                var ext = Activator.CreateInstance(extType) as CommandHandler;
            }
        }
    }
}
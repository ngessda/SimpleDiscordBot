namespace DiscordBot.Utils.Involving
{
    using DiscordBot.Utils.Involving.Interfaces;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using System;

    public class BotConfigurator : IBotConfigurator
    {
        public void ConfigureClient(DiscordClient client)
        {
            
        }

        public void ConfigureCommands(DiscordClient client, ref CommandsNextExtension commands, IServiceProvider services)
        {
            commands = new CommandInstaller().SetupAndGetCommands(client, services);
        }

        public void ConfigureServices(ref IServiceProvider services)
        {
            services = new ServiceInstaller().SetupAndGetServices();
        }
    }
}

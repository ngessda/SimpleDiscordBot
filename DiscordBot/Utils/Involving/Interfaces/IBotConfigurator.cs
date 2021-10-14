using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;

namespace DiscordBot.Utils.Involving.Interfaces
{
    public interface IBotConfigurator
    {
        public void ConfigureClient(DiscordClient client);
        public void ConfigureServices(ref IServiceProvider services);
        public void ConfigureCommands(DiscordClient client, ref CommandsNextExtension commands, IServiceProvider services);
        public void ConfigureExtensions(DiscordClient client);
    }
}

namespace DiscordBot.Utils.Involving.Interfaces
{
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using System;
    public interface ICommandsInstaller
    {
        public CommandsNextExtension SetupAndGetCommands(DiscordClient client, IServiceProvider services);
        public void SetupCommands(DiscordClient client, ref CommandsNextExtension commands, IServiceProvider services);
    }
}

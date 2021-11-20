namespace DiscordBot.Utils.Involving.Interfaces
{
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using System;
    public interface ICommandHandlerInstaller
    {
        public void Setup(CommandsNextExtension commands);
    }
}
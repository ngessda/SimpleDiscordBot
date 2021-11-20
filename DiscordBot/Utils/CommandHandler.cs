using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Utils
{
    public abstract class CommandHandler
    {
        public IServiceProvider _services;
        public CommandHandler(CommandsNextExtension commands)
        {
            _services = commands.Services;
        }
    }
}

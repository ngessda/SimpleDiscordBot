namespace DiscordBot.Env.Music.Services.Interfaces
{
    using DSharpPlus.CommandsNext;
    using System;
    using System.Threading.Tasks;

    public interface IVoiceDisconnectService
    {
        public Task CloseConnection(CommandContext ctx, IServiceProvider services);
    }
}

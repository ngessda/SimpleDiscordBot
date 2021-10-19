namespace DiscordBot.Env.Music.Services
{
    using DiscordBot.Env.Music.Enums;
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Lavalink;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class VoiceDisconnectService : IVoiceDisconnectService
    {
        public async Task CloseConnection(CommandContext ctx, IServiceProvider services)
        {
            var embed = services.GetService(typeof(IEmbedService)) as IEmbedService;
            var textChannel = ctx.Channel;
            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values?.First();
            if(node == null)
            {
                await textChannel.SendMessageAsync(
                    embed.CreateEmbed(EmbedType.NoServerConnection)
                    );
                return;
            }
            var voiceChannel = ctx.Member.VoiceState?.Channel;
            if(voiceChannel == null)
            {
                await textChannel.SendMessageAsync(
                    embed.CreateEmbed(EmbedType.NotInVoiceChannel)
                    );
                return;
            }
            var connection = node.GetGuildConnection(voiceChannel.Guild);
            if(connection != null)
            {
                await connection.DisconnectAsync();
            }
        }
    }
}

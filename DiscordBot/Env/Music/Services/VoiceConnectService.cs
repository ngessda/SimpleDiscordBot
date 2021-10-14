namespace DiscordBot.Env.Music.Services
{
    using DiscordBot.Env.Music.Enums;
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Lavalink;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class VoiceConnectService : IVoiceConnectService
    {
        public async Task<LavalinkGuildConnection> EstablishConnection(CommandContext ctx, IServiceProvider services)
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
                return null;
            }
            var voiceChannel = ctx.Member.VoiceState?.Channel;
            if(voiceChannel == null)
            {
                await textChannel.SendMessageAsync(
                    embed.CreateEmbed(EmbedType.NotInVoiceChannel)
                    );
                return null;
            }
            var connection = node.GetGuildConnection(ctx.Guild);
            if(connection == null)
            {
                connection = await node.ConnectAsync(voiceChannel);
            }
            return connection;
        }
    }
}

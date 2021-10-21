namespace DiscordBot.Env.Music.Services
{
    using DiscordBot.Env.Music.Services.Interfaces;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Entities;
    using DSharpPlus.Interactivity.Extensions;
    using DSharpPlus.Lavalink;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ChooseTrackService : IChooseTrackService
    {
        private Dictionary<string, int> options = new Dictionary<string, int>();

        public ChooseTrackService()
        {
            options[":one:"] = 0;
            options[":two:"] = 1;
            options[":three:"] = 2;
            options[":four:"] = 3;
            options[":five:"] = 4;
        }
        public async Task<LavalinkTrack> ChooseTrack(CommandContext ctx, LavalinkLoadResult loadResult)
        {
            var tracks = loadResult.Tracks.Take(options.Count).ToList();
            var embed = CreateEmbed(ctx, tracks);
            var msg = await ctx.Channel.SendMessageAsync(embed);
            foreach(var emoji in options.Keys)
            {
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, emoji));
            }
            var reaction = await ctx.Client.GetInteractivity()
                .WaitForReactionAsync(x => x.Message == msg, ctx.Member);
            await msg.DeleteAsync();
            if (reaction.TimedOut)
            {
                return null;
            }
            else
            {
                return tracks[options[reaction.Result.Emoji.GetDiscordName()]];
            }
        }

        private DiscordEmbed CreateEmbed(CommandContext ctx, IList<LavalinkTrack> tracks)
        {
            var sb = new StringBuilder();
            var builder = new DiscordEmbedBuilder();
            builder
                .WithColor(DiscordColor.Cyan)
                .WithTitle("Выберите песню");
            sb.Append("\n");
            for(int i = 0; i < tracks.Count; i++)
            {
                var t = tracks[i];
                var ts = new TimeSpan(t.Length.Ticks);
                sb.Append($"**{i + 1}.** {t.Title} ({ts.Minutes}:{ts.Seconds})\n");
            }
            builder.Description += sb.ToString();
            builder.Timestamp = DateTime.UtcNow;
            builder.WithAuthor("Результат поиска");
            builder.WithFooter($"Автор запроса {ctx.Member.DisplayName}", ctx.Member.AvatarUrl);
            return builder.Build();
        }
    }
}

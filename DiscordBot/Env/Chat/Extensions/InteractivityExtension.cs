namespace DiscordBot.Env.Chat.Extensions
{
    using DiscordBot.Utils.Involving.Interfaces;
    using DSharpPlus;
    using DSharpPlus.Interactivity;
    using DSharpPlus.Interactivity.Enums;
    using DSharpPlus.Interactivity.Extensions;
    using System;
    using System.Threading.Tasks;

    public class InteractivityExtension : IExtension
    {
        public async Task Setup(DiscordClient client)
        {
            client.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromSeconds(45)
            });
        }
    }
}

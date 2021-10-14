namespace DiscordBot
{
    using DiscordBot.Utils.Involving;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using System;
    using System.Threading.Tasks;

    public class Bot
    {
        private DiscordClient _client;
        private IServiceProvider _services;
        private CommandsNextExtension _commands;

        public Bot(string token)
        {
            _client = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
                AutoReconnect = true
            });

            var cfg = new BotConfigurator();
            cfg.ConfigureServices(ref _services);
            cfg.ConfigureCommands(_client, ref _commands, _services);
            cfg.ConfigureExtensions(_client);
        }

        public async Task Start()
        {
            await _client.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}

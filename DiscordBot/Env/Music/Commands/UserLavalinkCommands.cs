namespace DiscordBot.Env.Music.Commands
{
    using DiscordBot.Env.Music.Attributes;
    using DiscordBot.Env.Music.Enums;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using System.Threading.Tasks;

    public class UserLavalinkCommands : BaseCommandModule
    {
        [Command("join"), Aliases("j")]
        [MusicCommand(CommandType.Join)]
        public async Task JoinCommand(CommandContext ctx) { }

        [Command("leave"), Aliases("l")]
        [MusicCommand(CommandType.Leave)]
        public async Task LeaveCommand(CommandContext ctx) { }

        [Command("play"), Aliases("p")]
        [MusicCommand(CommandType.Play)]
        public async Task PlayCommand(CommandContext ctx, [RemainingText, Query] string query) { }

        [Command("pause"), Aliases("ps")]
        [MusicCommand(CommandType.Pause)]
        public async Task PauseCommand(CommandContext ctx) { }

        [Command("stop"), Aliases("s")]
        [MusicCommand(CommandType.Stop)]
        public async Task StopCommand(CommandContext ctx) { }

        [Command("next"), Aliases("n")]
        [MusicCommand(CommandType.Next)]
        public async Task NextCommand(CommandContext ctx) { }

        [Command("back"), Aliases("b")]
        [MusicCommand(CommandType.Back)]
        public async Task BackCommand(CommandContext ctx) { }
    }
}

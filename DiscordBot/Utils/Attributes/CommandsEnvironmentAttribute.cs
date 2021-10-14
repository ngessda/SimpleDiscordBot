namespace DiscordBot.Utils.Attributes
{
    using DiscordBot.Utils.Enums;
    using System;

    public class CommandsEnvironmentAttribute : Attribute
    {
        public CommandEnvironment Environment { get; private set; }

        public CommandsEnvironmentAttribute(CommandEnvironment env)
        {
            Environment = env;
        }
    }
}

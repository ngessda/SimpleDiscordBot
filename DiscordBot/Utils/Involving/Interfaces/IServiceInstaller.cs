namespace DiscordBot.Utils.Involving.Interfaces
{
    using System;
    public interface IServiceInstaller
    {
        public void SetupServices(ref IServiceProvider services);
        public IServiceProvider SetupAndGetServices();
    }
}

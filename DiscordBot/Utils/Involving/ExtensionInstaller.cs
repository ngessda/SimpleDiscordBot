namespace DiscordBot.Utils.Involving
{
    using DiscordBot.Utils.Involving.Interfaces;
    using DSharpPlus;
    using System.Linq;
    using System.Reflection;

    class ExtensionInstaller : IExtensionInstaller
    {
        private Assembly _baseAssembly;

        public ExtensionInstaller()
        {
            _baseAssembly = Assembly.GetExecutingAssembly();
        }

        public ExtensionInstaller(Assembly baseAssembly)
        {
            _baseAssembly = baseAssembly;
        }

        public void SetupExtensions(DiscordClient client)
        {
            var extList = _baseAssembly.GetTypes().Where(t => t.Namespace.EndsWith("Extensions"));
            foreach(var ext in extList)
            {
                (ext as IExtension).Setup(client);
            }
        }
    }
}

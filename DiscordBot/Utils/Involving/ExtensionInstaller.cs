namespace DiscordBot.Utils.Involving
{
    using DiscordBot.Utils.Involving.Interfaces;
    using DSharpPlus;
    using System;
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
            var extList = _baseAssembly.GetTypes()
                .Where(t => t.Namespace.EndsWith("Extensions") && t.IsClass && t.Name.EndsWith("Extension"))
                .ToList();
            foreach(var extType in extList)
            {
                var ext = Activator.CreateInstance(extType) as IExtension;
                ext.Setup(client);
            }
        }
    }
}

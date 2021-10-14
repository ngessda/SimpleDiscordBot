namespace DiscordBot.Utils.Involving
{
    using DiscordBot.Utils.Involving.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Reflection;

    class ServiceInstaller : IServiceInstaller
    {
        private Assembly _baseAssembly;
        public ServiceInstaller()
        {
            _baseAssembly = Assembly.GetExecutingAssembly();
        }

        public ServiceInstaller(Assembly baseAssembly)
        {
            _baseAssembly = baseAssembly;
        }
        public IServiceProvider SetupAndGetServices()
        {
            var collection = new ServiceCollection();
            FillCollection(collection);
            return collection.BuildServiceProvider();
        }

        public void SetupServices(ref IServiceProvider services)
        {
            var collection = new ServiceCollection();
            FillCollection(collection);
            services = collection.BuildServiceProvider();
        }

        private void FillCollection(ServiceCollection collection)
        {
            var typeList = _baseAssembly.GetTypes()
                .Where(t => t.Namespace.EndsWith("Services.Interfaces")
                        && t.IsInterface)
                .ToList();
            var implList = _baseAssembly.GetTypes()
                .Where(t => t.Namespace.EndsWith("Services") 
                        && t.IsClass)
                .ToList();
            for(int i = 0; i < typeList.Count; i++)
            {
                for(int j = 0; j < implList.Count; j++)
                {
                    if (typeList[i].Name.EndsWith(implList[j].Name))
                    {
                        collection.AddScoped(typeList[i], implList[j]);
                        break;
                    }
                }
            }
        }
    }
}

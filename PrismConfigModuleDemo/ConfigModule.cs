using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Modularity;

namespace PrismConfigModuleDemo
{
    public class ConfigModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ConfigManager>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //containerProvider.Resolve<ConfigManager>().WatchConfigFile();
        }
    }
}

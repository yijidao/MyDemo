using LibVLCSharp.Shared;
using VlcPrismModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace VlcPrismModule
{
    public class VlcPrismModuleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //Core.Initialize();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MediaPlayerView>();
            containerRegistry.RegisterForNavigation<MonitorView>();
            containerRegistry.RegisterForNavigation<MonitorView2>();
        }
    }
}
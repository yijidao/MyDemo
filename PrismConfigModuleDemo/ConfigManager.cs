using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismConfigModuleDemo
{
    public class ConfigManager : IDisposable
    {

        public string ConfigPath => Path.Combine(Environment.CurrentDirectory, "PrismDemo.config");

        public FileSystemWatcher Watcher { get; private set; }

        public IDisposable ObservableDisposable { get; set; }

        public ConfigManager()
        {
            
        }

        public void Load()
        {
            var fileMap = new ExeConfigurationFileMap {ExeConfigFilename = ConfigPath};
            var c= ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            c.Save(ConfigurationSaveMode.Modified);
            
            ConfigurationManager.RefreshSection(c.AppSettings.SectionInformation.Name);
        }

        public void WatchConfigFile()
        {
            if (Watcher != null) return;
            var watcher = new FileSystemWatcher($"{Environment.CurrentDirectory}")
            {
                Filter = "PrismDemo.config",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                EnableRaisingEvents = true,
                IncludeSubdirectories = true

            };

            ObservableDisposable = Observable.FromEventPattern(watcher, nameof(watcher.Changed))
                .Throttle(TimeSpan.FromSeconds(2))
                .Subscribe(_ =>
                {
                    Load();
                });
            
            

            Watcher = watcher;
        }


        public void Dispose()
        {
            Watcher?.Dispose();
            ObservableDisposable?.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Castle.Core.Logging;

namespace PrismConfigModuleDemo
{

    public class ConfigManager : IDisposable
    {
        private readonly ILogger _logger;
        private FileSystemWatcher _watcher;
        private IDisposable _observableDisposable;
        private Configuration _config;
        //public Configuration Config { get; private set; }

        public ConfigManager(ILogger logger)
        {
            _logger = logger;
            Load();
            WatchConfigFile();
        }

        private void Load()
        {
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, "PrismDemo.config");
                _config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap(path),
                    ConfigurationUserLevel.None);
            }
            catch (Exception e)
            {
                _logger.Error("加载config", e);
            }

        }

        private void WatchConfigFile()
        {
            var watcher = new FileSystemWatcher($"{Environment.CurrentDirectory}")
            {
                Filter = "PrismDemo.config",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                EnableRaisingEvents = true,
                IncludeSubdirectories = true

            };

            _observableDisposable = Observable.FromEventPattern(watcher, nameof(watcher.Changed))
                .Throttle(TimeSpan.FromSeconds(2))
                .Subscribe(_ =>
                {
                    Load();
                });
            _watcher = watcher;
        }

        public string GetAppSetting(string key)
        {
            return _config.AppSettings.Settings[key]?.Value ?? "";
        }


        public void Dispose()
        {
            _watcher?.Dispose();
            _observableDisposable?.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace ConfigDemo.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        private ObservableCollection<ConfigurationSection> _configSections = new ();
        public ObservableCollection<ConfigurationSection> ConfigSections
        {
            get => _configSections;
            set => SetProperty(ref _configSections, value);
        }


        public ConfigViewModel()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, "demo3.config");
                //var map = new ExeConfigurationFileMap(path);
                var map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = path
                };
                var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                foreach (ConfigurationSection section in config.Sections)
                {
                    if (section is WebConfigSection || section is CommonConfigSection)
                    {
                        ConfigSections.Add(section);
                    }
                }


            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}

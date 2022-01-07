using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ConfigDemo.ViewModels
{
    public class ConfigViewModel : BindableBase, IDialogAware
    {
        private ObservableCollection<AbstractConfigSection> _configSections;
        public ObservableCollection<AbstractConfigSection> ConfigSections
        {
            get => _configSections;
            set => SetProperty(ref _configSections, value);
        }

        private string _configPath;
        public string ConfigPath
        {
            get => _configPath;
            set => SetProperty(ref _configPath, value);
        }

        private Configuration _config;

        public ICommand LoadConfigCommand { get; }

        public ICommand SaveConfigCommand { get; }

        public ICommand CheckConfigCommand { get; }

        public ICommand NotepadCommand { get; }

        public ConfigViewModel()
        {
            LoadConfigCommand = new DelegateCommand(() =>
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "config files (*.config)|*.config",
                    InitialDirectory = Path.Combine(Environment.CurrentDirectory, "Config")
                };


                if (openFileDialog.ShowDialog() != true) return;
                ConfigPath = openFileDialog.FileName;
                LoadConfig();
            });

            SaveConfigCommand = new DelegateCommand(() =>
            {
                _config.Save();
            });

            NotepadCommand = new DelegateCommand(() =>
            {
                Process.Start("notepad.exe", ConfigPath);
            });

            CheckConfigCommand = new DelegateCommand(() =>
            {
                foreach (var section in ConfigSections)
                {

                    if (!(section is CommonConfigSection cfs)) return;
                    foreach (CommonConfigElement o in cfs)
                    {
                        o.Check();
                    }
                }
            });

            var path = Path.Combine(Environment.CurrentDirectory, "Config/Hjmos_Ncc2.config");
            ConfigPath = path;

            LoadConfig();

        }

        private void LoadConfig()
        {
            try
            {
                var list = new List<AbstractConfigSection>();

                var map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = ConfigPath
                };
                _config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                foreach (ConfigurationSection section in _config.Sections)
                {
                    if (section is AbstractConfigSection abstractConfigSection)
                    {
                        list.Add(abstractConfigSection);
                    }
                }

                ConfigSections = new ObservableCollection<AbstractConfigSection>(list);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public string Title { get; }
        public event Action<IDialogResult> RequestClose;
    }
}

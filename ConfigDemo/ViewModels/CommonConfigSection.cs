using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConfigDemo.ViewModels
{
    public class CommonConfigSection : AbstractConfigSection, IEnumerable
    {
        [ConfigurationProperty("settings", IsRequired = true, IsDefaultCollection = false)]
        public CommonConfigElementCollection Settings => (CommonConfigElementCollection)this["settings"];

        public virtual IEnumerator GetEnumerator() => Settings.GetEnumerator();
    }

    public class CommonConfigElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new CommonConfigElement();

        protected override object GetElementKey(ConfigurationElement element) => ((CommonConfigElement)element).Key;
    }


    public class CommonConfigElement : ConfigurationElement, INotifyPropertyChanged
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get => (string)this["key"];
            set => this["key"] = value;
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get => (string)this["value"];
            set => this["value"] = value;
        }

        [ConfigurationProperty("description")]
        public string Description
        {
            get => (string)this["description"];
            set => this["description"] = value;
        }

        private ConfigCheck _checkStatus = ConfigCheck.None;
        public ConfigCheck CheckStatus
        {
            get => _checkStatus;
            set => SetProperty(ref _checkStatus, value);
        }

        public async Task Check()
        {
            CheckStatus = ConfigCheck.Checking;
            await Task.Delay(2000);
            CheckStatus = ConfigCheck.Success;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
        #endregion
    }

    public enum ConfigCheck
    {
        None,
        Checking,
        Success,
        Error
    }
}

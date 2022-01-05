using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigDemo
{
    public class CommonConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("description")]
        public string Description
        {
            get => (string)this["description"];
            set => this["description"] = value;
        }

        [ConfigurationProperty("name")]
        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        [ConfigurationProperty("settings", IsRequired = true, IsDefaultCollection = false)]
        public CommonConfigElementCollection Settings => (CommonConfigElementCollection)this["settings"];
    }

    public class CommonConfigElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new CommonConfigElement();

        protected override object GetElementKey(ConfigurationElement element) => ((CommonConfigElement)element).Key;
    }

    public class CommonConfigElement : ConfigurationElement
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

    }
}

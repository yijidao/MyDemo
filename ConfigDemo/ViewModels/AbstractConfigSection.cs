using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigDemo.ViewModels
{
    public abstract class AbstractConfigSection : ConfigurationSection
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
    }
}

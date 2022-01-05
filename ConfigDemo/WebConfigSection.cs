using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigDemo
{
    public class WebConfigSection : ConfigurationSection
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

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get => (string)this["url"];
            set => this["url"] = value;
        }

        [ConfigurationProperty("paths", IsRequired = true, IsDefaultCollection = false)]
        public CommonConfigElementCollection Paths => (CommonConfigElementCollection)this["paths"];
    }

    

   
}

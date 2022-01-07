using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigDemo.ViewModels
{
    public class WebConfigSection : CommonConfigSection
    {
        [ConfigurationProperty("paths", IsRequired = true, IsDefaultCollection = false)]
        public WebConfigElementCollection Paths => (WebConfigElementCollection)this["paths"];

        public override IEnumerator GetEnumerator()
        {
            //return Paths.GetEnumerator();
            return new WebConfigSectionEnumerator(Settings.GetEnumerator(), Paths.GetEnumerator());
        }

        private class WebConfigSectionEnumerator : IEnumerator
        {
            private readonly IEnumerator _enumerator;
            private readonly IEnumerator _enumerator2;

            public WebConfigSectionEnumerator(IEnumerator enumerator, IEnumerator enumerator2)
            {
                _enumerator = enumerator;
                _enumerator2 = enumerator2;
            }

            public bool MoveNext()
            {
                var result = _enumerator.MoveNext();
                if (result)
                {
                    Current = _enumerator.Current;
                    return true;
                }

                result = _enumerator2.MoveNext();
                Current = _enumerator2.Current;
                return result;
            }

            public void Reset()
            {
                _enumerator.Reset();
                _enumerator2.Reset();
            }

            public object Current { get; private set; }
        }
    }

    public class WebConfigElementCollection : ConfigurationElementCollection
    {

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get => (string)this["url"];
            set => this["url"] = value;
        }

        protected override ConfigurationElement CreateNewElement() => new CommonConfigElement();

        protected override object GetElementKey(ConfigurationElement element) => ((CommonConfigElement)element).Key;
    }
}

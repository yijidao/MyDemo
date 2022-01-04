using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigDemo
{
    public class ModuleSection : ConfigurationSection, IModule
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false)]
        public ModuleCollection Modules => (ModuleCollection)base[""];
    }

    public interface IModule
    {
        ModuleCollection Modules { get; }
    }

    public class ModuleCollection : ConfigurationElementCollection
    {
        public ModuleCollection()
        {
        }

        public ModuleCollection(ModuleElement[] wholeViewElements)
        {
            if (wholeViewElements == null)
                throw new ArgumentNullException(nameof(wholeViewElements));
            foreach (ModuleElement item in wholeViewElements)
            {
                BaseAdd(item);
            }
        }

        protected override bool ThrowOnDuplicate => true;
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;
        protected override string ElementName => "Module";

        protected override ConfigurationElement CreateNewElement()
        {
            return new Module();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Module)element).Name;
        }

        public new Module this[string name] => (Module)BaseGet(name);

        public void Add(Module wholeViewElement)
        {
            BaseAdd(wholeViewElement);
        }

        public bool Contains(string wholeViewname)
        {
            return base.BaseGet(wholeViewname) != null;
        }
    }

    public class Module : ConfigurationElement
    {
        public Module()
        {
        }

        public Module(string name, string value)
        {
            base["name"] = name;
            base["value"] = value;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => (string)base["name"];

        [ConfigurationProperty("value", IsRequired = false)]
        public string Value => (string)base["value"];

        /// <summary>
        /// 是否属于线网
        /// </summary>
        [ConfigurationProperty("isNet", IsRequired = false, DefaultValue = true)]
        public bool IsNet => (bool)base["isNet"];

        /// <summary>
        /// 是否属于线路
        /// </summary>
        [ConfigurationProperty("isLine", IsRequired = false, DefaultValue = true)]
        public bool IsLine => (bool)base["isLine"];

        /// <summary>
        /// 是否属于车站
        /// </summary>
        [ConfigurationProperty("isStation", IsRequired = false, DefaultValue = true)]
        public bool IsStation => (bool)base["isStation"];

        [ConfigurationProperty("authority", IsRequired = true)]
        public int Authority => (int)base["authority"];

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ModuleElementCollection ModuleElements => (ModuleElementCollection)base[""];
    }

    public class ModuleElementCollection : ConfigurationElementCollection
    {
        public ModuleElementCollection()
        {
        }

        public ModuleElementCollection(ModuleElement[] wholeViewChildrenElements)
        {
            if (wholeViewChildrenElements == null)
                throw new ArgumentNullException(nameof(wholeViewChildrenElements));
            foreach (ModuleElement item in wholeViewChildrenElements)
            {
                BaseAdd(item);
            }
        }

        protected override bool ThrowOnDuplicate => true;
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;
        protected override string ElementName => "ModuleElement";

        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleElement)element).Name;
        }

        /// <summary>
        /// 获取所有键
        /// </summary>
        public IEnumerable<string> AllKeys => BaseGetAllKeys().Cast<string>();

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new ModuleElement this[string name] => (ModuleElement)BaseGet(name);

        public void Add(Module wholeViewElement)
        {
            BaseAdd(wholeViewElement);
        }

        public bool Contains(string wholeViewname)
        {
            return base.BaseGet(wholeViewname) != null;
        }
    }

    public class ModuleElement : ConfigurationElement
    {
        public ModuleElement()
        {
        }

        public ModuleElement(string name, string value)
        {
            base["name"] = name;
            base["value"] = value;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => (string)base["name"];

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value => (string)base["value"];

        [ConfigurationProperty("number", IsRequired = false)]
        public string Number => (string)base["number"];

        /// <summary>
        /// 是否属于线网
        /// </summary>
        [ConfigurationProperty("isNet", IsRequired = false, DefaultValue = true)]
        public bool IsNet => (bool)base["isNet"];

        /// <summary>
        /// 是否属于线路
        /// </summary>
        [ConfigurationProperty("isLine", IsRequired = false, DefaultValue = true)]
        public bool IsLine => (bool)base["isLine"];

        /// <summary>
        /// 是否属于车站
        /// </summary>
        [ConfigurationProperty("isStation", IsRequired = false, DefaultValue = true)]
        public bool IsStation => (bool)base["isStation"];
    }
}

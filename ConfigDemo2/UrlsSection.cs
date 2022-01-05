namespace ConfigDemo2;
using System.Configuration;

public class UrlsSection : ConfigurationSection
{

    [ConfigurationProperty("name", DefaultValue = "MyFavorites", IsRequired = true, IsKey = false)]
    public string Name
    {
        get => (string)this["name"];
        set => this["name"] = value;
    }

    [ConfigurationProperty("simple")]
    public UrlConfigElement Simple => (UrlConfigElement)this["simple"];

    [ConfigurationProperty("urls", IsDefaultCollection = false)]
    public UrlsCollection Urls => (UrlsCollection)this["urls"];
}

public class UrlsCollection : ConfigurationElementCollection
{
    protected override ConfigurationElement CreateNewElement() => new UrlConfigElement();

    protected override object GetElementKey(ConfigurationElement element) => ((UrlConfigElement)element).Name;
}

public class UrlConfigElement : ConfigurationElement
{
    [ConfigurationProperty("name", DefaultValue = "HJMos", IsRequired = true, IsKey = true)]
    public string Name
    {
        get => (string)base["name"];
        set => base["name"] = value;
    }

    [ConfigurationProperty("url", DefaultValue = "hjmos.com", IsRequired = true)]
    public string Url
    {
        get => (string)base["url"];
        set => base["url"] = value;
    }

    [ConfigurationProperty("port", DefaultValue = 6666, IsRequired = true)]
    public int Port
    {
        get => (int)base["port"];
        set => base["port"] = value;
    }
}
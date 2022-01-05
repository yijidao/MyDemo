// See https://aka.ms/new-console-template for more information

using System.Configuration;
using ConfigDemo2;

Console.WriteLine("Hello, World!");
var path = Path.Combine(Environment.CurrentDirectory, "demo1.config");
//var map = new ExeConfigurationFileMap(path);
var map = new ExeConfigurationFileMap
{
    ExeConfigFilename = path
};

var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

if (config == null)
{
    Console.WriteLine("config file is null");
    return;
}

var sections = config.Sections;
foreach (ConfigurationSection section in sections)
{
    if (section is UrlsSection urlsSection)
    {
        PrintConfig(urlsSection);
    }
}

void PrintConfig(UrlsSection urlsSection)
{
   

    Console.WriteLine("The simple element of config file:");
    Console.WriteLine($"Name = {urlsSection.Simple.Name} URL = {urlsSection.Simple.Url} Port = {urlsSection.Simple.Port}");

    Console.WriteLine("The urls collection of config file:");
    foreach (UrlConfigElement urlElement in urlsSection.Urls)
    {
        Console.WriteLine($"Name = {urlElement.Name} URL = {urlElement.Url} Port = {urlElement.Port}");
    }

}

Console.ReadLine();




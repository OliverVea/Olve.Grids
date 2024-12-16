using Olve.Grids.IO.Configuration.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Olve.Grids.IO.Configuration;

public class ConfigurationModelFileReader
{
    private static readonly IDeserializer Deserializer = new StaticDeserializerBuilder(
        new YamlStaticContext()
    )
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public OneOf<ConfigurationModel, FileParsingError> Read(string filePath)
    {
        if (!filePath.EndsWith(".yml") && !filePath.EndsWith(".yaml"))
        {
            return FileParsingError.New("File must be a YAML file: {0}", filePath);
        }

        if (!File.Exists(filePath))
        {
            return FileParsingError.New("File not found: {0}", filePath);
        }

        var fileContent = File.ReadAllText(filePath);

        try
        {
            var configuration = Deserializer.Deserialize<ConfigurationModel>(fileContent);
            return configuration;
        }
        catch (YamlException e)
        {
            return FileParsingError.New("Error parsing YAML file: {0}", e.Message);
        }
    }
}
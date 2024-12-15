using Olve.Grids.IO.Configuration.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Olve.Grids.IO.Configuration;

[YamlStaticContext]
[YamlSerializable(typeof(ConfigurationModel))]
public partial class YamlStaticContext;

internal class ConfigurationModelFileReader(string filePath)
{ 
    private static readonly IDeserializer Deserializer = new StaticDeserializerBuilder(new YamlStaticContext())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();
    
    private readonly Lazy<OneOf<ConfigurationModel, FileParsingError>> _configuration = new(() => ReadConfigurationFile(filePath));
    
    public OneOf<ConfigurationModel, FileParsingError> Read() => _configuration.Value;
    
    private static OneOf<ConfigurationModel, FileParsingError> ReadConfigurationFile(string filePath)
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
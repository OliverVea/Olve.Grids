using Olve.Grids.IO.Configuration.Models;
using YamlDotNet.Serialization;

namespace Olve.Grids.IO.Configuration;

[YamlStaticContext]
[YamlSerializable(typeof(ConfigurationModel))]
[YamlSerializable(typeof(AdjacencyModel))]
[YamlSerializable(typeof(AdjacentModel))]
[YamlSerializable(typeof(WeightModel))]
public partial class YamlStaticContext;

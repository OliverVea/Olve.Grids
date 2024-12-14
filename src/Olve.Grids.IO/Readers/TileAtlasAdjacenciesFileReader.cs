using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Olve.Grids.IO.Readers;

/*
generateFromBrushes: true
adjacencies:
  - tile: 16
    overwriteBrushAdjacencies:
      - right
    adjacents:
      - tile: 17
        direction: right
  - tile: 17
    overwriteBrushAdjacencies:
      - right
    adjacents:
      - tile: 16
        direction: left
 */

public class TileAtlasAdjacenciesFileReader(string filePath)
{
    public class FileContent
    {
        public bool GenerateFromBrushes { get; set; } = true;
        public FileAdjacency[]? Adjacencies { get; set; } = [];

        public class FileAdjacency
        {
            // Example: 16
            public int Tile { get; set; }

            // Example: ['right', 'left']
            public string[] OverwriteBrushAdjacencies { get; set; } = [];
            public FileAdjacent[] Adjacents { get; set; } = [];
        }

        public class FileAdjacent
        {
            // Example: 17
            public int Tile { get; set; }

            // If true, set adjacency. Otherwise, clear it.
            public bool IsAdjacent { get; set; } = true;

            // Example: 'right'
            public string Direction { get; set; }
        }
    }

    public IAdjacencyLookupBuilder Read(
        IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> brushLookup
    )
    {
        var builder = new AdjacencyLookup();

        var fileContent = File.ReadAllText(filePath);

        var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(new CamelCaseNamingConvention())
            .Build();

        var content = yamlDeserializer.Deserialize<FileContent>(fileContent);

        if (content.GenerateFromBrushes)
        {
            var adjacencyEstimator = new AdjacencyFromTileBrushEstimator();
            adjacencyEstimator.SetAdjacencies(builder, brushLookup);
        }

        foreach (var adjacency in content.Adjacencies ?? [])
        {
            var tile = new TileIndex(adjacency.Tile);

            foreach (var overwriteBrushAdjacency in adjacency.OverwriteBrushAdjacencies)
            {
                var direction = Enum.Parse<AdjacencyDirection>(overwriteBrushAdjacency, true);

                var neighborsInDirection = builder.GetNeighborsInDirection(tile, direction);

                foreach (var neighbor in neighborsInDirection)
                {
                    builder.Remove(tile, neighbor, direction);
                }
            }
        }

        foreach (var adjacency in content.Adjacencies ?? [])
        {
            var tile = new TileIndex(adjacency.Tile);

            foreach (var adjacent in adjacency.Adjacents)
            {
                var neighbor = new TileIndex(adjacent.Tile);
                var direction = Enum.Parse<AdjacencyDirection>(adjacent.Direction, true);

                if (adjacent.IsAdjacent)
                {
                    builder.Add(tile, neighbor, direction);
                }
                else
                {
                    builder.Remove(tile, neighbor, direction);
                }
            }
        }

        return builder;
    }
}

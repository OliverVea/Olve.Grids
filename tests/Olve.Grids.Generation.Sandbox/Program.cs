using Olve.Grids;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Generation.Generation;
using Olve.Grids.Generation.Sandbox;
using Olve.Grids.Generation.TileAtlas;
using Olve.Utilities.Types;

const string imageFile = "./data/tileatlas_test_01.png";
const string brushFile = "./data/brush-grid_test_01.txt";

const string outputFile = "./output.png";

var tileSize = new Size(4, 4);

var tileAtlasBuilder = TileAtlasBuilder.Create(imageFile);

foreach (var (tileIndex, cornerBrushes) in TileBrushes.AllBrushes)
{
    tileAtlasBuilder.BrushLookupBuilder.SetCornerBrushes(tileIndex, cornerBrushes);
}

tileAtlasBuilder = tileAtlasBuilder.WithTileSize(tileSize);

var tileAtlas = tileAtlasBuilder.Build();

/*
foreach (var (from, to, direction) in TileAdjacencies.Adjacencies)
{
    tileAtlas.AdjacencyLookup[from, to] = direction;
}
*/

var adjacencyEstimator = new AdjacencyFromTileBrushEstimator();
adjacencyEstimator.SetAdjacencies(tileAtlas.AdjacencyLookup, tileAtlas.BrushLookup);

var fileLines = File.ReadAllLines(brushFile);

var brushGridSize = new Size(fileLines.FirstOrDefault()?.Length ?? 0, fileLines.Length);

var brushGrid = new BrushGrid(brushGridSize);

foreach (var (y, line) in fileLines.Index())
{
    foreach (var (x, c) in line.Index())
    {
        var position = new Position(x, y);
            
        OneOf.OneOf<BrushId, Any> brushId = c switch
        {
            '_' => BrushIds.Light,
            '?' => new Any(),
            _ => BrushIds.Dark,
        };

        brushGrid.SetBrush(position, brushId);
    }
}

var request = new GenerationRequest(tileAtlas, brushGrid);
var operation = new GenerationOperation();

var result = operation.Execute(request);

var visualizationExporter = new VisualizationExporter();
visualizationExporter.ExportAsPng(result, outputFile);

Console.WriteLine($"Got result {result}");
Console.WriteLine($"Exported to {outputFile}");

using Olve.Grids;
using Olve.Grids.Adjacencies;
using Olve.Grids.Generation;
using Olve.Grids.Generation.Generation;
using Olve.Grids.Generation.Sandbox;
using Olve.Grids.Generation.TileAtlas;

const string imageFile = "./data/tileatlas_test_01.png";
const string brushLookupFile = "./data/brushes_test_01.txt";
const string brushGridFile = "./data/brush-grid_test_01.txt";

const string outputFile = "./output.png";

var tileSize = new Size(4, 4);

var tileAtlasBuilder = TileAtlasBuilder
    .Create(imageFile)
    .WithTileSize(tileSize)
    .ReadBrushLookupFromFile(brushLookupFile);
    
var brushGrid = tileAtlasBuilder.ReadBrushGridFromFile(brushGridFile);

var tileAtlas = tileAtlasBuilder.Build();

var adjacencyEstimator = new AdjacencyFromTileBrushEstimator();
adjacencyEstimator.SetAdjacencies(tileAtlas.AdjacencyLookup, tileAtlas.BrushLookup);

var request = new GenerationRequest(tileAtlas, brushGrid);
var operation = new GenerationOperation();

var result = operation.Execute(request);

var visualizationExporter = new VisualizationExporter();
visualizationExporter.ExportAsPng(result, outputFile);

Console.WriteLine($"Got result {result}");
Console.WriteLine($"Exported to {outputFile}");

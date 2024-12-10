using System.Diagnostics;
using Olve.Grids;
using Olve.Grids.Generation;
using Olve.Grids.Generation.Generation;
using Olve.Grids.Generation.Sandbox;
using Olve.Grids.Generation.TileAtlas;

var imageFile = "./tilesets/tileatlas_test_01.png";
var tileSize = new Size(4, 4);

var tileAtlasBuilder = TileAtlasBuilder.Create(imageFile);

tileAtlasBuilder = tileAtlasBuilder.WithTileSize(tileSize);

while (true)
{
    var tileAtlas = tileAtlasBuilder.Build();

    foreach (var (from, to, direction) in TileAdjacencies.Adjacencies)
    {
        tileAtlas.AdjacencyLookup[from, to] = direction;
    }

    var request = new GenerationRequest(tileAtlas, (8, 8));
    var operation = new GenerationOperation();

    var result = operation.Execute(request);

    var visualizationExporter = new VisualizationExporter();
    visualizationExporter.ExportAsPng(result, "./output.png");

    await Task.Delay(TimeSpan.FromMilliseconds(200));
}

# Olve.Grids.IO

## Overview

`Olve.Grids.IO` is a library for handling various grid-related operations, including reading brush grids, exporting visualizations, and loading configuration data for grids. It provides functionality for serializing and deserializing grid configurations, handling tile atlases, and performing various configuration loading and parsing tasks. The library is designed to be part of a larger grid management and visualization system.

## Key Features

- **Grid Configuration**: Define and load grid configurations with support for adjacencies, tile groups, and weights.
- **Visualization Export**: Export grid visualizations as PNG files using a tile atlas image.
- **Tile Atlas Builder**: Configure and build tile atlases with support for tile size, columns, rows, and more.
- **Brush Grid Reader**: Load and parse brush grids from files.
- **Adjacency & Weight Configuration**: Configure adjacency rules and weight configurations for grid elements.

## Directory Structure

### 1. **Core Functionality**

- `FileIOConstants.cs`: Constants for file operations, such as `AnyBrushChar`.
- `VisualizationExporter.cs`: Export grid visualizations as PNG files.

### 2. **Configuration**

- **Adjacency Configuration**: Classes to manage adjacencies between grid elements.
    - `AdjacencyConfiguration.cs`: Contains adjacencies and related logic.
    - `AdjacencyConfigurationLoader.cs`: Loads and configures adjacency lookup tables.
    - `AdjacencyConfigurationParser.cs`: Parses adjacency configuration files.
- **Tile Groups**: Grouping of tiles for easier management.
    - `TileGroups.cs`: A container for tile groups.
    - `TileGroupParser.cs`: Parses tile group definitions.
- **Weight Configuration**: Manage weights associated with tiles.
    - `WeightConfiguration.cs`: Define weight configurations.
    - `WeightConfigurationLoader.cs`: Loads and configures weight lookup.
- **Configuration Model**: Defines the structure of the grid configuration file.
    - `ConfigurationModel.cs`: The model of the configuration file.
    - `ConfigurationLoader.cs`: Loads the complete configuration.
    - `ConfigurationModelFileReader.cs`: Reads configuration model files.

### 3. **Tile Atlas Builder**

- `TileAtlasBuilder.cs`: Build tile atlases based on a configuration.
- `TileAtlasConfiguration.cs`: Defines tile atlas configuration options.
- `SerializableTileAtlasConfiguration.cs`: Serialize tile atlas configurations.
- `ImageSizeHelper.cs`: Helper methods for extracting image sizes.
- `TileAtlasConfigurationValidator.cs`: Validates tile atlas configurations.

### 4. **File Readers**

- `InputBrushFileReader.cs`: Reads brush grids from files.
- `TileAtlasBrushesFileReader.cs`: Loads brushes for tile atlases from files.

### 5. **Parsing**

- `IParser.cs`: Interface for parsing different configuration models.
- `DirectionParser.cs`: Parses direction information for adjacencies.
- `TileIndexParser.cs`: Parses tile index data.
- `GroupParser.cs`: Parses tile group data.

## Usage

### Example: Exporting a Grid as PNG

```csharp
var exporter = new VisualizationExporter();
var generationResult = new GenerationResult(); // Your generation result
var path = "path/to/output.png";
var tileAtlasImage = new Image(); // Your tile atlas image
exporter.ExportAsPng(generationResult, path, tileAtlasImage);
```

## Configuration Loading Example

```csharp
var loader = ConfigurationLoader.Create();
var configuration = loader.Load("path/to/config.yaml", adjacencyLookup, weightLookup, tileIndices, brushConfiguration);
```
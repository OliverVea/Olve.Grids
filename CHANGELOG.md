## v1.5.0 (2025-02-01)

### Feat

- adding barebones support for generation in `project/{projectId}/generate` page
- adding layout to generate page
- adding title bar to generate page
- adding `project/[projectId}/generate` page

### Fix

- fixing overflow of tile grid in project dashboard
- adding `overflow-hidden` to `MainLayout`
- ensuring that `MainLayout` cannot exceed screen

### Refactor

- adding `AdjacentModel.Adjacency` directly to `AdjacentModel` in `DeBroglieGenerator`
- added additional tailwind classes

## v1.4.0 (2025-01-31)

### Feat

- **UI**: added support for toggling locked sides for each tile through the UI
- added `SetLockedSideOperation` to set a locked side for a project
- adding `Checkbox` component
- adding `Direction.ToSide` method

### Refactor

- refactored tailwind classes of `Button` component to be an `IEnumerable<TailwindClass>` method for improved readability
- renaming `Project.LastAccessedAt` to `Project.LastChangedAt`
- renaming `EstimateAdjacenciesFromBrushesCommand` to `EstimateAdjacenciesFromBrushesOperation` to fit with the `IOperation` interface

## v1.3.0 (2025-01-31)

### Feat

- adjacencies are automatically calculated based on brushes
- `Project` now contains a set of locked tiles
- `EstimateAdjacenciesFromBrushesCommand` now enforces `ToUpdate` and `ToNotUpdate`
- adding `Direction.IsCardinal()`

### Fix

- fixing adjacency preview layout
- **UI**: improving adjacency preview layout and preview image automatic sizing

### Refactor

- adding `Aspect` tailwind class
- adding `Direction.X` and `Direction.Y` composite directions
- adding constructor to `BrushLookup` for `IEnumerable<(TileIndex, CornerBrushes)>`
- removed unecessary usings
- removing unused constructor from `WeightLookup`

### Perf

- using `GetOrAdd` in `EstaimateAdjacencicesFromBrushesCommand`

## v1.2.2 (2025-01-30)

### Refactor

- adding and using `TileWeight` and `TileWeights`
- adding and using `TileAdjacency` and `TileAdjacencies`
- creating and using `TileBrush` and `TileBrushes`

## v1.2.1 (2025-01-30)

### Fix

- `GetTiles(BrushId brushId, Corner corner)` now uses the corner from the perspective of the brush and not the tile as expected
- fixing bug in `items` not being parsed properly by `AdjacencyLookup` when the lookup was constructed, potentially resulting in an invalid state

### Refactor

- naming elements in `_tileCornerToBrush` tuple
- add `Directions.All` for all possible directions
- renaming `Directions.All` to `Directions.Cardinal`

### Perf

- using `TryAsReadOnlySet` in `EstimateAdjacenciesFromBrushesCommand` to reduce allocations

## v1.2.0 (2025-01-29)

### Feat

- improving inactive rendering of adjacency tiles
- **UI**: allow for setting tile adjacency
- added a static `Directions` class for enumerating directions (up, down, left, right)
- added `Toggle` to `this IAdjacencyLookup` to toggle if a neighbor is in a direction with a method
- edit adjacencies page gets `TileInformation` from parent
- edit adjacencies page gets `TileInformation` from parent
- edit adjacencies page gets `TileInformation` from parent

### Fix

- using new `EstimateAdjacenciesFromBrushesCommand` in `AdjacencyConfigurationLoader`
- `ProjectDashboardContainer.Provider` can now be null if the current page is not the project dashboard
- `VerticalDivider` now functions as expected
- renaming `FlexGrow` to `Grow` and setting correct tailwind class
- fixed incorrect serialization of `GridConfiguration`

### Refactor

- changing `AdjacencyFromTileBrushEstimator` to `EstimateAdjacenciesFromBrushesCommand` and adding a `LockedAdjacencies` enumerable to exclude `TileIndex`-`Direction` pairs from the estimation
- added `Direction.ToChevron` extension method
- adding tailwind cursor classes
- refactored tile components to support `AdjacencyTile`
- added various tailwind utility classes to `TailwindExtensions´
- changed name from `AdjacencyDirectionExtensions` to `DirectionExtensions`
- removing unnecessary usings

### Perf

- `GridConfiguration.GetTileIndices` only calculate ´tileCount´ once

## v1.1.4 (2025-01-22)

### Refactor

- adding green and blue colors to Colors.cs
- refactoring page naming and unpacking ProjectDashboard to new folder and namespace

## v1.1.3 (2025-01-17)

### Fix

- bumped version number

## v0.0.1 (2025-01-17)

### Refactor

- using `Olve.Utilities` `v0.2.0`
- splitting into separate files, renaming serialized -> serializable

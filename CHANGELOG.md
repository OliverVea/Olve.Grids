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

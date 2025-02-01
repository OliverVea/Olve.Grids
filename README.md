<div align="center">

![Grids Banner](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/grids-banner.gif)

[![NuGet](https://img.shields.io/nuget/v/Olve.Grids?logo=nuget)](https://www.nuget.org/packages/Olve.Grids)[![GitHub](https://img.shields.io/github/license/OliverVea/Olve.Grids)](LICENSE)![LOC](https://img.shields.io/endpoint?url=https%3A%2F%2Fghloc.vercel.app%2Fapi%2FOliverVea%2FOlve.Grids%2Fbadge)![NuGet Downloads](https://img.shields.io/nuget/dt/Olve.Grids)


</div>

# Olve.Grids

## Table of contents

- [Purpose](#purpose)
- [Overview](#overview)

## Purpose

The purpose of this library is to define rules for a tile atlas and then procedurally generate outputs given some input geometry.

It handles a specific problem, i.e. generating valid variations of a tile atlas given some input geometry. It assumes a grid-based system where each cell can contain a tile from a tile atlas.

## Overview

* **[Olve.Grids](src/Olve.Grids)** - The core library.
* **[Olve.Grids.DeBroglie](src/Olve.Grids.DeBroglie)** - Generation of using the [DeBroglie](https://github.com/BorisTheBrave/DeBroglie) library.
* **[Demo](src/Demo)** - A CLI application showcasing the library.

Please see the individual projects for more information.

## Todo

### `Olve.Utilities`

- Build bespoke constraint solver and remove dependency to `DeBroglie`
- Add `alias` tiles

### `UI.Blazor` and `UI.Core`

- Add generation using configured `TileAtlas`
- Allow export of `SerializedTileAtlas`
- Improve UI QOL

### Other

- Create algorithm to guess tile corner brush based on other tile corners
- Fix various `TODO`s in the code
- Add description to GitHub repo
- Rewrite [`README.md`](README.md)
- Add XML documentation to public libraries
- Public DocFX documentation on GitHub pages
- Include `Meziantou.Analyzer` in `Directory.Build.props`
- Publish CLI demo application on new releases
- Publish UI application on new releases
- Broaden test coverage

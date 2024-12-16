# Olve.Grids [![NuGet](https://img.shields.io/nuget/v/Olve.Grids?logo=nuget)](https://www.nuget.org/packages/Olve.Grids) [![GitHub](https://img.shields.io/github/license/OliverVea/Olve.Grids)](LICENSE)

## Purpose

The purpose of this library is to define rules for a tile atlas and then procedurally generate outputs given some input
geometry.

It handles a specific problem, i.e. generating valid variations of a tile atlas given some input geometry.
It assumes a grid-based system where each cell can contain a tile from a tile atlas.

## Overview

* **[Olve.Grids](src/Olve.Grids)** - The core library.
* **[Olve.Grids.DeBroglie](src/Olve.Grids.DeBroglie)** - Generation of using
  the [DeBroglie](https://github.com/BorisTheBrave/DeBroglie) library.
* **[Demo](src/Demo)** - A CLI application showcasing the library.

Please see the individual projects for more information.

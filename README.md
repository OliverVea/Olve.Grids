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

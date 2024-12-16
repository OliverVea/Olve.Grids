# grid-demo

This is a demo CLI application showcasing the [Olve.Grids](https://github.com/OliverVea/Olve.grids) nuget package.

## How to use this demo

Run the `grid-demo` executable with the following arguments:

### Tile atlas

A tile atlas is a `.png` file that contains all the tiles that can be used in the output. The tiles are arranged in a grid, where each tile is the same size.

An example tile atlas is provided in the `Demo` directory.

![Tiles in the tile atlas](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/upscaled-atlas.png)

It has 18 tiles of an even 4x4 pixel size:

![Tiles in the tile atlas](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/upscaled-grid-atlas.png)

### Tile atlas brushes

As this library is based on the dual grid system, each tile is assigned four different brushes, each brush corresponding to a corner of the tile.

For example, tile 0 from the tile atlas has the following brushes:

```txt
DD
DL
```

Where `D` and `L` are the brush of the light and dark color respectively. To better understand this format, here's an illustration of what a tile with this configuration might look like:

![Example of tile with configuration overlaid](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/atlas-overlay-tile1.png)

The brushes for the atlas must be defined in a file and provided to the application along with the tile atlas.

Here is an example brush file `./assets/tile-atlas.brushes.txt` for the tile atlas above:

```txt
DD  DD  DD  LL  LL  DL
DL  LL  LD  LD  DL  LD

DL  LL  LD  LD  DL  LD
DL  LL  LD  LL  LL  DL

DL  LL  LD  DD  DD  QQ
DD  DD  DD  DD  DD  QQ
```

The configuration corresponds to the tile atlas:

![Example of tile with configuration overlaid](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/atlas-overlay.png)

Whitespace can be used to separate each tile like above, but are simply ignored by the application.

### Input brushes

The input brushes are the brushes that are used to generate the output. These brushes must be defined in a file and provided to the application.

Here is an example input brush file:

```txt
DDDDDDDDDDD
D?????????D
D?????????D
D?????????D
D?????????D
D????????DD
D??????DDLD
D????DDLDLD
D???DLDLDLD
DDDDDDDDDDD
```

Here, each cell describes a brush, where `D` is the dark color, `L` is the light color, and `?` is a wildcard that can be any color.

The characters are retrieved from the tile atlas brushes file above.

### Example

Here is an example command to generate an output:

```bash
grid-demo --tile-atlas "tile-atlas.png" --tile-atlas-brushes "tile-atlas.brushes.txt" --input-brushes "input.brushes.txt" --output "output.png"
```

This will generate an output based on the input brushes and the tile atlas.

For example, the input brushes above will generate the following output:

![Examples of outputs with the earlier tileset and brush input](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/output-examples.gif)

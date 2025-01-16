# grid-demo

This is a demo CLI application showcasing the [Olve.Grids](https://github.com/OliverVea/Olve.grids) nuget package. It can be downloaded in the newest [release](https://github.com/OliverVea/Olve.Grids/releases/latest).

## Table of Contents

- [How to use this demo](#how-to-use-this-demo)
    - [Tile atlas](#tile-atlas)
    - [Tile atlas brushes](#tile-atlas-brushes)
    - [Input brushes](#input-brushes)
    - [Example](#example)
- [Commands](#commands)
    - [`run`](#run)
        - [Options](#options)
        - [Example Usage](#example-usage)
    - [`pack`](#pack)
        - [Options](#options-1)
        - [Example Usage](#example-usage-1)
    - [`runpacked`](#runpacked)
        - [Arguments](#arguments)
        - [Options](#options-2)
        - [Example Usage](#example-usage-2)

## How to use this demo

Run the `grid-demo` executable with the following arguments:

### Tile atlas

A tile atlas is a `.png` file that contains all the tiles that can be used in the output. The tiles are arranged in a grid, where each tile is the same size.

An example tile atlas is provided in the `Demo` directory.

![Tiles in the tile atlas](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/upscaled-atlas.png)

It has tiles of an even 4x4 pixel size:

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

DL  LL  LD  DD  DD  DD
DD  DD  DD  DD  DD  DD

DD  DD  DD  DD  WW  DW
DW  WW  WD  DD  WW  WD

DW  WW  WD  WW  WW  WD
DW  WW  WD  WD  DW  DW

DW  WW  WD  WD  DW  QQ
DD  DD  DD  WW  WW  QQ
```

The configuration corresponds to the tile atlas:

![Example of tile with configuration overlaid](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/atlas-overlay.png)

Whitespace can be used to separate each tile like above, but are simply ignored by the application.

### Input brushes

The input brushes are the brushes that are used to generate the output. These brushes must be defined in a file and provided to the application.

Here is an example input brush file:

```txt
W????DDDDDDDDDDDDDDD
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W??????????????????D
W???????????????????
W???????????????????
W???????????????????
W???????????????????
WWWWWWWWWWWWWWWWWWWW
```

Here, each cell describes a brush, where `D` is the dark color, `L` is the light color, and `?` is a wildcard that can be any color.

The characters are retrieved from the tile atlas brushes file above.

### Example

Here is an example command to generate an output:

```bash
grid-demo run --tile-atlas "tile-atlas.png" --tile-atlas-brushes "tile-atlas.brushes.txt" --input "input.brushes.txt" --output "output.png"
```

This will generate an output based on the input brushes and the tile atlas.

For example, the input brushes above will generate the following output:

![Example output](https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/docs/imgs/example-output.png)

### `run`

The `run` command generates an output image based on the provided tile atlas, tile atlas brushes, and input brushes. Below are the available options for the `run` command:

#### Options

- **`-a, --tile-atlas`**  
  The tile atlas file. This is a `.png` file that contains all the tiles used in the output.  
  **Default:** `assets/tile-atlas.png`

- **`-s, --tile-size`**  
  The size of each tile in the tile atlas, specified as `widthxheight`.  
  **Default:** `4x4`

- **`-b, --tile-atlas-brushes`**  
  The tile atlas brushes file containing brush definitions for each tile in the tile atlas.  
  **Default:** `assets/tile-atlas.brushes.txt`

- **`-c, --tile-atlas-config`**  
  An optional configuration file for additional tile atlas settings. If not provided, the application will proceed without additional configurations.  
  **Default:** None

- **`-i, --input`**  
  The input file containing brushes used to generate the output.  
  **Default:** `assets/input.brushes.txt`

- **`-o, --output`**  
  The file path for the generated output image. The file must have a `.png` extension.  
  **Default:** `output.png`

- **`-v, --verbosity`**  
  The verbosity level of the command output. Available values are:
    - `Quiet`: Suppresses most output.
    - `Normal`: Shows basic output, including progress messages.
    - `Verbose`: Provides detailed output, including generation times and additional debug information.  
      **Default:** `Normal`

- **`--overwrite`**  
  Allows overwriting the output file if it already exists.  
  **Default:** Disabled (output file will not be overwritten unless this flag is provided).

#### Example Usage

```bash
grid-demo run --tile-atlas "tile-atlas.png" --tile-size "8x8" --tile-atlas-brushes "tile-atlas.brushes.txt" --input "input.brushes.txt" --output "output.png" --verbosity "Verbose" --overwrite
```

### `pack`

The `pack` command generates a packed tile atlas based on the provided tile atlas image, tile atlas brushes, and optional configuration file. Below are the available options for the `pack` command:

#### Options

- **`-a, --tile-atlas`**  
  The tile atlas file. This is a `.png` file that contains all the tiles to be packed.  
  **Default:** `assets/tile-atlas.png`

- **`-s, --tile-size`**  
  The size of each tile in the tile atlas, specified as `widthxheight`.  
  **Default:** `4x4`

- **`-b, --tile-atlas-brushes`**  
  The tile atlas brushes file containing brush definitions for each tile in the tile atlas.  
  **Default:** `assets/tile-atlas.brushes.txt`

- **`-o, --output`**  
  The file path for the generated packed tile atlas.  
  **Default:** `output.grids`

- **`-v, --verbosity`**  
  The verbosity level of the command output. Available values are:
    - `Quiet`: Suppresses most output.
    - `Normal`: Shows basic output, including progress messages.
    - `Verbose`: Provides detailed output, including additional debug information.  
      **Default:** `Normal`

- **`--overwrite`**  
  Allows overwriting the output file if it already exists.  
  **Default:** Disabled (output file will not be overwritten unless this flag is provided).

- **`-c, --tile-atlas-config`**  
  An optional configuration file for additional tile atlas settings. If not provided, the application will proceed without additional configurations.  
  **Default:** None

#### Example Usage

```bash
grid-demo pack --tile-atlas "tile-atlas.png" --tile-size "8x8" --tile-atlas-brushes "tile-atlas.brushes.txt" --output "packed.grids" --verbosity "Verbose" --overwrite
```

### `runpacked`

The `runpacked` command generates an output image based on a pre-packed tile atlas file, input brushes, and a tile atlas image. Below are the available options and arguments for the `runpacked` command:

#### Arguments

- **`<packed-tile-atlas>`**  
  The packed tile atlas file to use. This file must have a `.grids` extension.  
  **Required**

#### Options

- **`-a, --tile-atlas`**  
  The tile atlas file. This is a `.png` file containing the image data for the tiles used in the packed tile atlas.  
  **Default:** None (must be explicitly specified)

- **`-i, --input`**  
  The input file containing brushes used to generate the output.  
  **Default:** `assets/input.brushes.txt`

- **`-o, --output`**  
  The file path for the generated output image. The file must have a `.png` extension.  
  **Default:** `output.png`

- **`-v, --verbosity`**  
  The verbosity level of the command output. Available values are:
    - `Quiet`: Suppresses most output.
    - `Normal`: Shows basic output, including progress messages.
    - `Verbose`: Provides detailed output, including additional debug information.  
      **Default:** `Normal`

- **`--overwrite`**  
  Allows overwriting the output file if it already exists.  
  **Default:** Disabled (output file will not be overwritten unless this flag is provided).

#### Example Usage

```bash
grid-demo runpacked "packed.grids" --tile-atlas "tile-atlas.png" --input "input.brushes.txt" --output "output.png" --verbosity "Verbose" --overwrite
```

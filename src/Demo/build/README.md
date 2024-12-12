# grid-demo

This is a demo CLI application showcasing the [Olve.Grids](https://github.com/OliverVea/Olve.grids) nuget package.

## How to use this demo

Run the `grid-demo` executable with the following arguments:

### Tile atlas

A tile atlas is a `.png` file that contains all the tiles that can be used in the output. The tiles are arranged in a grid, where each tile is the same size.

An example tile atlas is provided in the `Demo` directory. It has a tile size of 4x4 pixels, and contains 18 tiles:

<div style="position: relative; width: 240px; height: 120px; display: inline-block;">
    <img src="https://raw.githubusercontent.com/OliverVea/Olve.Grids/refs/heads/master/src/Demo/assets/tile-atlas.png" width="240" height="120" style="image-rendering: pixelated; display: block;">
    <div style="position: absolute; top: 0; left: 0; width: 240px; height: 120px; display: grid; grid-template-columns: repeat(6, 2fr); grid-template-rows: repeat(3, 2fr); pointer-events: none;">
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">0</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">1</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">2</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">3</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">4</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">5</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">6</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">7</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">8</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">9</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">10</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">11</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">12</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">13</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">14</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">15</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">16</div>
        <div style="border: 1px solid rgba(255, 255, 255, 0.5);">17</div>
    </div>
</div>

### Tile atlas brushes

As this library is based on the dual grid system, each tile is assigned four different brushes, each brush corresponding to a corner of the tile.

For example, tile 0 from the tile atlas has the following brushes:

```
DD
DL
```

Where `D` and `L` are the brush of the light and dark color respectively.

The brushes for the atlas must be defined in a file and provided to the application along with the tile atlas.

Here is an example brush file for the tile atlas above:

```
DD  DD  DD  LL  LL  DL
DL  LL  LD  LD  DL  LD

DL  LL  LD  LD  DL  LD
DL  LL  LD  LL  LL  DL

DL  LL  LD  DD  DD  QQ
DD  DD  DD  DD  DD  QQ
```

Whitespace can be used to separate each tile like above, but are simply ignored by the application.

### Input brushes

The input brushes are the brushes that are used to generate the output. These brushes must be defined in a file and provided to the application.

Here is an example input brush file:

```
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

### Output

The output is a `.png` file that contains the generated output.
The output is generated base on the input brushes with the corresponding tiles from the tile atlas.

### Example

Here is an example command to generate an output:

```
grid-demo --tile-atlas "tile-atlas.png" --tile-atlas-brushes "tile-atlas.brushes.txt" --input-brushes "input.brushes.txt" --output "output.png"
```

This will generate an output based on the input brushes and the tile atlas.

For example, the input brushes above will generate the following output:

<img src="https://github.com/OliverVea/Olve.Grids/blob/master/src/Demo/assets/output.png?raw=true" width="240" height="220" style="image-rendering: pixelated; display: block;">
using Godot;
using HexMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Extentions
{
    public static void SetCell(this TileMap self, AxialCoord coord, string key)
    {
        var id = self.TileSet.FindTileByName(key);

        var offsetCoord = coord.ToOffsetCoord();

        self.SetCell(offsetCoord.col, offsetCoord.row, id);
    }

    public static void ClearCell(this TileMap self, AxialCoord coord)
    {
        var offsetCoord = coord.ToOffsetCoord();

        self.SetCell(offsetCoord.col, offsetCoord.row, -1);
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Extentions
{
    public static void SetCells(this TileMap self, (int x, int y) index, string key)
    {
        var id = self.TileSet.FindTileByName(key);
        self.SetCell(index.x, index.y, id);
    }
}

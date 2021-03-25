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

    public static Vector2 HexToPixelVector2(this Layout self, AxialCoord coord)
    {
        var point = self.HexToPixel(coord);
        return new Vector2((float)point.x, (float)point.y);
    }

    public static AxialCoord PixelVectorToHex(this Layout self, Vector2 vector2)
    {
        return self.PixelToHex(new Point(vector2.x, vector2.y));
    }

    public static IEnumerable<T> GetChildren<T>(this Node node) where T : Node
    {
        List<T> rslt = new List<T>();
        foreach (var child in node.GetChildren())
        {
            if (child is T)
            {
                rslt.Add(child as T);
            }
        }

        return rslt;
    }

    public static T GetParentRecursively<T>(this Node node) where T : Node
    {
        if(node == null)
        {
            return null;
        }

        var parent = node.GetParent();
        if (parent is T ret)
        {
            return ret;
        }

        return parent.GetParentRecursively<T>();
    }

    public static void EndWith(this IDisposable disposable, Node node)
    {
        var wait = node.ToSignal(node, "tree_exited");
        wait.OnCompleted(disposable.Dispose);

    }
}

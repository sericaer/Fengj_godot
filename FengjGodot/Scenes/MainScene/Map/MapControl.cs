using Fengj.Map;
using Godot;
using HexMath;
using System;

public class MapControl : Control
{
    internal MapData gmObj;

    internal Func<AxialCoord, Vector2> funcCalcPos;

    internal void SetGmObj(MapData mapData)
    {
        gmObj = mapData;
        foreach(var cell in gmObj.cells)
        {
            var label = new Label();
            label.Text = $"{cell.axialCoord.q}-{cell.axialCoord.r}";
            AddChild(label);

            label.SetPosition(funcCalcPos(cell.axialCoord) - label.RectSize/2);
        }
    }
}
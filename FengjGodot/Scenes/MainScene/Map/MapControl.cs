using Fengj.Map;
using Godot;
using HexMath;
using System;
using System.Collections.Generic;
using System.Linq;

public class MapControl : Control
{
	internal MapData gmObj;

	internal Func<AxialCoord, Vector2> funcCalcPos;
	internal Func<Vector2, AxialCoord> funcCalcAxialCoord;

	internal void SetGmObj(MapData mapData)
	{
		gmObj = mapData;
        //foreach(var cell in gmObj.cells)
        {


            var cell = mapData.center;

            AddCellTop(cell);
        }
    }

    private void AddCellTop(ICell cell)
    {
        var cellTop = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/Map/Cell/CellTop.tscn").Instance() as CellTop;
        AddChild(cellTop);

        cellTop.SetGmObj(cell);
        cellTop.ForceUpdateTransform();
        GD.Print(cellTop.GetRect().Size / 2);

        cellTop.SetGlobalPosition(funcCalcPos(cell.axialCoord) - cellTop.GetRect().Size / 2);
    }

    internal void OnViewPortGlobalRectChanged(Rect2 rect)
    {
		var center = rect.Position + rect.Size / 2;

		var coordCenter = funcCalcAxialCoord(center);

		for(int i=0; i<10; i++)
        {
			var coords = coordCenter.GetRing(i);

            var inViewCoords = coords.Where(y => rect.HasPoint(funcCalcPos(y)));
            if(inViewCoords.Count() == 0)
            {
                break;
            }

            foreach(var coord in inViewCoords)
            {
                var cell = gmObj.GetCell(coord);
                AddCellTop(cell);
            }
        }
		
	}
}

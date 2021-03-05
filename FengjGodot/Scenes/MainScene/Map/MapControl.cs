using Fengj.Map;
using Godot;
using HexMath;
using System;
using System.Collections.Generic;

public class MapControl : Control
{
	internal MapData gmObj;

	internal Func<AxialCoord, Vector2> funcCalcPos;

	internal void SetGmObj(MapData mapData)
	{
		gmObj = mapData;
		//foreach(var cell in gmObj.cells)
		{
			//var label = new Label();
			//label.Text = $"{cell.axialCoord.q}-{cell.axialCoord.r}";
			//AddChild(label);

			//label.SetPosition(funcCalcPos(cell.axialCoord) - label.RectSize/2);

			var cell = mapData.center;

			var cellTop = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/Map/Cell/CellTop.tscn").Instance() as CellTop;
			AddChild(cellTop);

			cellTop.SetGmObj(cell);
			cellTop.ForceUpdateTransform();
			GD.Print(cellTop.GetRect().Size / 2);

			cellTop.SetGlobalPosition(funcCalcPos(cell.axialCoord) - cellTop.GetRect().Size / 2);
		}
	}
}

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

			cellTop.SetPosition(funcCalcPos(cell.axialCoord) - cellTop.GetRect().Size/2);

		}
	}

	internal void UpdatePos(Vector2 offsetPosition)
	{

		foreach (var child in this.GetChildren())
		{
			if (child is CellTop)
			{
				var cellTop = child as CellTop;
				cellTop.SetPosition(funcCalcPos(cellTop.gmObj.axialCoord) - cellTop.GetRect().Size / 2);
			}
		}
	}
}

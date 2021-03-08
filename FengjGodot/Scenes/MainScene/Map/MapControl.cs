using Fengj.Map;
using Godot;
using HexMath;
using System;
using System.Collections.Generic;
using System.Linq;

public class MapControl : Control
{

	internal MapData gmObj;

	internal Layout layout { get; set; }

	private Dictionary<(int q, int r), CellTop> cellTopDict;

	public override void _Ready()
	{
		cellTopDict = new Dictionary<(int q, int r), CellTop>();
	}

	internal void SetGmObj(MapData mapData)
	{
		gmObj = mapData;
	}

	private void AddCellTop(ICell cell)
	{
		var cellTop = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/Map/Cell/CellTop.tscn").Instance() as CellTop;
		AddChild(cellTop);

		cellTop.SetGmObj(cell);
		cellTop.ForceUpdateTransform();

		cellTop.SetGlobalPosition(layout.HexToPixelVector2(cell.axialCoord) - cellTop.GetRect().Size / 2);

		cellTopDict.Add((cell.axialCoord.q, cell.axialCoord.r), cellTop);
	}

	internal void OnViewPortGlobalRectChanged(Rect2 rect)
	{
		var center = rect.Position + rect.Size / 2;

		var coordCenter = layout.PixelVectorToHex(center);

		List<(int q, int r)> list = new List<(int q, int r)>();
		for(int i=0; i<10; i++)
		{
			var coords = coordCenter.GetRing(i);

			var inViewCoords = coords.Where(y => rect.HasPoint(layout.HexToPixelVector2(y)));
			if(inViewCoords.Count() == 0)
			{
				GD.Print($"not show {i}");
				break;
			}

			list.AddRange(inViewCoords.Select(x=>(x.q, x.r)));
		}


		var olds = cellTopDict.Keys.ToArray();
		foreach (var needdel in olds.Except(list))
		{
			cellTopDict[needdel].QueueFree();
			cellTopDict.Remove(needdel);
		}

		foreach (var needadd in list.Except(olds))
		{
			AddCellTop(gmObj.GetCell(needadd.q, needadd.r));
		}
	}
}

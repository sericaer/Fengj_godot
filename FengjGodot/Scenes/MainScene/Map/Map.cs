using Fengj;
using Fengj.API;
using Fengj.Map;
using Godot;
using HexMath;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

public class Map : Node2D
{
	internal MapData gmObj;

	public TileMap MaskMap;
	public TileMap terrainMap;
	public TileMap riverMap;

	public override void _Ready()
	{
		MaskMap = GetNode<TileMap>("MaskMap");
		terrainMap = GetNode<TileMap>("TerrainMap");
		riverMap = GetNode<TileMap>("RiverMap");
	}

	internal void SetTerrainTileSet(TileSet tileSet)
	{
		terrainMap.TileSet = tileSet;
	}

	internal void SetCell(ICell cell)
	{
		switch (cell.detectType)
		{
			case DetectType.UN_VISIBLE:
				MaskMap.SetCell(cell.axialCoord, "MASK");
				break;
			case DetectType.VISION_VISIBLE:
				{
					var key = $"{cell.terrainType}_VISION";
					MaskMap.SetCell(cell.axialCoord, key);
				}
				break;
			case DetectType.TERRAIN_VISIBLE:
				{
					MaskMap.ClearCell(cell.axialCoord);
					terrainMap.SetCell(cell.axialCoord, cell.terrainDef.path);

					if (cell.HasComponent(TerrainCMPType.RIVER))
					{
						riverMap.SetCell(cell.axialCoord, "RIVER");
					}
				}
				break;
			default:
				throw new Exception();
		}
	}

	internal void SetGmObj(MapData mapData)
	{
		MaskMap.Clear();
		terrainMap.Clear();
		riverMap.Clear();

		gmObj = mapData;

		SetTerrainTileSet(GlobalResource.tileSet);

		foreach (var cell in gmObj.cells)
		{
			SetCell(cell);
		}

		gmObj.WhenPropertyChanges(x => x.changedCell).Subscribe(y => UpdateCell(y.Value));
	}

	internal void UpdateCell(ICell cell)
	{
		SetCell(cell);
	}
}

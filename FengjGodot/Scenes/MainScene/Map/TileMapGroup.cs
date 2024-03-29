using Fengj;
using Fengj.API;
using Fengj.Map;
using Godot;
using HexMath;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

public class TileMapGroup : Node2D
{
	public TileMap MaskMap;
	public TileMap terrainMap;
	public TileMap riverMap;
	public TileMap selectedCellMap;
		 
	public override void _Ready()
	{
		MaskMap = GetNode<TileMap>("MaskMap");
		terrainMap = GetNode<TileMap>("TerrainMap");
		riverMap = GetNode<TileMap>("RiverMap");
		selectedCellMap = GetNode<TileMap>("SelectedCellMap");

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
		selectedCellMap.Clear();

		SetTerrainTileSet(GlobalResource.tileSet);

		foreach (var cell in mapData.cells)
		{
			SetCell(cell);
		}
	}

	internal void UpdateCell(ICell cell)
	{
		SetCell(cell);
	}

	internal void SetSelectCell(AxialCoord coord)
	{
		selectedCellMap.Clear();
		selectedCellMap.SetCell(coord, "SELECT");
	}
}

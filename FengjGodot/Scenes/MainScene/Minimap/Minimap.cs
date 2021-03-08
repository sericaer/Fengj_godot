using Fengj.Map;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;

public class Minimap : Node2D
{
	public TileMap tileMap;

	internal MapData gmObj;


	public override void _Ready()
	{
		tileMap = GetNode<TileMap>("TileMap");
	}

	internal void SetGmObj(MapData map)
	{
		gmObj = map;

		tileMap.Clear();

		foreach (var cell in gmObj.cells)
        {
            if(cell.detectType == DetectType.TERRAIN_VISIBLE)
            {
                tileMap.SetCell(cell.axialCoord, cell.terrainDef.type.ToString());
            }

        }
    }
}

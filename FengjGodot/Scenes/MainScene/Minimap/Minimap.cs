using Fengj.Map;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;

public class Minimap : Node2D
{

	public TileMap tileMap;

	internal MapData gmObj;

	//public Vector2 Size => new Vector2(gmObj.colum * tileMap.CellSize.x, gmObj.row * tileMap.CellSize.y) * tileMap.Scale; 


	public override void _Ready()
	{
		tileMap = GetNode<TileMap>("TileMap");
	}


	internal void SetGmObj(MapData map)
	{
		gmObj = map;

		tileMap.Clear();

		//tileMap.TileSet = GlobalResource.tileSet;

		foreach (var cell in gmObj.cells)
        {
            if(cell.detectType == DetectType.TERRAIN_VISIBLE)
            {
                //GD.Print(cell.vectIndex, cell.terrainDef.type.ToString());
                tileMap.SetCell(cell.axialCoord, cell.terrainDef.type.ToString());
            }

        }

        // gmObj.WhenPropertyChanges(x => x.changedCell).Subscribe(y => UpdateCell(y.Value));

        //var point = tileMap.MapToWorld(new Vector2(gmObj.row / 2, gmObj.colum / 2));
        //camera.Position = point;
        //camera.MapSize = new Vector2(gmObj.colum * tileMap.CellSize.x, gmObj.row * tileMap.CellSize.y);
    }

	private void UpdateCell(ICell cell)
	{
		//if(cell.detectLevel == 0)
		//{
		//	return;
		//}

		tileMap.SetCell(cell.axialCoord, cell.terrainDef.type.ToString());
	}
}

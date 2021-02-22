using Fengj;
using Fengj.API;
using Fengj.Map;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;

public class Map : Node2D
{
	internal MapData gmObj;

	public TileMap tileMap;
	public MapCamera2D camera;

	public Vector2 vectotCameraBase;

	public Vector2 Size => new Vector2(gmObj.colum * tileMap.CellSize.x, gmObj.row * tileMap.CellSize.y) * tileMap.Scale;

	public override void _Ready()
	{
		camera = GetNode<MapCamera2D>("TileMap/Camera2D");
		vectotCameraBase = camera.Position;

		GD.Print("offset", camera.Position);
		GD.Print("offset", vectotCameraBase);

		tileMap = GetNode<TileMap>("TileMap");
		tileMap.TileSet = new TileSet();
	}

	internal void SetGmObj(MapData map)
	{
		gmObj = map;

		tileMap.TileSet = GlobalResource.tileSet;

		foreach (var cell in gmObj.cells)
		{
			//if(cell.detectLevel != 0)
			{
				tileMap.SetCells(cell.vectIndex, cell.terrainDef.path);
			}
			
		}

		gmObj.WhenPropertyChanges(x => x.changedCell).Subscribe(y=>UpdateCell(y.Value));

		var point = tileMap.MapToWorld(new Vector2(gmObj.row/2,gmObj.colum/2));
		camera.Position = point;
		camera.MapSize = new Vector2(gmObj.colum* tileMap.CellSize.x, gmObj.row * tileMap.CellSize.y);
	}

	private void UpdateCell(Cell cell)
	{
		if(cell.detectLevel == 0)
		{
			return;
		}

		tileMap.SetCells(cell.vectIndex, cell.terrainDef.path);
	}

	internal bool isPointIn(Vector2 pos)
	{
		var index = GetTileIndex(pos);
		return ((index.x > 0 && index.x < gmObj.row) && (index.y > 0 && index.y < gmObj.colum));
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsPressed())
			{
				if (eventMouseButton.ButtonIndex == 1 || eventMouseButton.ButtonIndex == 2)
				{
					var position = GetTileIndex(eventMouseButton.Position);

					var cell = gmObj.GetCell((int)position.x, (int)position.y);
					if(cell.detectLevel == 0)
					{
						cell.detectLevel = 1;
					}

					GD.Print("Click", position);
					return;
				}

				if (eventMouseButton.ButtonIndex == 4)
				{
					camera.ZoomDec();

				}

				if (eventMouseButton.ButtonIndex == 5)
				{
					camera.ZoomInc();
				}

			}
		}
	}

	private Vector2 GetTileIndex(Vector2 position)
	{
		//TODO GetTileIndex

		GD.Print("mouse", position);

		var offset = camera.Position - vectotCameraBase;
		GD.Print("offset", offset);

		var viewPortRect = GetViewportRect().Size / 2;
		var zoomOffset = viewPortRect - (viewPortRect / camera.Zoom);
		GD.Print("zoomOffset", zoomOffset);

		position = position + offset / camera.Zoom;

		position = position - zoomOffset;

		position = position * camera.Zoom;

		GD.Print("position", position);

		var rectIndex = tileMap.WorldToMap(position);

		GD.Print("rectIndex", rectIndex);

		var point = tileMap.MapToWorld(rectIndex);
		var hexCenter = new Vector2(point.x + 60, point.y + 70);

		GD.Print("hexCenter", hexCenter);

		var cubVect = new Vector2(position.x - hexCenter.x, hexCenter.y - position.y);
		GD.Print("cubVect", cubVect);

		double angle = cubVect.Angle();
		if (angle < 0)
		{
			angle += 2 * Math.PI;
		}
		GD.Print("angle", angle);

		var realIndex = rectIndex;

		if (angle > Math.PI / 6 && angle < Math.PI / 2)
		{
			var hexDialm = 70 * Math.Cos(Math.PI / 6);

			var dist = hexCenter.DistanceTo(position);
			var calcAgle = angle - Math.PI / 6;
			calcAgle = calcAgle > Math.PI / 6 ? calcAgle - Math.PI / 6 : calcAgle;

			var maxDist = hexDialm / Math.Cos(calcAgle);

			GD.Print("C 1");

			if (dist > maxDist)
			{
				realIndex = new Vector2(rectIndex.x + 1, rectIndex.y - 1);
			}
		}

		if (angle > Math.PI / 2 && angle < Math.PI * 5 / 6)
		{
			GD.Print("C 2");

			var hexDialm = 70 * Math.Cos(Math.PI / 6);

			var dist = hexCenter.DistanceTo(position);

			GD.Print("dist", dist);

			var calcAgle = angle - Math.PI / 2;
			calcAgle = calcAgle > Math.PI / 6 ? calcAgle - Math.PI / 6 : calcAgle;

			var maxDist = hexDialm / Math.Cos(calcAgle);

			GD.Print("maxDist", maxDist);

			if (dist > maxDist)
			{
				realIndex = new Vector2(rectIndex.x, rectIndex.y - 1);
			}
		}

		return realIndex;
	}
}

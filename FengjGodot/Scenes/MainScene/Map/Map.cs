using Godot;
using System;

public class Map : Node2D
{

	public TileMap tileMap;
	public MapCamera2D camera;

	Vector2 vectotCameraBase;

	public override void _Ready()
	{
		GD.Print(GlobalPath.mod);

		camera = GetNode<MapCamera2D>("Camera2D");
		vectotCameraBase = camera.Position;

		tileMap = GetNode<TileMap>("TileMap");
		tileMap.TileSet = CreateTileSet();

		tileMap.SetCell(0, 0, 0);
		tileMap.SetCell(1, 0, 0);
		tileMap.SetCell(2, 0, 0);
	}



	private TileSet CreateTileSet()
	{
		var tileSet = new TileSet();

		var id = tileSet.GetLastUnusedTileId();
		tileSet.CreateTile(id);
		tileSet.TileSetName(0, "TEST1");

		var path = GlobalPath.mod + "Native/png/map/terrain/grass_05.png";
		tileSet.TileSetTexture(0, ResourceLoader.Load<Texture>(path));

		return tileSet;
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

					GD.Print("Click", position);
					return;
				}

				if (eventMouseButton.ButtonIndex == 4)
				{
					if (camera.Zoom.x > 1)
					{
						camera.Zoom -= new Vector2(0.1f, 0.1f);

						GD.Print("camera.Zoom", camera.Zoom);
					}
					return;
				}

				if (eventMouseButton.ButtonIndex == 5)
				{
					if (camera.Zoom.x < 5)
					{
						var camera = GetNode<Camera2D>("Camera2D");
						camera.Zoom += new Vector2(0.1f, 0.1f);

						GD.Print("camera.Zoom", camera.Zoom);
					}

					return;
				}

			}
		}
	}

	private Vector2 GetTileIndex(Vector2 position)
	{
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

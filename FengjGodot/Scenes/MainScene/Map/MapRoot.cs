using Fengj;
using Fengj.API;
using Fengj.Map;
using Godot;
using HexMath;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Linq;

public class MapRoot : Node2D
{
	internal MapData gmObj;

	public Map map;
	public MapCamera2D camera;

	public override void _Ready()
	{
		camera = GetNode<MapCamera2D>("TileMap/Camera2D");
		camera.FuncIsViewRectVaild = (rect) =>
		{
			var leftTop = rect.Position;
			var rightBottom = rect.End;
			var leftBottom = rect.Position + new Vector2(0, rect.Size.y);
			var rightTop = rect.Position + new Vector2(rect.Size.x, 0);

			var array = new Vector2[] { leftTop, rightBottom, leftBottom, rightTop };
			return array.Any(p => {
				var offsetCoord = this.GetTileIndex(p);
				return gmObj.HasCell(offsetCoord);
			});
		};

		map = GetNode<Map>("Map");
	}

	internal void SetGmObj(MapData mapData)
	{
		gmObj = mapData;

		map.SetTerrainTileSet(GlobalResource.tileSet);
		foreach (var cell in gmObj.cells)
		{
			map.SetCell(cell);
		}

		gmObj.WhenPropertyChanges(x => x.changedCell).Subscribe(y => map.UpdateCell(y.Value));
	}

	//internal bool isPointIn(Vector2 pos)
	//{
	//	var index = GetTileIndex(pos);
	//	return ((index.x > 0 && index.x < gmObj.row) && (index.y > 0 && index.y < gmObj.colum));
	//}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsPressed())
			{
				if (eventMouseButton.ButtonIndex == 1 || eventMouseButton.ButtonIndex == 2)
				{
					var position = eventMouseButton.Position;
					//var coord = GetTileIndex(eventMouseButton.Position);

					////var cell = gmObj.GetCell((int)position.x, (int)position.y);
					////if (cell.detectLevel == 0)
					////{
					////	cell.detectLevel = 1;
					////}

					//GD.Print($"Click {position}, Coord {coord}");
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

	private AxialCoord GetTileIndex(Vector2 position)
	{
		//todo

		Layout flat = new Layout(Layout.flat, new Point(10.0, 15.0), new Point(camera.Position.x, camera.Position.y));

		var aixalCoord = flat.PixelToHex(new Point(position.x, position.y));

		return aixalCoord;

		//TODO GetTileIndex

		//GD.Print("mouse", position);

		//var offset = camera.Position - vectotCameraBase;
		//GD.Print("offset", offset);

		//var viewPortRect = GetViewportRect().Size / 2;
		//var zoomOffset = viewPortRect - (viewPortRect / camera.Zoom);
		//GD.Print("zoomOffset", zoomOffset);

		//position = position + offset / camera.Zoom;

		//position = position - zoomOffset;

		//position = position * camera.Zoom;

		//GD.Print("position", position);

		//var rectIndex = tileMap.WorldToMap(position);

		//GD.Print("rectIndex", rectIndex);

		//var point = tileMap.MapToWorld(rectIndex);
		//var hexCenter = new Vector2(point.x + 60, point.y + 70);

		//GD.Print("hexCenter", hexCenter);

		//var cubVect = new Vector2(position.x - hexCenter.x, hexCenter.y - position.y);
		//GD.Print("cubVect", cubVect);

		//double angle = cubVect.Angle();
		//if (angle < 0)
		//{
		//    angle += 2 * Math.PI;
		//}
		//GD.Print("angle", angle);

		//var realIndex = rectIndex;

		//if (angle > Math.PI / 6 && angle < Math.PI / 2)
		//{
		//    var hexDialm = 70 * Math.Cos(Math.PI / 6);

		//    var dist = hexCenter.DistanceTo(position);
		//    var calcAgle = angle - Math.PI / 6;
		//    calcAgle = calcAgle > Math.PI / 6 ? calcAgle - Math.PI / 6 : calcAgle;

		//    var maxDist = hexDialm / Math.Cos(calcAgle);

		//    GD.Print("C 1");

		//    if (dist > maxDist)
		//    {
		//        realIndex = new Vector2(rectIndex.x + 1, rectIndex.y - 1);
		//    }
		//}

		//if (angle > Math.PI / 2 && angle < Math.PI * 5 / 6)
		//{
		//    GD.Print("C 2");

		//    var hexDialm = 70 * Math.Cos(Math.PI / 6);

		//    var dist = hexCenter.DistanceTo(position);

		//    GD.Print("dist", dist);

		//    var calcAgle = angle - Math.PI / 2;
		//    calcAgle = calcAgle > Math.PI / 6 ? calcAgle - Math.PI / 6 : calcAgle;

		//    var maxDist = hexDialm / Math.Cos(calcAgle);

		//    GD.Print("maxDist", maxDist);

		//    if (dist > maxDist)
		//    {
		//        realIndex = new Vector2(rectIndex.x, rectIndex.y - 1);
		//    }
		//}

		//return realIndex;
	}
}

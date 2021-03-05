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
	public MapControl control;


	public Layout layout;

	public override void _Ready()
	{
		camera = GetNode<MapCamera2D>("Camera2D");
		map = GetNode<Map>("Map");
		control = GetNode<MapControl>("CanvasLayer/Control");

		layout = new Layout(Layout.flat, new Point(75, 75), new Point(camera.basePosition.x, camera.basePosition.y));

		control.funcCalcPos = (axialCoord) =>
		{
			var point = layout.HexToPixel(axialCoord);
			return new Vector2((float)point.x, (float)point.y);
		};

		camera.FuncIsViewRectVaild = (rect) =>
		{
			var cellSizeOffset = map.terrainMap.CellSize;

			var leftTop = rect.Position;
			var rightBottom = rect.End;
			var leftBottom = rect.Position + new Vector2(0, rect.Size.y);
			var rightTop = rect.Position + new Vector2(rect.Size.x, 0);

			leftTop += cellSizeOffset;
			rightBottom -= cellSizeOffset;
			leftBottom += cellSizeOffset * new Vector2(1, -1);
			rightTop += cellSizeOffset * new Vector2(-1, 1);

			var array = new Vector2[] { leftTop, rightBottom, leftBottom, rightTop };
			return array.Any(p =>
			{

				var offsetCoord = this.GetTileIndex(p);
				GD.Print($"p:{p} offsetCoord;{offsetCoord.q},{offsetCoord.r} dist:{offsetCoord.Length()}");
				return gmObj.HasCell(offsetCoord);
			});
		};
	}

	internal void SetGmObj(MapData mapData)
	{
		gmObj = mapData;

		map.SetGmObj(gmObj);
		control.SetGmObj(gmObj);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsPressed())
			{
			
				if (eventMouseButton.ButtonIndex == 1 || eventMouseButton.ButtonIndex == 2)
				{
					var position = camera.GetGlobalMousePosition();
					var coord = GetTileIndex(position);
					GD.Print($"Click {position}, Coord {coord.q},{coord.r}");

					//var cell = gmObj.GetCell(coord);
					//if (cell.detectType == DetectType.VISION_VISIBLE)
					//{
					//	cell.detectType = DetectType.TERRAIN_VISIBLE;
					//}

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
		var aixalCoord = layout.PixelToHex(new Point(position.x, position.y));

		return aixalCoord;
	}
}

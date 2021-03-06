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

		var canvasLayer = GetNode<CanvasLayer>("CanvasLayer");
		canvasLayer.Offset = Position;

		control = canvasLayer.GetNode<MapControl>("Control");

		layout = new Layout(Layout.flat, new Point(75, 75), new Point(0, 0));

		control.funcCalcPos = (axialCoord) =>
		{
			var point = layout.HexToPixel(axialCoord);
			return new Vector2((float)point.x, (float)point.y);
		};

		control.funcCalcAxialCoord = (pos) =>
		{
			return layout.PixelToHex(new Point(pos.x, pos.y));
		};

		camera.FuncIsViewRectVaild = (pos) =>
		{
			var offsetCoord = this.GetTileIndex(pos);
			GD.Print($"p:{pos} offsetCoord;{offsetCoord.q},{offsetCoord.r} dist:{offsetCoord.Length()}");
			return gmObj.HasCell(offsetCoord);
		};

		camera.ViewPortGlobalRectChanged = (rect) =>
		{
			control.OnViewPortGlobalRectChanged(rect);
		};
	}

	internal void SetGmObj(MapData mapData)
	{
		gmObj = mapData;

		map.SetGmObj(gmObj);

		control.SetGmObj(gmObj);
		control.OnViewPortGlobalRectChanged(camera.GetViewPortGlobalRect());
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsPressed())
			{
			
				if (eventMouseButton.ButtonIndex == 1 || eventMouseButton.ButtonIndex == 2)
				{
					var position = camera.GetLocalMousePosition() + camera.Position ;
					var coord = GetTileIndex(position);
					GD.Print($"Click {position}, Coord {coord.q},{coord.r}");

					var cell = gmObj.GetCell(coord);
					if (cell.detectType == DetectType.VISION_VISIBLE)
					{
						cell.detectType = DetectType.TERRAIN_VISIBLE;
					}

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

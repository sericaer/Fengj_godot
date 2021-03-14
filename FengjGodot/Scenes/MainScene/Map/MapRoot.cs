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
		control.layout = layout;
	}

	internal void SetGmObj(MapData mapData)
	{
		gmObj = mapData;

		map.SetGmObj(gmObj);
		foreach(var detectedCell in gmObj.cells.Where(x=>x.detectType == DetectType.TERRAIN_VISIBLE))
		{
			camera.UpdateMoveLimit(layout.HexToPixelVector2(detectedCell.axialCoord));
		}

		control.SetGmObj(gmObj);
		control.OnViewPortGlobalRectChanged(camera.GetViewPortGlobalRect());

		gmObj.WhenPropertyChanges(x => x.changedCell).Subscribe(y =>
		{
			map.UpdateCell(y.Value);

			if (y.Value.detectType == DetectType.TERRAIN_VISIBLE)
			{
				camera.UpdateMoveLimit(layout.HexToPixelVector2(y.Value.axialCoord));
			};
		});
	}

	internal void TryChangedViewportPosition(Vector2 pos)
	{
		camera.Position = pos;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsPressed())
			{

				if (eventMouseButton.ButtonIndex == 1 || eventMouseButton.ButtonIndex == 2)
				{
					var position = camera.GetLocalMousePosition() + camera.Position;
					var coord = layout.PixelVectorToHex(position);
					GD.Print($"Click {position}, Coord {coord.q},{coord.r}");

					map.SetSelectCell(coord);
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

	private void _on_Camera2D_ViewPortChanged(Rect2 rect)
	{
		control.OnViewPortGlobalRectChanged(rect);
	}
}

using Fengj.Map;
using Godot;
using System;

public class MinmapControl : Panel
{
	[Signal]
	public delegate void ViewRectPositionChanged(Vector2 pos);

	private Minimap map;
	private ViewportContainer viewPortContainer;
	private Control viewRect;

	public override void _Ready()
	{
		map = GetNode<Minimap>("ViewportContainer/Viewport/MinMap");
		viewRect = GetNode<Control>("ViewportContainer/Viewport/MinMap/CanvasLayer/ViewRect");
		viewPortContainer = GetNode<ViewportContainer>("ViewportContainer");
	}

	internal void SetGmObj(MapData gmObj, Rect2 mapViewPortRect)
	{
		map.SetGmObj(gmObj);

		GD.Print($"mapViewPortRect.Position{mapViewPortRect.Position}");
		UpdateViewRect(mapViewPortRect);
	}

	internal void UpdateViewRect(Rect2 mapViewPortRect)
	{
		viewRect.RectSize = mapViewPortRect.Size * map.tileMap.Scale;
		viewRect.SetPosition((mapViewPortRect.Position + mapViewPortRect.Size/2) * map.tileMap.Scale - viewRect.RectSize / 2);
	}

	private void _on_ButtonMinimap_pressed()
	{
		this.Visible = false;
	}

	private void _on_MinMap_MouseButtonPressed(Vector2 pos)
	{
		var realPos = pos + GetNode<Control>("ViewportContainer").RectPosition;
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsPressed())
			{
				if (eventMouseButton.ButtonIndex == 1 || eventMouseButton.ButtonIndex == 2)
				{
					var mousePos = GetLocalMousePosition();

					var viewPortContainerRect = viewPortContainer.GetRect();
					if (!viewPortContainerRect.HasPoint(mousePos))
					{
						return;
					}

					var pos = viewPortContainer.GetLocalMousePosition() - viewPortContainerRect.Size / 2;
					var readPos = pos / map.tileMap.Scale;

					GD.Print("GetLocalMousePosition()", mousePos, " pos", pos, " readPos", readPos);

					EmitSignal(nameof(ViewRectPositionChanged), readPos);
				}
			}
		}
	}
}                                                                     


using Fengj.Map;
using Godot;
using System;

public class MinmapControl : Panel
{
	[Signal]
	public delegate void MouseButtonPressed(Vector2 pos);

	private Minimap map;
	private Control viewRect;

	public override void _Ready()
	{
		map = GetNode<Minimap>("ViewportContainer/Viewport/MinMap");
		viewRect = GetNode<Control>("ViewportContainer/Viewport/MinMap/CanvasLayer/ViewRect");
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

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!Visible)
		{
			return;
		}

		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsPressed())
			{
				if (eventMouseButton.ButtonIndex == 1 || eventMouseButton.ButtonIndex == 2)
				{
					var mousePos = GetLocalMousePosition();
					var pos = (mousePos - GetViewportRect().Size / 2) / map.tileMap.Scale;

					EmitSignal(nameof(MouseButtonPressed), pos);
				}

			}
		}
	}
}                                                                     


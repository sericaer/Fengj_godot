using Fengj.Map;
using Godot;
using System;

public class MinmapControl : Panel
{
	public Func<Vector2, bool> FuncViewRectMoved;

	private Minimap map;
	private Control viewRect;

	public override void _Ready()
	{
		map = GetNode<Minimap>("ViewportContainer/Viewport/MinMap");
		
	}

	internal void SetGmObj(MapData gmObj, Rect2 mapViewPortRect)
	{
		map.SetGmObj(gmObj);

		viewRect = GetNode<Control>("ViewportContainer/Viewport/MinMap/CanvasLayer/ViewRect");
		viewRect.RectSize = mapViewPortRect.Size * map.tileMap.Scale;
		viewRect.SetPosition(mapViewPortRect.Position * map.tileMap.Scale);
	}

	private void _on_ButtonMinimap_pressed()
	{
		this.Visible = false;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(!Visible)
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
					var pos = (mousePos  - GetViewportRect().Size / 2) / map.tileMap.Scale;

					if(FuncViewRectMoved(pos))
                    {
						viewRect.SetPosition(mousePos);
					}
				}

			}
		}
	}
}                                                                     


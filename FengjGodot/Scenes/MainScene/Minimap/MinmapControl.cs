using Fengj.Map;
using Godot;
using System;

public class MinmapControl : Panel
{
	public Vector2 viewPositionOffset { get; set; }
	public Vector2 viewRectSizeOffset { get; set; }

	private Minimap map;

	public override void _Ready()
	{
		map = GetNode<Minimap>("ViewportContainer/Viewport/MinMap");
		
	}

	internal void SetGmObj(MapData gmObj, Rect2 mapViewPortRect)
	{
		map.SetGmObj(gmObj);

		var viewRect = GetNode<Control>("ViewportContainer/Viewport/MinMap/ViewRect");
		viewRect.RectSize = mapViewPortRect.Size * map.tileMap.Scale;
		viewRect.SetPosition(mapViewPortRect.Position * map.tileMap.Scale - viewRect.RectSize / 2);
	}

	private void _on_ButtonMinimap_pressed()
	{
		this.Visible = false;
	}
}                                                                     


using Fengj.Map;
using Godot;
using System;

public class MinmapControl : Panel
{

	public Vector2 viewPositionOffset { get; set; }
	public Vector2 viewRectSizeOffset { get; set; }

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	internal void SetGmObj(MapData gmObj)
	{
		var map = GetNode<Minimap>("ViewportContainer/Viewport/Map");
		map.SetGmObj(gmObj);

		GD.Print(map.GetParent<Viewport>().Size / 2 - map.Size / 2);
		map.Position = map.GetParent<Viewport>().Size / 2 - map.Size / 2;
	}

	private void _on_ButtonMinimap_pressed()
	{
		this.Visible = false;
	}

	private void _on_Minmap_visibility_changed()
	{
		if (this.Visible)
		{
			var map = GetNode<Minimap>("ViewportContainer/Viewport/Map");
			var viewRect = GetNode<Control>("ViewportContainer/Viewport/ViewRect");
			viewRect.RectSize = viewRectSizeOffset * map.Size;

			viewRect.SetPosition(map.Size/2 - viewRect.RectSize/2);		
		}
	}
}                                                                     


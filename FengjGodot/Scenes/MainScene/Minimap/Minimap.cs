using Fengj.Map;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;

public class Minimap : Node2D
{
	[Signal]
	public delegate void MouseButtonPressed(Vector2 pos);

	public TileMap tileMap;

	public Camera2D camera;

	internal MapData gmObj;


	public override void _Ready()
	{
		tileMap = GetNode<TileMap>("TileMap");
		camera = GetNode<Camera2D>("Camera2D");
	}

	internal void SetGmObj(MapData map)
	{
		gmObj = map;

		tileMap.Clear();

		foreach (var cell in gmObj.cells)
		{
			if(cell.detectType == DetectType.TERRAIN_VISIBLE)
			{
				tileMap.SetCell(cell.axialCoord, cell.terrainDef.type.ToString());
			}

		}
	}

    //public override void _UnhandledInput(InputEvent @event)
 //   public override void _Input(InputEvent @event)
 //   {
	//	if (@event is InputEventMouseButton eventMouseButton)
	//	{
	//		if (eventMouseButton.IsPressed())
	//		{
	//			if (eventMouseButton.ButtonIndex == 1 || eventMouseButton.ButtonIndex == 2)
	//			{
	//				var mousePos = camera.GetLocalMousePosition();
	//				GD.Print("camera.GetLocalMousePosition()", mousePos);
	//				EmitSignal(nameof(MouseButtonPressed), mousePos);
	//			}
	//		}
	//	}
	//}
}

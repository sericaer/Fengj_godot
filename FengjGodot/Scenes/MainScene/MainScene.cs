using Godot;
using System;

using Fengj.Facade;

public class MainScene : Control
{

	Map map;

	Facade facade;
	public override void _Ready()
	{
		facade = new Facade(GlobalPath.mod);
		GlobalResource.BuildTileSet(facade.modder.terrainDefs);

		facade.GenerateRunData(new RunInit() { mapSize = (100, 100)});

		map = GetNode<Map>("Map");
		map.gmObj = facade.runData.map;
	}

	private void _on_ButtonDirect_mouse_entered(String direct)
	{
		GD.Print(direct);
		map.camera.StartMove(direct);
	}


	private void _on_ButtonDirect_mouse_exited()
	{
		map.camera.StopMove();
	}

}

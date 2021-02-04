using Godot;
using System;

using Fengj.Facade;
using Fengj.Modder;

public class MainScene : Control
{

	Map map;

	Facade facade;
	public override void _Ready()
	{
		Facade.logger = (str) => GD.Print(str);
		Facade.InitIO();

		facade = new Facade();

		facade.CreateModder(GlobalPath.mod);

		GlobalResource.BuildTileSet(facade.modder.terrainDefs);

		facade.CreateRunData(new RunInit() { mapBuildType = Fengj.Map.MapBuildType.MAP_PLAIN, mapSize = (100, 100)});

		map = GetNode<Map>("Map");
		
		map.SetGmObj(facade.runData.map);
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

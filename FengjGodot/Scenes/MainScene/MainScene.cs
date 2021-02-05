using Godot;
using System;

using Fengj.Facade;
using Fengj.Modder;
using System.Linq;

public class MainScene : Control
{

	Map map;

	Facade facade;
	public override void _Ready()
	{
		Facade.logger = (str) => GD.Print(str);
		Facade.InitStatic();

		facade = new Facade();

		facade.CreateModder(GlobalPath.mod);

		GlobalResource.BuildTileSet(facade.modder.dictTerrainDefs.Values.SelectMany(p=>p.Values));

		facade.CreateRunData(new RunInit() { mapBuildType = Fengj.Map.MapBuildType.MAP_PLAIN, mapSize = (100, 100)});

		map = GetNode<Map>("Map");
		GD.Print(facade.runData.map);
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

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

		facade.CreateRunData(new RunInit() { mapBuildType = Fengj.Map.MapBuildType.MAP_PLAIN, mapSize = (90, 90)});

		map = GetNode<Map>("WordMap/ViewportContainer/Viewport/Map");
		GD.Print(facade.runData.map);
		map.SetGmObj(facade.runData.map);

		var minmap = GetNode<MinmapControl>("CanvasLayer/Minmap");
		minmap.SetGmObj(facade.runData.map);
	}

	private void _on_ButtonDirect_mouse_entered(String direct)
	{
		map.camera.StartMove(direct);
	}


	private void _on_ButtonDirect_mouse_exited()
	{
		map.camera.StopMove();
	}

	private void _on_ButtonMinmap_pressed()
	{
		var minimapControl = GetNode<MinmapControl>("CanvasLayer/Minmap");
		minimapControl.viewPositionOffset = map.camera.Position / map.tileMap.CellSize * map.camera.Zoom;

		//GD.Print("offset", minimapControl.viewPositionOffset);

		minimapControl.viewRectSizeOffset = map.camera.GetViewportRect().Size / map.tileMap.CellSize * map.camera.Zoom ;

		//minimapControl.Visible = true;
	}
}




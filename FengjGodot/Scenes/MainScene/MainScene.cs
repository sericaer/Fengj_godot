using Godot;
using System;

using Fengj.Facade;
using Fengj.Modder;
using System.Linq;

public class MainScene : Node2D
{

	MapRoot mapRoot;

	Facade facade;
	public override void _Ready()
	{
		Facade.logger = (str) => GD.Print(str);
		Facade.InitStatic();

		facade = new Facade();

		facade.CreateModder(GlobalPath.mod);

		GlobalResource.BuildTileSet(facade.modder.dictTerrainDefs.Values.SelectMany(p=>p.Values));

		facade.CreateRunData(new RunInit() { mapBuildType = Fengj.Map.MapBuildType.MAP_PLAIN, mapSize = (90, 90)});

		mapRoot = GetNode<MapRoot>("MapRoot");

		GD.Print(mapRoot);

		mapRoot.SetGmObj(facade.runData.map);

		//var minmap = GetNode<MinmapControl>("CanvasLayer/Minmap");
		//minmap.SetGmObj(facade.runData.map);
	}

	private void _on_ButtonDirect_mouse_entered(String direct)
	{
		GD.Print("_on_ButtonDirect_mouse_entered");
		mapRoot.camera.StartMove(direct);
	}


	private void _on_ButtonDirect_mouse_exited()
	{
		mapRoot.camera.StopMove();
	}

	private void _on_ButtonMinmap_pressed()
	{
		var minimapControl = GetNode<MinmapControl>("CanvasLayer/Minmap");
		minimapControl.viewPositionOffset = mapRoot.camera.Position / mapRoot.map.terrainMap.CellSize * mapRoot.camera.Zoom;

		//GD.Print("offset", minimapControl.viewPositionOffset);

		minimapControl.viewRectSizeOffset = mapRoot.camera.GetViewportRect().Size / mapRoot.map.terrainMap.CellSize * mapRoot.camera.Zoom ;

		//minimapControl.Visible = true;
	}
}

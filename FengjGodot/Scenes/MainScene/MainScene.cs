using Godot;
using System;

using Fengj.Facade;
using Fengj.Modder;
using System.Linq;

public class MainScene : Node2D
{

	MapRoot mapRoot;
	MinmapControl minimapControl;

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
		mapRoot.SetGmObj(facade.runData.map);

		minimapControl = GetNode<MinmapControl>("CanvasLayer/MinMap");
		
		minimapControl.Connect("MouseButtonPressed", this, "_on_MiniMapMouseButton_pressed");
	}

	private void _on_ButtonDirect_mouse_entered(String direct)
	{
		mapRoot.camera.StartMove(direct);
	}

	private void _on_ButtonDirect_mouse_exited()
	{
		mapRoot.camera.StopMove();
	}

	private void _on_ButtonMiniMap_pressed()
	{
		minimapControl.SetGmObj(facade.runData.map, mapRoot.camera.GetViewPortGlobalRect());
		minimapControl.Visible = true;
	}

	private void _on_MiniMapMouseButton_pressed(Vector2 pos)
	{
		mapRoot.camera.Position = pos;
		minimapControl.UpdateViewRect(mapRoot.camera.GetViewPortGlobalRect());

	}
}

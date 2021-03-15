using Godot;
using System;

using Fengj.Facade;
using Fengj.Modder;
using System.Linq;

public class MainScene : Node2D
{

	MapRoot mapRoot;
	MinmapControl minimapControl;
	Control cencterControl;

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

		cencterControl = GetNode<Control>("CanvasLayer/GUI/Center");
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

	private void _on_MinMap_ViewRectPositionChanged(Vector2 pos)
	{
		mapRoot.camera.SetCameraPosition(pos);
		minimapControl.UpdateViewRect(mapRoot.camera.GetViewPortGlobalRect());
	}

	private void _on_MapRoot_CellClicked(Vector2 vect)
	{
		var olds = cencterControl.GetChildren<CellTabPanel>();
		foreach (var elem in olds)
		{
			elem.QueueFree();
		}
		
		var cellTabPanel = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/CellTabPanel/CellTabPanel.tscn").Instance() as CellTabPanel;
		cencterControl.AddChild(cellTabPanel);
		cellTabPanel.SetGmObj(facade.runData.map.GetCell((int)vect.x, (int)vect.y), facade.runData.taskManager);

		cellTabPanel.Connect("DetectCell", this, nameof(MapRoot._on_DetectCell));
	}
}




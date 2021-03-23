using Godot;
using System;

using Fengj.Facade;
using Fengj.Modder;
using System.Linq;

public class MainScene : Node2D
{

	MapRoot mapRoot;
	MinmapControl minimapControl;
	PopupContainer popupContainer;
	TaskContainer taskContainer;
	Clan clanTop;

	Facade facade;

	public override void _EnterTree()
	{
		Facade.logger = (str) => GD.Print(str);
		Facade.InitStatic();

		facade = new Facade();

		facade.CreateModder(GlobalPath.mod);

		GlobalResource.BuildTileSet(facade.modder.dictTerrainDefs.Values.SelectMany(p => p.Values));

		facade.CreateRunData(new RunInit() { mapBuildType = Fengj.Map.MapBuildType.MAP_PLAIN, mapSize = (90, 90) });

		CellTop.taskManager = facade.runData.taskManager;
	}

	public override void _Ready()
	{

		mapRoot = GetNode<MapRoot>("MapRoot");
		mapRoot.SetGmObj(facade.runData.map);

		minimapControl = GetNode<MinmapControl>("CanvasLayer/MinMap");

		popupContainer = GetNode<PopupContainer>("CanvasLayer/GUI/Center/PopupContainer");

		taskContainer = GetNode<TaskContainer>("CanvasLayer/GUI/Center/TaskContainer");
		taskContainer.taskManager = facade.runData.taskManager;

		clanTop = GetNode<Clan>("CanvasLayer/GUI/Top/HBoxContainer/Clan");
		clanTop.SetGmObj(facade.runData.clanManager);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			GD.Print(eventMouseMotion.Position);

			String direct = "NULL";
			if(eventMouseMotion.Position.x <= 3)
			{
				direct = "LEFT";
			}
			if (eventMouseMotion.Position.x >= GetViewportRect().Size.x-3)
			{
				direct = "RIGHT";
			}
			if (eventMouseMotion.Position.y <= 3)
			{
				direct = "UP";
			}
			if (eventMouseMotion.Position.y >= GetViewportRect().Size.y-3)
			{
				direct = "DOWN";
			}

			mapRoot.camera.StartMove(direct);
		}
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
		var cellTabPanel = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/CellTabPanel/CellTabPanel.tscn").Instance() as CellTabPanel;
		popupContainer.AddChild(cellTabPanel);
		
		cellTabPanel.SetCellCoord(((int)vect.x, (int)vect.y));

		//cellTabPanel.Connect("DetectCell", mapRoot, nameof(MapRoot._on_DetectCell));
	}

	private void _on_TimeSpeedControl_DaysInc()
	{
		GD.Print("_on_TimeSpeedControl_DaysInc");
		facade.runData.DaysInc();
	}

	private void _on_ClanTopButton_pressed()
	{
		var clanTable = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/ClanTable/ClanTable.tscn").Instance() as ClanTable;
		popupContainer.AddChild(clanTable);
		clanTable.SetGmObj(facade.runData.clanManager);
	}

	private void _on_TaskContainer_TaskClick(Vector2 vect)
    {
		var cellTabPanel = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/CellTabPanel/CellTabPanel.tscn").Instance() as CellTabPanel;
		popupContainer.AddChild(cellTabPanel);

		cellTabPanel.SetCellCoord(((int)vect.x, (int)vect.y));
	}
}




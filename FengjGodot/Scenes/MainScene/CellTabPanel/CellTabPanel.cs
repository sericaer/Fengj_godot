using Fengj.Clan;
using Fengj.Map;
using Fengj.Task;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Linq;

class CellTabPanel : MarginContainer
{
	[Signal]
	public delegate void DetectCell(Vector2 vect2);

	//private ITaskManager taskManager;
	private ICell gmObj;

	Control detectPanel;
	Label terrainLabel;
	TabContainer tabContainer;

	internal void SetCellCoord((int q, int r) coord)
	{
		this.gmObj = MapData.inst.GetCell(coord.q, coord.r);

		if (gmObj.detectType == DetectType.TERRAIN_VISIBLE)
		{
			terrainLabel.Text = gmObj.terrainType.ToString();
			return;
		}

		detectPanel.Visible = true;

		for (int i = 1; i < tabContainer.GetTabCount(); i++)
		{
			tabContainer.SetTabDisabled(i, true);
		}

		var task = TaskManager.inst.getCellTask(gmObj);

		if (task != null)
		{
			GD.Print("if (task != null)");
			var progressPanel = detectPanel.GetNode<Panel>("ProgressPanel");
			progressPanel.Visible = true;

			var progressBar = progressPanel.GetNode<ProgressBar>("VBoxContainer/ProgressBar");
			progressBar.Value = task.percent;

			task.WhenPropertyValueChanges(x => x.percent).Subscribe(x => progressBar.Value = x);
			task.WhenPropertyValueChanges(x => x.isFinsihed).Subscribe(x => { if (x) detectPanel.Visible = false; });
			task.WhenPropertyValueChanges(x => x.isCanceled).Subscribe(x => { if (x) progressPanel.Visible = false; progressBar.Value = 0; });
		}
	}

	public override void _Ready()
	{
		tabContainer = GetNode<TabContainer>("Panel/TabContainer");
		detectPanel = tabContainer.GetNode<Control>("Info/Detect");
		terrainLabel = tabContainer.GetNode<Label>("Info/VBoxContainer/PanelContainer/HBoxContainer/Value");

		var detectButton = detectPanel.GetNode<Button>("Button");
		detectButton.Connect("pressed", this, nameof(_on_DetectedButton_pressed));
	}

	private void _on_DetectedButton_pressed()
	{
		var clanSelectPanel = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/ClanTable/ClanSelectPanel.tscn").Instance() as ClanSelectPanel;
		this.AddChild(clanSelectPanel);
		clanSelectPanel.SetGmObj(ClanManager.inst);

		clanSelectPanel.Connect("SelectedClan", this, nameof(_on_CreateDetectTask));
	}


	private void _on_CreateDetectTask(string clanKey)
	{
		var clans = ClanManager.inst.Where(x => x.key == clanKey);
		var task = new CellTask(CellTask.Type.Detect, gmObj, clans.ToList());
		TaskManager.inst.AddTask(task);

		EmitSignal(nameof(DetectCell), new object[] { new Vector2(gmObj.axialCoord.q, gmObj.axialCoord.r) });
		this.QueueFree();
	}

	private void _on_CloseButton_pressed()
	{
		QueueFree();
	}

	private void _on_DetectCancelButton_pressed()
    {
		var task = TaskManager.inst.getCellTask(gmObj);
		TaskManager.inst.Cancel(task);
	}
}

using Fengj.Clan;
using Fengj.Map;
using Fengj.Task;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Linq;

interface IRequireCell
{
	(int q, int r) coord { get; }

	ICell cell { set; }
}

class CellTabPanel : TabContainer
{
	[Signal]
	public delegate void DetectCell(Vector2 vect2);

	//private ITaskManager taskManager;
	private ICell gmObj;

	Control detectPanel;
	Label terrainLabel;

	internal void SetCellCoord((int q, int r) coord)
	{
		this.gmObj = MapData.inst.GetCell(coord.q, coord.r);

		if (gmObj.detectType == DetectType.TERRAIN_VISIBLE)
		{
			terrainLabel.Text = gmObj.terrainType.ToString();
			return;
		}

		detectPanel.Visible = true;

		for (int i = 1; i < this.GetTabCount(); i++)
		{
			this.SetTabDisabled(i, true);
		}

		var task = TaskManager.inst.getCellTask(gmObj);

		if (task != null)
		{
			var progressPanel = detectPanel.GetNode<Panel>("ProgressPanel");
			progressPanel.Visible = true;

			var progressBar = progressPanel.GetNode<ProgressBar>("ProgressBar");
			task.WhenPropertyValueChanges(x => x.percent).Subscribe(x => progressBar.Value = x);
			task.WhenPropertyValueChanges(x => x.isFinsihed).Subscribe(x => { if (x) detectPanel.Visible = false; });
		}
	}

	public override void _Ready()
	{
		detectPanel = GetNode<Control>("Info/Detect");
		terrainLabel = GetNode<Label>("Info/VBoxContainer/PanelContainer/HBoxContainer/Value");

		var detectButton = detectPanel.GetNode<Button>("Button");
		detectButton.Connect("pressed", this, nameof(_on_DetectedButton_pressed));
	}

	private void _on_DetectedButton_pressed()
	{
		var clanSelectPanel = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/ClanTable/ClanSelectPanel.tscn").Instance() as ClanSelectPanel;
		GetParent().AddChild(clanSelectPanel);
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
}




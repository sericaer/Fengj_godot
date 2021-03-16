using Fengj.Map;
using Fengj.Task;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;

public class CellTabPanel : TabContainer
{
	[Signal]
	public delegate void DetectCell(Vector2 vect2);

	private ITaskManager taskManager;
	private ICell gmObj;

	Control detectPanel;
	Label terrainLabel;

	internal void SetGmObj(ICell cell, ITaskManager taskManager)
	{
		this.gmObj = cell;
		this.taskManager = taskManager;

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

		var task = taskManager.getCellTask(gmObj.axialCoord.q, gmObj.axialCoord.r);
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
		var task = new CellTask(CellTask.Type.Detect, (gmObj.axialCoord.q, gmObj.axialCoord.r));
		taskManager.AddTask(task);

		EmitSignal(nameof(DetectCell), new object[] { new Vector2(gmObj.axialCoord.q, gmObj.axialCoord.r) });
		this.QueueFree();
	}
}




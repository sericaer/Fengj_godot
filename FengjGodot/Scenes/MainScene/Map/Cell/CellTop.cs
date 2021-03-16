using Fengj.Map;
using Godot;
using System;
using ReactiveMarbles.PropertyChanged;
using System.Linq;
using Fengj.Task;

public class CellTop : PanelContainer
{
	public static ITaskManager taskManager;

	internal ICell gmObj;

	Label label;
	Label percent;

	
	public override void _Ready()
	{
		label = GetNode<Label>("VBoxContainer/Label");
		percent = GetNode<Label>("VBoxContainer/Percent");
	}

	internal void SetGmObj(ICell cell)
	{
		gmObj = cell;
		label.Text = $"{cell.axialCoord.q}-{cell.axialCoord.r}";
		percent.Visible = false;

		ShowTask();
	}

	internal void ShowTask()
	{
		var cellTask = taskManager.getCellTask(gmObj.axialCoord.q, gmObj.axialCoord.r);
		if (cellTask != null)
		{
			percent.Visible = true;

			cellTask.WhenPropertyChanges(x => x.percent).Subscribe(x => percent.Text = $"{x.Value}%");
			cellTask.WhenPropertyChanges(x => x.isFinsihed).Subscribe(x =>
			{
				if (x.Value)
				{
					percent.Visible = false;
				}
			});
		}
	}
}

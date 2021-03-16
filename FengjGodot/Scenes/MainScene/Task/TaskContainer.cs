using Godot;
using System;
using System.Linq;

public class TaskContainer : VBoxContainer
{
	internal Fengj.Task.ITaskManager taskManager
	{
		set
		{
			value.OnAddItem(x =>
			{
				var taskUI = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/Task/TaskPanel.tscn").Instance() as TaskPanel;
				this.AddChild(taskUI);

				taskUI.gmObj = x;

			});

			value.OnRemoveItem(x =>
			{
				GD.Print("OnRemoveItem");
				var needQueue = this.GetChildren<TaskPanel>().SingleOrDefault(y => y.gmObj == x);
				needQueue?.QueueFree();
			});
		}
	}
}

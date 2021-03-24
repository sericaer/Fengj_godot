using Fengj.Task;
using Godot;
using System;
using System.Linq;

public class TaskContainer : VBoxContainer
{
	[Signal]
	public delegate void TaskClick(Vector2 vector2);

	internal Fengj.Task.ITaskManager taskManager
	{
		set
		{
			value.OnAddItem(x =>
			{
				var taskUI = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/Task/TaskPanel.tscn").Instance() as TaskPanel;
				this.AddChild(taskUI);

				taskUI.gmObj = x;

				if(x is CellDetectTask cellTask)
                {
					var param = new Vector2(cellTask.cell.axialCoord.q, cellTask.cell.axialCoord.r);
					taskUI.button.Connect("pressed", this, nameof(_on_TaskButtonPressed), new Godot.Collections.Array() { param });
				}
			});

			value.OnRemoveItem(x =>
			{
				GD.Print("OnRemoveItem");
				var needQueue = this.GetChildren<TaskPanel>().SingleOrDefault(y => y.gmObj == x);
				needQueue?.QueueFree();
			});
		}
	}

	private void _on_TaskButtonPressed(Vector2 obj)
    {
		EmitSignal(nameof(TaskClick), obj);
    }
}

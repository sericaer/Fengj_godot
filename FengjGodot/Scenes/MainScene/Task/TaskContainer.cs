using Godot;
using System;
using System.Linq;

public class TaskContainer : PanelContainer
{
    internal Fengj.Task.ITaskManager taskManager
    {
        set
        {
            value.OnAddItem(x =>
            {
                var taskUI = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/Task/TaskPanel.tscn").Instance() as TaskPanel;
                container.AddChild(taskUI);

                taskUI.gmObj = x;

            });

            value.OnRemoveItem(x =>
            {
                GD.Print("OnRemoveItem");
                var needQueue = container.GetChildren<TaskPanel>().SingleOrDefault(y => y.gmObj == x);
                needQueue?.QueueFree();
            });
        }
    }

    Container container;

    public override void _Ready()
    {
        container = GetNode<VBoxContainer>("VBoxContainer");
    }
}

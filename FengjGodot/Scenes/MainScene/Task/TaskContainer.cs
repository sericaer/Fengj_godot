using Fengj.Task;
using Godot;
using System;
using System.Linq;

public class TaskContainer : PanelContainer
{
    internal ITaskManager taskManager
    {
        set
        {
            value.OnAddItem(x =>
            {
                var taskUI = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/Task/Task.tscn").Instance() as Task;
                container.AddChild(taskUI);

                taskUI.gmObj = x;

            });

            value.OnRemoveItem(x =>
            {
                GD.Print("OnRemoveItem");
                var needQueue = container.GetChildren<Task>().SingleOrDefault(y => y.gmObj == x);
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

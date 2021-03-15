using Fengj.Map;
using Godot;
using System;
using ReactiveMarbles.PropertyChanged;
using System.Linq;

public class CellTop : PanelContainer
{
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
	}

	internal void AddTask()
	{
		percent.Text = "100%";
	}
}

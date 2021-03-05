using Fengj.Map;
using Godot;
using System;

public class CellTop : PanelContainer
{
	internal ICell gmObj;

	Label label;

	public override void _Ready()
	{
		label = GetNode<Label>("VBoxContainer/Label");

	}

	internal void SetGmObj(ICell cell)
	{
		gmObj = cell;
		label.Text = $"{cell.axialCoord.q}-{cell.axialCoord.r}";
	}
}

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

	private ICell gmObj;

	DetectPanel detectPanel;
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

		detectPanel.SetCell(gmObj);
	}

	public override void _Ready()
	{
		tabContainer = GetNode<TabContainer>("Panel/TabContainer");
		detectPanel = tabContainer.GetNode<DetectPanel>("Info/Detect");
		terrainLabel = tabContainer.GetNode<Label>("Info/VBoxContainer/PanelContainer/HBoxContainer/Value");
	}

	private void _on_CloseButton_pressed()
	{
		QueueFree();
	}
}

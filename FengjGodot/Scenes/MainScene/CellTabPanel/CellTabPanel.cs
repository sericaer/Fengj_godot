using Fengj.Map;
using Godot;
using System;

public class CellTabPanel : TabContainer
{
	[Signal]
	public delegate void DetectCell(Vector2 vect2);

	private ICell gmObj;

	Control detectPanel;
	Label terrainLabel;

	internal void SetGmObj(ICell cell)
	{
		this.gmObj = cell;
		if(gmObj.detectType != DetectType.TERRAIN_VISIBLE)
		{
			detectPanel.Visible = true;

			for(int i=1;i<this.GetTabCount();i++)
			{
				this.SetTabDisabled(i, true);
			}

			return;
		}

		terrainLabel.Text = gmObj.terrainType.ToString();
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
		EmitSignal(nameof(DetectCell), new object[] { new Vector2(gmObj.axialCoord.q, gmObj.axialCoord.r) });
		this.QueueFree();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}




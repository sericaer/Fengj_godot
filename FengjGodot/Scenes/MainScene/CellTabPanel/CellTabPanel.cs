using Fengj.Map;
using Godot;
using System;

public class CellTabPanel : TabContainer
{
	[Signal]
	public delegate void DetectCell(Vector2 vect2);

	private ICell cell;

	Button detectButton;
	
	internal void SetGmObj(ICell cell)
	{
		this.cell = cell;
	}

	public override void _Ready()
	{
		detectButton = GetNode<Button>("Info/Detect/Button");
		detectButton.Connect("pressed", this, nameof(_on_DetectedButton_pressed));
	}

	private void _on_DetectedButton_pressed()
	{
		EmitSignal(nameof(DetectCell), new object[] { new Vector2(cell.axialCoord.q, cell.axialCoord.r) });
		this.QueueFree();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}




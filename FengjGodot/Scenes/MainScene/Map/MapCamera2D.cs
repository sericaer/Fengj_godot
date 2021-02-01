using Godot;
using System;

public class MapCamera2D : Camera2D
{

	MoveTo moveTo = MoveTo.NULL;

	enum MoveTo
	{
		NULL,
		UP,
		DOWN,
		LEFT,
		RIGHT,
	}

	public override void _Ready()
	{

		var viewPortRect = GetViewportRect();

		this.Position = viewPortRect.Size / 2;
		
	}

	public override void _Process(float delta)
	{
		switch (moveTo)
		{
			case MoveTo.LEFT:
				this.Position -= new Vector2(3 * this.Zoom.x, 0);
				break;
			case MoveTo.RIGHT:
				this.Position += new Vector2(3 * this.Zoom.x, 0);
				break;
			case MoveTo.UP:
				this.Position -= new Vector2(0, 3 * this.Zoom.y);
				break;
			case MoveTo.DOWN:
				this.Position += new Vector2(0, 3 * this.Zoom.y);
				break;
		}
	}

	internal void StartMove(string direct)
	{
		Enum.TryParse(direct, out moveTo);
	}

	internal void StopMove()
	{
		moveTo = MoveTo.NULL;
	}
}

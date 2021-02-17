using Godot;
using System;

public class MapCamera2D : Camera2D
{

	MoveTo moveTo = MoveTo.NULL;

	public Vector2 MapSize { get; internal set; }

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
		var rectSize = GetViewportRect().Size;

		switch (moveTo)
		{
			case MoveTo.LEFT:
				if (this.Position.x / this.Zoom.x - rectSize.x/2 > 0)
				{
					this.Position -= new Vector2(3 * this.Zoom.x, 0);
				}
				break;
			case MoveTo.RIGHT:
				if (this.Position.x + rectSize.x * this.Zoom.x /2 < MapSize.x)
				{
					this.Position += new Vector2(3 * this.Zoom.x, 0);
				}
				break;
			case MoveTo.UP:
				if (this.Position.y / this.Zoom.y - rectSize.y / 2 > 0)
				{
					this.Position -= new Vector2(0, 3 * this.Zoom.y);
				}
				break;
			case MoveTo.DOWN:
				if (this.Position.y + rectSize.y * this.Zoom.y / 2 < MapSize.y)
				{
					this.Position += new Vector2(0, 3 * this.Zoom.y);
				}
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

	internal void ZoomDec()
	{
		if (Zoom.x > 1)
		{
			Zoom -= new Vector2(0.1f, 0.1f);

			GD.Print("camera.Zoom", Zoom);
		}
		return;
	}

	internal void ZoomInc()
	{
		if (Zoom.x < 5)
		{
			Zoom += new Vector2(0.1f, 0.1f);

			GD.Print("camera.Zoom", Zoom);
		}
	}
}

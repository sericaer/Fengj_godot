using Godot;
using System;

public class MapCamera2D : Camera2D
{
	public Func<Rect2, bool> FuncIsViewRectVaild;

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
		//this.Position = offset;
	}

	public override void _Process(float delta)
	{
		var rect = GetViewPortGlobalRect();
		switch (moveTo)
		{
			case MoveTo.LEFT:
				rect.Position -= new Vector2(3 * this.Zoom.x, 0);
				if (FuncIsViewRectVaild(rect))
				{
					this.Position -= new Vector2(3 * this.Zoom.x, 0);
				}
				break;
			case MoveTo.RIGHT:
				rect.Position += new Vector2(3 * this.Zoom.x, 0);
				if (FuncIsViewRectVaild(rect))
				{
					this.Position += new Vector2(3 * this.Zoom.x, 0);
				}
				break;
			case MoveTo.UP:
				rect.Position -= new Vector2(0, 3 * this.Zoom.y);
				if (FuncIsViewRectVaild(rect))
				{
					this.Position -= new Vector2(0, 3 * this.Zoom.y);
				}
				break;
			case MoveTo.DOWN:
				rect.Position += new Vector2(0, 3 * this.Zoom.y);
				if (FuncIsViewRectVaild(rect))
				{
					this.Position += new Vector2(0, 3 * this.Zoom.y);
				}
				break;
		}
	}

	public Rect2 GetViewPortGlobalRect()
	{
		var rect = GetViewportRect();
		rect.Size *= this.Zoom;
		rect.Position = Position;

		return rect;
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
			Zoom *= new Vector2(0.5f, 0.5f);

			GD.Print("camera.Zoom", Zoom);
			GD.Print("GetViewportRect", GetViewPortGlobalRect().Size);
		}
		return;
	}

	internal void ZoomInc()
	{
		if (Zoom.x < 32)
		{
			Zoom *= new Vector2(2f, 2f);

			GD.Print("camera.Zoom", Zoom);
			GD.Print("GetViewportRect", GetViewPortGlobalRect().Size);
		}
	}
}

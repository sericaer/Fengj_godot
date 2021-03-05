using Godot;
using System;

public class MapCamera2D : Camera2D
{
	public Func<Vector2, bool> FuncIsViewRectVaild;

	MoveTo moveTo = MoveTo.NULL;

	enum MoveTo
	{
		NULL,
		UP,
		DOWN,
		LEFT,
		RIGHT,
	}


	public override void _Process(float delta)
	{
		var changed = new Vector2();
		switch (moveTo)
		{
			case MoveTo.NULL:
				return;
			case MoveTo.LEFT:
				changed = new Vector2(-3 * this.Zoom.x, 0);
				break;
			case MoveTo.RIGHT:
				changed = new Vector2(3 * this.Zoom.x, 0);
				break;
			case MoveTo.UP:
				changed = new Vector2(0, -3 * this.Zoom.y);
				break;
			case MoveTo.DOWN:
				changed = new Vector2(0, 3 * this.Zoom.y);
				break;
			default:
				throw new Exception();
		}

		if (FuncIsViewRectVaild(this.Position + changed * 5))
		{
			this.Position += changed;
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

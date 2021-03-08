using Godot;
using System;

public class MapCamera2D : Camera2D
{
	[Signal]
	public delegate void ViewPortChanged(Rect2 rect);

	private MoveTo moveTo = MoveTo.NULL;

	private (float left, float right, float top, float bottom) limit;

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

		var newPos = this.Position + changed;
		if ((newPos.x > limit.left && newPos.x < limit.right)
			&&(newPos.y > limit.top && newPos.y < limit.bottom))
		{
			this.Position += changed;

			EmitSignal(nameof(ViewPortChanged), GetViewPortGlobalRect());
		}
	}

	public Rect2 GetViewPortGlobalRect()
	{
		var rect = GetViewport().GetVisibleRect();
		rect.Size *= this.Zoom;
		GD.Print(rect.Size);
		rect.Position = Position - rect.Size / 2;

		return rect;
	}

	internal void StartMove(string direct)
	{
		Enum.TryParse(direct, out moveTo);
	}

	internal void UpdateMoveLimit(Vector2 pos)
	{
		if(limit.left > pos.x)
		{
			limit.left = (int)pos.x;
		}
		if (limit.right < pos.x)
		{
			limit.right = (int)pos.x;
		}
		if (limit.top > pos.y)
		{
			limit.top = (int)pos.y;
		}
		if(limit.bottom < pos.y)
		{
			limit.bottom = (int)pos.y;
		}
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

			EmitSignal(nameof(ViewPortChanged), GetViewPortGlobalRect());
		}
		return;
	}

	internal void ZoomInc()
	{
		if (Zoom.x < 32)
		{
			Zoom += new Vector2(0.1f, 0.1f);

			EmitSignal(nameof(ViewPortChanged), GetViewPortGlobalRect());
		}
	}
}

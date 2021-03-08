using Godot;
using System;

public class MapCamera2D : Camera2D
{
	[Signal]
	public delegate void ViewPortChanged(Rect2 rect);

	private MoveTo moveTo = MoveTo.NULL;

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

		this.Position += changed;

		EmitSignal(nameof(ViewPortChanged), GetViewPortGlobalRect());
	}

	public Rect2 GetViewPortGlobalRect()
	{
		var rect = GetViewportRect();
		rect.Size *= this.Zoom;
		rect.Position = Position - rect.Size/2;

		return rect;
	}

	internal void StartMove(string direct)
	{
		Enum.TryParse(direct, out moveTo);
	}

    internal void UpdateMoveLimit(Vector2 pos)
    {
        if(LimitLeft > pos.x)
        {
			LimitLeft = (int)pos.x;
		}
		if (LimitRight < pos.x)
		{
			LimitRight = (int)pos.x;
		}
		if (LimitTop > pos.y)
        {
			LimitTop = (int)pos.y;
        }
		if(LimitBottom < pos.y)
        {
			LimitBottom = (int)pos.y;
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

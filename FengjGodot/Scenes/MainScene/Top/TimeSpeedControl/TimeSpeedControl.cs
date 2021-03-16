using Godot;
using System;

public class TimeSpeedControl : PanelContainer
{
    [Signal]
    public delegate void DaysInc();

    public override void _Ready()
    {
        
    }

	private void _on_Timer_timeout()
	{
		//if (isPause)
		//{
		//	return;
		//}

		EmitSignal(nameof(DaysInc));
	}
}

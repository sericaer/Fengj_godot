using Godot;
using System;

public class MainScene : Control
{

	Map map;

	public override void _Ready()
	{
		map = GetNode<Map>("Map");
	}

	private void _on_ButtonDirect_mouse_entered(String direct)
	{
		GD.Print(direct);
		map.camera.StartMove(direct);
	}


	private void _on_ButtonDirect_mouse_exited()
	{
		map.camera.StopMove();
	}

}

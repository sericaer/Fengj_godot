using Godot;
using System;

using Fengj;

public class MainScene : Control
{

	Map map;

	public override void _Ready()
	{
		Facade.modder = Modder.Load(GlobalPath.mod);
		Facade.runner = Runner.Gen(Facade.modder.terrainDefs);

		map = GetNode<Map>("Map");
		map.gmObj = Facade.runner.map;
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

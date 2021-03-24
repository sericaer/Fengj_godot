using Fengj.Clan;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClanTable : Control
{
	[Signal]
	internal delegate void ClickClan(string key);

	IEnumerable<ClanBase> clans;

	public override void _Ready()
	{
		
	}

	internal void SetGmObj(IEnumerable<ClanBase> clans)
	{
		this.clans = clans;

		var rowData = clans.Select(x => new Godot.Collections.Array() { x.name, x.origin, x.popNum.ToString() });

		var param = new Godot.Collections.Array(rowData);

		GetNode("Table").Call("set_rows", new object[] { param });
	}

	private void _on_Table_CLICK_ROW(Godot.Collections.Array rowData)
	{
		var name = rowData[0] as String;
		var origin = rowData[1] as String;

		var rslt = clans.Single(x => x.name == name && x.origin == origin);

		EmitSignal(nameof(ClickClan), rslt.key);
	}
}


public class GodotData<T> : Godot.Object
{
	public T data;
}

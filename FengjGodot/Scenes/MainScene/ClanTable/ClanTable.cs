using Fengj.Clan;
using Godot;
using System;
using System.Linq;

public class ClanTable : Control
{
    public override void _Ready()
    {
        
    }

    internal void SetGmObj(IClanManager clanManager)
    {
        var rowData = clanManager.Select(x => new Godot.Collections.Array() { x.name, x.origin, x.popNum.ToString() });

        var param = new Godot.Collections.Array(rowData);

        GetNode("Table").Call("set_rows", new object[] { param });
    }
}

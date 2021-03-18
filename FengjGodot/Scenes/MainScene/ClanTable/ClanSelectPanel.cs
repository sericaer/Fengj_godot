using Fengj.Clan;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClanSelectPanel : VBoxContainer
{
    IEnumerable<IClan> clans;

    ClanTable table;

    Control confirmPanel;
    
    public override void _Ready()
    {
        table = GetNode<ClanTable>("Table");
        confirmPanel = GetNode<Control>("Table/ConfirmPanel");
    }

    internal void SetGmObj(IEnumerable<IClan> clans)
    {
        this.clans = clans;
        table.SetGmObj(clans);
    }

    private void _on_Table_ClickClan(string key)
    {
        var selectClans = clans.Single(x => x.key == key);
        GD.Print(selectClans.name);
    }
}

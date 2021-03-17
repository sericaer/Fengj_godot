using Fengj.Clan;
using Godot;
using System;

public class Clan : PanelContainer
{
    Label popNum;

    public override void _Ready()
    {
        popNum = GetNode<Label>("");
    }

    private IClanManager clanManager;

    internal void SetGmObj(IClanManager clanManager)
    {
        this.clanManager = clanManager;

    }
}

using Fengj.Clan;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClanSelectPanel : VBoxContainer
{
	[Signal]
	public delegate void SelectedClan(string key);

	IEnumerable<IClan> clans;

	ClanTable table;

	Control confirmPanel;

	IClan selectedClan;

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
		selectedClan = clans.Single(x => x.key == key);
		confirmPanel.Visible = true;

		confirmPanel.GetNode<Label>("VBoxContainer/Label").Text = $"{selectedClan.origin}-{selectedClan.name}";
	}

	private void _on_ButtonConfirm_pressed()
	{
		EmitSignal(nameof(SelectedClan), selectedClan.key);
		QueueFree();
	}
	
	private void _on_ButtonClose_pressed()
	{
		QueueFree();
	}
	private void _on_ButtonCancel_pressed()
	{
		selectedClan = null;
		confirmPanel.Visible = false;
	}
}







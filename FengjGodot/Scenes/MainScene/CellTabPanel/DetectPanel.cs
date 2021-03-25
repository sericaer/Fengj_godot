using Fengj.Clan;
using Fengj.Map;
using Fengj.Task;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Linq;
using System.Reactive.Linq;

class DetectPanel : Panel
{
    public ICell cell { get; set; }

    TaskData task { get; set; }

    Panel progressPanel;
    ProgressBar progressBar;
    Button detectButton;

    public override void _Ready()
    {
        detectButton = GetNode<Button>("DetectButton");
        progressPanel = GetNode<Panel>("ProgressPanel");
        progressBar = progressPanel.GetNode<ProgressBar>("VBoxContainer/ProgressBar");
    }

    internal void SetCell(ICell cell)
    {
        this.cell = cell;

        var task = TaskManager.inst.getCellTask(cell);
        if(task != null)
        {
            SetTask(task);
        }
    }

    private void SetTask(TaskData task)
    {
        progressPanel.Visible = true;
        this.task = task;
        this.task.WhenPropertyValueChanges(x => x.percent).Subscribe(x => progressBar.Value = x).EndWith(this);
        this.task.WhenPropertyValueChanges(x => x.isFinsihed).Subscribe(x => { if (x) this.Visible = false; }).EndWith(this);
        this.task.WhenPropertyValueChanges(x => x.isCanceled).Subscribe(x => { if (x) { progressPanel.Visible = false; progressBar.Value = 0; } }).EndWith(this);

        Observable.CombineLatest(this.task.WhenPropertyValueChanges(x => x.difficulty), this.task.WhenPropertyValueChanges(x => x.speedDetail),
                (diffculty, speedDetail) =>
                {
                    var rslt = $"diffculty:{diffculty}" + " \n" + String.Join("\n", speedDetail.Select(x => $"{x.desc}:{x.value}"));
                    GD.Print(rslt);
                    return rslt;
                }).Subscribe(desc => progressBar.HintTooltip = desc).EndWith(this);
    }

    private void _on_DetectedButton_pressed()
    {
        var clanSelectPanel = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene/ClanTable/ClanSelectPanel.tscn").Instance() as ClanSelectPanel;
        this.GetParentRecursively<CellTabPanel>().AddChild(clanSelectPanel);
        clanSelectPanel.SetGmObj(ClanManager.inst);

        clanSelectPanel.Connect("SelectedClan", this, nameof(_on_CreateDetectTask));
    }


    private void _on_CreateDetectTask(string clanKey)
    {
        var clans = ClanManager.inst.Where(x => x.key == clanKey);
        task = new CellDetectTask(cell, clans.ToList());
        TaskManager.inst.AddTask(task);

        SetTask(task);
    }

    private void _on_DetectCancelButton_pressed()
    {
        TaskManager.inst.Cancel(task);
    }
}

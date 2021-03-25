using Fengj.Task;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;

class TaskPanel : PanelContainer
{
    public TaskData gmObj
    {
        get
        {
            return _gmObj;
        }
        set
        {
            _gmObj = value;

            value.WhenPropertyValueChanges(x => x.percent).Subscribe(x => progressBar.Value = x).EndWith(this);
        }
    }


    ProgressBar progressBar;
    public Button button;

    private TaskData _gmObj;

    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("ProgressBar");
        button = GetNode<Button>("Button");
    }
}

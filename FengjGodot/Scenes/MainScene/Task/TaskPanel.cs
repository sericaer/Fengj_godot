using Fengj.Task;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;

class TaskPanel : PanelContainer
{
    public Fengj.Task.Task gmObj
    {
        get
        {
            return _gmObj;
        }
        set
        {
            _gmObj = value;

            value.WhenPropertyValueChanges(x => x.percent).Subscribe(x => progressBar.Value = x);
        }
    }


    ProgressBar progressBar;

    private Fengj.Task.Task _gmObj;

    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("ProgressBar");
    }
}

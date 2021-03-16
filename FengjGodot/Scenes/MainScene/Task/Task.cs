using Fengj.Task;
using Godot;
using ReactiveMarbles.PropertyChanged;
using System;

public class Task : PanelContainer
{
    public ITask gmObj
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

    private ITask _gmObj;

    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("ProgressBar");
    }
}

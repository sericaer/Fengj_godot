using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GlobalPath
{
    public static string mod
    {
        get
        {
            return run + "mods/";
        }
    }

    public static string save
    {
        get
        {
            return run + "saves/";
        }
    }

    private static string run
    {
        get
        {
            if (OS.HasFeature("ReleaseApp"))
            {
                return System.IO.Path.GetDirectoryName(OS.GetExecutablePath()) + "/";

            }

            return ProjectSettings.GlobalizePath("res://Release/Fengj/");
        }
    }
}


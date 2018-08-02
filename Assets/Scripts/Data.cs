using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts;

/// <summary>
/// Главный класс из которого берутся данные
/// </summary>
public class Data
{
    public void StartBugSelection()
    {
        foreach (var bug in Bugs)
        {
            bug.StartAction();
        }
    }

    public IList<Bug> Bugs { get; set; }

    public Map Map { get; set; }

}
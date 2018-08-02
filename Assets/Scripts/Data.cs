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
        BugsCollection.StartExecution();
    }

    public BugCollection BugsCollection;

    public Map Map { get; set; }

}
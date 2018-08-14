using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using UnityEngine;

/// <summary>
/// Класс, в котором хранится коллекция жуков
/// </summary>
[DataContract]
public class BugCollection
{
    /// <summary>
    ///     Максимальное количество команд, которое может выполнить жук за один ход  
    /// </summary>
    public static readonly int MaxStepsBug = 64;

    [DataMember]
    public int GenerationNumber;

    [DataMember]
    public int CountBugs;

    public void AddBug(List<Bug> bugs)
    {
        foreach (Bug bug in bugs)
        {
            Bugs.Add(bug);
            CountBugs++;
        }

        bugs.Clear();
    }
    [DataMember]
    public List<Bug> Bugs { get; set; }

    public BugCollection()
    {
        Bugs = new List<Bug>();
        CountBugs = 0;
        GenerationNumber = 0;
    }

    public BugCollection(int countBug)
    {
        Bugs = new List<Bug>();
        for (int i = 0; i < countBug; i++)
        {
            Bugs.Add(new Bug(Color.white));
            CountBugs++;
        }
    }

    /// <summary>
    /// Начать действие над коллекцией жуков
    /// </summary>
    public void StartExecution()
    {
        foreach (var bug in Bugs)
        {
            if (bug.Health == 0)
            {
                ControlScript.DeadBugs.Add(bug);
            }
            else
            {
                bug.StartAction();
            }
        }

        foreach (Bug bug in ControlScript.DeadBugs)
        {
            Bugs.Remove(bug);
            Data.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = CellEnum.TypeOfCell.Empty;
            Data.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
            CountBugs--;
            Data.NumberDeadBugs++;
            if (ControlScript.BestBugs.Count < Data.BugCount)
            {
                ControlScript.BestBugs.Add(bug);
            }
            else
            {
                if (ControlScript.BestBugs.Exists(x => x.LifeTime < bug.LifeTime))
                {
                    ControlScript.BestBugs.Add(bug);
                    ControlScript.BestBugs.Remove(ControlScript.BestBugs[0]);
                    SortBugs();
                }
            }
        }

        ControlScript.DeadBugs.Clear();
    }

    public void NewGeneration()
    {
        GenerationNumber++;
        foreach (var bug in Bugs)
        {
            Data.NumberDeadBugs++;
            Data.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = CellEnum.TypeOfCell.Empty;
            Data.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
            if (ControlScript.BestBugs.Count < Data.BugCount)
            {
                ControlScript.BestBugs.Add(bug);
            }
            else
            {
                if (ControlScript.BestBugs.Exists(x => x.LifeTime < bug.LifeTime))
                {
                    ControlScript.BestBugs.Add(bug);
                    ControlScript.BestBugs.Remove(ControlScript.BestBugs[0]);
                    SortBugs();
                }
            }
        }

        List<Bug> bugs = new List<Bug>();
        for (int i = 0; i < Data.BugCount * 10; i++)
        {
            bugs.Add(
                new Bug(
                    ControlScript.BestBugs[i / 10].color,
                    new Genome(ControlScript.BestBugs[i / 10].Gene.GenomeMutate(Data.Rnd.Next(0, 2)))));
        }

        CountBugs = bugs.Count;
        Bugs = bugs;
    }

    /// <summary>
    /// Сортировка жуков по значению прожитого времени для дальнейшего отбора
    /// </summary>
    private void SortBugs()
    {
        ControlScript.BestBugs = ControlScript.BestBugs.OrderBy(x => x.LifeTime).ToList();
    }
}


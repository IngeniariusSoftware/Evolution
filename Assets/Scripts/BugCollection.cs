using System;
using System.Collections.Generic;
using System.IO;
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

    /// <summary>
    /// Количество всех родившихся жуков      
    /// </summary>
    public int CountBirthBugs = 0;

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
            CountBirthBugs++;
            if (CountBirthBugs % (Data.BugCount * 10) == 0)
            {
                GenerationNumber++;
            }
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
    /// Начинаем ход поколения жуков
    /// </summary>
    public void StartExecution()
    {
        foreach (var bug in Bugs)
        {
            if (bug.LifeTime == 0 || bug.LifeTime == Bug.MaxLifeTime)
            {
                ControlScript.DeadBugs.Add(bug);
            }
            else
            {
                bug.StartAction();
            }
        }
    }

    /// <summary>
    /// Удалить жуков из тещего стека
    /// </summary>
    public void DeleteBugs()
    {
        foreach (Bug bug in ControlScript.DeadBugs)
        {
            if (CountBugs > Data.BugCount)
            {
                Bugs.Remove(bug);
                CountBugs--;
                Data.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = Cell.TypeOfCell.Empty;
                Data.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
                Data.NumberDeadBugs++;
            }
        }

        ControlScript.DeadBugs.Clear();
    }


    /// <summary>
    /// Сгенирировать новое поколение на основе предыдущего
    /// </summary>
    public void NewGeneration()
    {
        GenerationNumber++;
        CountBirthBugs = 0;
        var sw = new StreamWriter("stat.txt", true);
        sw.WriteLine("{0} {1}", GenerationNumber, Data.CurrentGameStep);
        sw.Close();

        List<Bug> bugs = new List<Bug>();

        foreach (var bug in Bugs)
        {
            Data.NumberDeadBugs++;
            Data.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = Cell.TypeOfCell.Empty;
            Data.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
            for (int i = 0; i < 10 && Map.CellLists[(int)Cell.TypeOfCell.Empty].Count > 0; i++)
            {
                bugs.Add(new Bug(bug.color, new Genome(bug.Gene.GenomeMutate(Data.Rnd.Next(0, 2)))));
            }
        }

        CountBugs = bugs.Count;
        Bugs = bugs;
        if (GenerationNumber % 10 == 0)
        {
            SavesManager.SaveGame("autosave");
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

/// <summary>
/// Класс, в котором хранится коллекция жуков
/// </summary>
public class BugCollection
{
    public int Count;

    public void AddBug(List<Bug> bugs)
    {
        foreach (Bug bug in bugs)
        {
            Bugs.Add(bug);
            Count++;
        }

        bugs.Clear();
    }

    public List<Bug> Bugs { get; set; }

    public BugCollection()
    {
        Bugs = new List<Bug>();
        Count = 0;
    }

    public BugCollection(int countBug)
    {
        Bugs = new List<Bug>();
        for (int i = 0; i < countBug; i++)
        {
            Bugs.Add(new Bug());
            Count++;
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
            Map.WorldMap[bug.Coordinate.Y, bug.Coordinate.X].CellType = CellEnum.TypeOfCell.Empty;
            Map.WorldMap[bug.Coordinate.Y, bug.Coordinate.X].LinkedBug = null;
            Count--;
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
        foreach (var bug in Bugs)
        {
            Map.WorldMap[bug.Coordinate.Y, bug.Coordinate.X].CellType = CellEnum.TypeOfCell.Empty;
            Map.WorldMap[bug.Coordinate.Y, bug.Coordinate.X].LinkedBug = null;
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
            bugs.Add(new Bug(new Genome(ControlScript.BestBugs[i / 10].Gene.GenomeMutate(Data.Rnd.Next(0, 2)))));
        }

        Count = bugs.Count;
        Bugs = bugs;
    }

    /// <summary>
    /// Сортировка жуков по значению прожитого времени для дальнейшего отбора
    /// </summary>
    private void SortBugs()
    {
        ControlScript.BestBugs = ControlScript.BestBugs.OrderBy(x => x.LifeTime).ToList();
        //Bugs= Array.Sort(Bugs.ToArray());
    }


    ///// <summary>
    ///// Конструктор выполняющий полное копирование прилетевших значений
    ///// </summary>
    ///// <param name="inBugs"></param>
    //public BugCollection(IList<Bug> inBugs)
    //{
    //    Bugs = inBugs.Select(x => x).ToList();
    //}

    ///// <summary>
    ///// Создает новое поколение жуков
    ///// </summary>
    //public void CreateNewGeneration()
    //{
    //}




}


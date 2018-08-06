using System.Collections.Generic;
using System.Linq;

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
        List<Bug> deadBugs = new List<Bug>();
        foreach (var bug in Bugs)
        {

            if (bug.Health == 0)
            {
                if (Count > 10)
                {
                    deadBugs.Add(bug);
                    Count--;
                }
            }
            else
            {
                bug.StartAction();
            }
        }

        foreach (var bug in deadBugs)
        {
            Map.WorldMap[bug.Coordinate.Y, bug.Coordinate.X].CellType = CellEnum.TypeOfCell.Empty;
            Map.WorldMap[bug.Coordinate.Y, bug.Coordinate.X].LinkedBug = null;
            Bugs.Remove(bug);
        }
    }

    public void NewGeneration()
    {
        
        List<Bug> bugs = new List<Bug>();
        for (int i = 0; i < Count * 10; i++)
        {
            bugs.Add(new Bug(new Genome(Bugs[i / 10].Gene.GenomeMutate(Data.Rnd.Next(0,2)))));
        }

        foreach (var bug in Bugs)
        {
            Map.WorldMap[bug.Coordinate.Y, bug.Coordinate.X].CellType = CellEnum.TypeOfCell.Empty;
            Map.WorldMap[bug.Coordinate.Y, bug.Coordinate.X].LinkedBug = null;
        }

        Count = bugs.Count;
        Bugs = bugs;
    }

    ///// <summary>
    ///// Сортировка жуков по значению здоровья для дальнейшего отбора
    ///// </summary>
    //private void SortBugs()
    //{
    //    Bugs = Bugs.OrderBy(x => x.Health).ToList();
    //    //Bugs= Array.Sort(Bugs.ToArray());
    //}


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


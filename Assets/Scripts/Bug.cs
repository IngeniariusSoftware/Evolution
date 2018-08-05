using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bug : IComparable<Bug>
{
    public Coordinates Coordinate { get; set; }

    private int _health;

    public int Health
    {
        get
        {
            return _health;
        }

        set
        {
            if (value > -1 && value <= Data.MaxBugHealth)
            {
                _health = value;
            }
            else
            {
                if (value < 0)
                {
                    _health = 0;
                }
                else
                {
                    _health = 128;
                }
            }
        }
    }

    public int GenerationNumber { get; set; }

    public Genome Gene{ get; set; }

    private int _currentGenePosition;

    public int CurrentGenePosition
    {
        get
        {
            return _currentGenePosition;
        }

        set
        {
            _currentGenePosition = value % 64;
        }
    }

    public int Direction { get; set; }

    public void StartAction()
    {

    }

    public int NextGenePosition(int shift)
    {
        return (CurrentGenePosition + shift) % 64;
    }

    public Bug(Coordinates coordinate, Genome gene, int generationNumber = 0)
    {
        Coordinate = coordinate;
        Gene = gene;
        Health = Data.StartBugHealth;
        GenerationNumber = generationNumber;
        CurrentGenePosition = 0;
        Direction = Random.Range(0, 8);
    }

    /// <summary>
    /// Сортировка жуков для формирования генома
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Bug other)
    {
        if (this.Health > other.Health)
        {
            return -1;
        }

        if (this.Health == other.Health)
        {
            return 0;
        }

        return 1;
    }

    private static Coordinates BugDestination;

    public delegate bool BugCommand(Bug bug);

    public static BugCommand[] MasBugCommands = {Move, CheckCell, Take, Rotate, Multiply, Push, CheckHealth, Attack, Share};

    public static bool DoCommand(Bug bug)
    {
        BugDestination = Coordinates.CoordinateShift[((bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)]) % 8)]
                      + bug.Coordinate;
        if (MasBugCommands.Length > bug.Gene.genome[bug.CurrentGenePosition])
        {
            return MasBugCommands[bug.CurrentGenePosition].Invoke(bug);
        }
        else
        {
            return GenomJump(bug);
        }
    }

    private static bool Move(Bug bug)
    {
        switch (Map.WorldMap[BugDestination.Y, BugDestination.X].CellType)
        {
            case CellEnum.TypeOfCell.Empty:
                {
                    bug.Coordinate = BugDestination; // Куча событий +
                    break;
                }
            case CellEnum.TypeOfCell.Food:
                {
                    bug.Coordinate = BugDestination;  // Куча событий +
                    bug.Health += Data.FoodValue;
                    break;
                }
            case CellEnum.TypeOfCell.Poison:
                {
                    bug.Health = 0;  // Куча событий +
                    break;
                }
        }

        return true;
    }

    private static bool CheckCell(Bug bug)
    {
        bug.CurrentGenePosition += (int)Map.WorldMap[BugDestination.Y, BugDestination.X].CellType + 1;
        return false;
    }

    private static bool IsFriend(Bug bug)
    {
        // Проверка генома 
        bug.CurrentGenePosition += (int)Map.WorldMap[BugDestination.Y, BugDestination.X].CellType + 1;
        return false;
    }

    private static bool Take(Bug bug)
    {
        switch (Map.WorldMap[BugDestination.Y, BugDestination.X].CellType)
        {
            case CellEnum.TypeOfCell.Food:
                {
                    Map.WorldMap[BugDestination.Y, BugDestination.X].CellType = CellEnum.TypeOfCell.Empty;
                    bug.Health += Data.FoodValue;
                    break;
                }

            case CellEnum.TypeOfCell.Poison:
                {
                    Map.WorldMap[BugDestination.Y, BugDestination.X].CellType = CellEnum.TypeOfCell.Food;
                    break;
                }
        }

        return true;
    }

    private static bool Rotate(Bug bug)
    {
        bug.Direction = bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)] % 8;
        bug.CurrentGenePosition += 1;
        return false;
    }

    private static bool Multiply(Bug bug)
    {
        // Логика

        return true;
    }

    private static bool Push(Bug bug)
    {
        // Логика

        return true;
    }

    private static bool CheckHealth(Bug bug)
    {
        if (bug.Health > bug.Gene.genome[bug.NextGenePosition(1)] * Data.MaxBugHealth / Data.LengthGenome)
        {
            bug.CurrentGenePosition = bug.NextGenePosition(2);
        }
        else
        {
            bug.CurrentGenePosition = bug.NextGenePosition(3);
        }

        return false;
    }

    private static bool Attack(Bug bug)
    {
        if (Map.WorldMap[BugDestination.Y, BugDestination.X].LinkedBug != null)
        {
            if (Map.WorldMap[BugDestination.Y, BugDestination.X].LinkedBug.Health >= bug.Health * 2)
            {
                Map.WorldMap[BugDestination.Y, BugDestination.X].LinkedBug.Health += bug.Health;
                bug.Health = 0;
            }
            else
            {
                if (Map.WorldMap[BugDestination.Y, BugDestination.X].LinkedBug.Health * 2 <= bug.Health)
                {
                    bug.Health += Map.WorldMap[BugDestination.Y, BugDestination.X].LinkedBug.Health;
                    Map.WorldMap[BugDestination.Y, BugDestination.X].LinkedBug.Health = 0;
                    bug.CurrentGenePosition += 1;
                }
                else
                {
                    bug.CurrentGenePosition += 2;
                }
            }
        }
        else
        {
            bug.CurrentGenePosition += 3;
        }

        return true;
    }

    private static bool Share(Bug bug)
    {
        if (Map.WorldMap[BugDestination.Y, BugDestination.X].LinkedBug != null)
        {
            Map.WorldMap[BugDestination.Y, BugDestination.X].LinkedBug.Health += (int)(bug.Health * 0.4);
            bug.Health = (int)(bug.Health * 0.6);
            bug.CurrentGenePosition += 1;
        }
        else
        {
            bug.CurrentGenePosition += 2;
        }

        return false;
    }

    private static bool GenomJump(Bug bug)
    {
        bug.CurrentGenePosition = bug.NextGenePosition(bug.Gene.genome[bug.CurrentGenePosition]);
        return false;
    }
}


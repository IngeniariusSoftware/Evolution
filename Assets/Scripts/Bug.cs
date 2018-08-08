using System;
using System.Diagnostics;
using UnityEngine;

public class Bug
{
    public Coordinates LastPosition { get; set; }

    public Coordinates CurrentPosition { get; set; }

    public int LifeTime { get; set; }

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
                _health = 0;
            }
        }
    }

    public int GenerationNumber { get; set; }

    public Genome Gene { get; set; }

    private int _currentGenePosition;

    public int CurrentGenePosition
    {
        get
        {
            return _currentGenePosition;
        }

        set
        {
            _currentGenePosition = value % Data.LengthGenome;
        }
    }

    public int Direction { get; set; }

    public void StartAction()
    {
        LastPosition = CurrentPosition;   
        int countSteps = 0;
        bool isEnd = false;
        Health--;
        LifeTime++;
        while (countSteps < Data.MaxStepsBug && !isEnd)
        {
            isEnd = DoCommand(bug: this);
            countSteps++;
        }

        if (countSteps == Data.MaxStepsBug && !isEnd)
        {
            Health = 0;
        }
    }

    public int NextGenePosition(int shift)
    {
        return (CurrentGenePosition + shift) % Data.LengthGenome;
    }

    public Bug(Genome genome = null, Coordinates currentPosition = null)
    {
        if (currentPosition == null)
        {
            do
            {
                currentPosition = Coordinates.RandomCoordinates(Data.MapSize.Y, Data.MapSize.X);
            }
            while (Map.WorldMap[currentPosition.Y, currentPosition.X].CellType != CellEnum.TypeOfCell.Empty);
        }

        CurrentPosition = currentPosition;
        Map.WorldMap[currentPosition.Y, currentPosition.X].CellType = CellEnum.TypeOfCell.Bug;
        if (genome == null)
        {
            Gene = new Genome();
        }
        else
        {
            Gene = genome;
        }

        Health = Data.StartBugHealth;
        GenerationNumber = 0;
        LifeTime = 0;
        CurrentGenePosition = 0;
        Direction = Data.Rnd.Next(0, 8);
        Map.WorldMap[currentPosition.Y, currentPosition.X].LinkedBug = this;
    }
    
    private static Cell DestinationCell;

    public delegate bool BugCommand(Bug bug);

    //TODO Команды  Move, Rotate, CheckCell, Take, Multiply, Push, CheckHealth, Attack, Share, Photosynthesize , IsFriend Вспомогательная команда, поэтому не вносится
    public static BugCommand[] MasBugCommands = { Move, Rotate, CheckCell, Take, Multiply, Push, CheckHealth, Attack, Share};

    public static bool DoCommand(Bug bug)
    {
        Coordinates destination =
            Coordinates.CoordinateShift[((bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)]) % 8)]
            + bug.CurrentPosition;
        DestinationCell = Map.WorldMap[destination.Y, destination.X];
        if (MasBugCommands.Length > bug.Gene.genome[bug.CurrentGenePosition])
        {
            return MasBugCommands[bug.Gene.genome[bug.CurrentGenePosition]].Invoke(bug);
        }
        else
        {
            return GenomJump(bug);
        }
    }

    private static bool Move(Bug bug)
    {
        bug.CurrentGenePosition += (int)DestinationCell.CellType + 1;
        Bug neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.CurrentGenePosition++;
        }

        switch (DestinationCell.CellType)
        {
            case CellEnum.TypeOfCell.Empty:
                {
                    Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
                    Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = CellEnum.TypeOfCell.Empty;
                    bug.CurrentPosition = DestinationCell.Coordinate;
                    DestinationCell.CellType = CellEnum.TypeOfCell.Bug;
                    DestinationCell.LinkedBug = bug;
                    break;
                }

            case CellEnum.TypeOfCell.Berry:
                {
                    Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
                    Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = CellEnum.TypeOfCell.Empty;
                    bug.CurrentPosition = DestinationCell.Coordinate;
                    DestinationCell.CellType = CellEnum.TypeOfCell.Bug;
                    DestinationCell.LinkedBug = bug;
                    bug.Health += Data.BerryValue;
                    break;
                }

            case CellEnum.TypeOfCell.MineralBerry:
                {
                    Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
                    Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = CellEnum.TypeOfCell.Empty;
                    bug.CurrentPosition = DestinationCell.Coordinate;
                    DestinationCell.CellType = CellEnum.TypeOfCell.Bug;
                    DestinationCell.LinkedBug = bug;
                    bug.Health += Data.MineralBerryValue;
                    break;
                }

            case CellEnum.TypeOfCell.Poison:
                {
                    bug.Health = 0;
                    break;
                }
        }

        return true;
    }

    private static bool CheckCell(Bug bug)
    {
        bug.CurrentGenePosition += (int)DestinationCell.CellType + 1;
        Bug neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.CurrentGenePosition++;
        }

        return false;
    }

    private static bool Take(Bug bug)
    {
        bug.CurrentGenePosition += (int)DestinationCell.CellType + 1;
        Bug neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.CurrentGenePosition++;
        }

        switch (DestinationCell.CellType)
        {
            case CellEnum.TypeOfCell.Berry:
                {
                    DestinationCell.CellType = CellEnum.TypeOfCell.Empty;
                    bug.Health += Data.BerryValue;
                    break;
                }

            case CellEnum.TypeOfCell.MineralBerry:
                {
                    DestinationCell.CellType = CellEnum.TypeOfCell.Empty;
                    bug.Health += Data.MineralBerryValue;
                    break;
                }

            case CellEnum.TypeOfCell.Poison:
                {
                    DestinationCell.CellType = CellEnum.TypeOfCell.Berry;
                    break;
                }
        }

        return true;
    }

    private bool IsFriendBug(Bug bug)
    {
        bool isDifference = false;
        for (int i = 0; i < Gene.genome.Length; i++)
        {
            if (Gene.genome[i] != bug.Gene.genome[i])
            {
                if (isDifference)
                {
                    return false;
                }
                else
                {
                    isDifference = true;
                }
            }
        }

        return true;
    }

    private static bool Rotate(Bug bug)
    {
        bug.Direction = bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)] % 8;
        bug.CurrentGenePosition += 2;
        return false;
    }

    private static bool Multiply(Bug bug)
    {
        bug.Health -= Data.MuptiplyCost;
        bool isBorn = false;
        for (int i = 0; i < 8 && !isBorn; i++)
        {
            Coordinates birthCoordinate = bug.CurrentPosition + Coordinates.CoordinateShift[i];
            if (Map.WorldMap[birthCoordinate.Y, birthCoordinate.X].CellType == CellEnum.TypeOfCell.Empty)
            {
                Bug childBug = new Bug(
                    genome: new Genome(bug.Gene.GenomeMutate(Data.Rnd.Next(0, 2))),
                    currentPosition: birthCoordinate);
                ControlScript.childs.Add(childBug);
                isBorn = true;
                childBug.Health = bug.Health / 2;
            }
        }

        bug.CurrentGenePosition++;
        bug.Health = bug.Health / 2;
        return true;
    }

    private static bool Push(Bug bug)
    {
        bug.CurrentGenePosition += (int)DestinationCell.CellType + 1;
        Bug neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.CurrentGenePosition++;
        }

        if (DestinationCell.CellType != CellEnum.TypeOfCell.Wall)
        {
            Coordinates pushDestination = Coordinates.CoordinateShift[((bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)]) % 8)] + DestinationCell.Coordinate;
            Cell pushDestinationCell = Map.WorldMap[pushDestination.Y, pushDestination.X];
            if (pushDestinationCell.CellType == CellEnum.TypeOfCell.Empty)
            {
                pushDestinationCell.CellType = DestinationCell.CellType;
                if (pushDestinationCell.CellType == CellEnum.TypeOfCell.Bug)
                {
                    pushDestinationCell.LinkedBug = DestinationCell.LinkedBug;
                    pushDestinationCell.LinkedBug.CurrentPosition = pushDestination;
                    DestinationCell.LinkedBug = null;
                }

                DestinationCell.CellType = CellEnum.TypeOfCell.Empty;
            }
        }

        return true;
    }

    private static bool CheckHealth(Bug bug)
    {
        if (bug.Health > bug.Gene.genome[bug.NextGenePosition(1)] * Data.MaxBugHealth / Data.LengthGenome)
        {
            bug.CurrentGenePosition += 2;
        }
        else
        {
            bug.CurrentGenePosition += 3;
        }

        return false;
    }

    private static bool Attack(Bug bug) 
    {
        bug.CurrentGenePosition += (int)DestinationCell.CellType + 1;
        Bug neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.CurrentGenePosition++;
        }

        switch (DestinationCell.CellType)
        {
            case CellEnum.TypeOfCell.Mineral:
                {
                    DestinationCell.CellType = CellEnum.TypeOfCell.MineralBerry;
                    break;
                }

            case CellEnum.TypeOfCell.Bug:
                {

                    if (DestinationCell.LinkedBug != null)
                    {
                            DestinationCell.LinkedBug.Health -= 5;
                            bug.Health += 5;
                    }

                    break;
                }

            case CellEnum.TypeOfCell.Wall:
                {
                    break;
                }

            default:
                {
                    DestinationCell.CellType = CellEnum.TypeOfCell.Empty;
                    break;
                }
        }

        return true;
    }

    private static bool Share(Bug bug)
    {
        bug.CurrentGenePosition += (int)DestinationCell.CellType + 1;
        Bug neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.CurrentGenePosition++;
            neighbourBug.Health += 5;
            bug.Health -= 5;
        }

        return false;
    }

    private static bool Photosynthesize(Bug bug)
    {
        bug.CurrentGenePosition += (int)DestinationCell.CellType + 1;
        Bug neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.CurrentGenePosition++;
        }

        if (DestinationCell.CellType == CellEnum.TypeOfCell.Mineral)
        {
            bug.Health += 5;
        }

        return true;
    } 

    private static bool GenomJump(Bug bug)
    {
        bug.CurrentGenePosition = bug.NextGenePosition(bug.Gene.genome[bug.CurrentGenePosition]);
        return false;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BugCommands
{
    private static Coordinates Destination;

    public static bool DoCommand(Bug bug)
    {
        Coordinates destination =
            Coordinates.CoordinateShift[((bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)]) % 8)] + bug.Coordinate;

        bool isComplete = // Команда;

        bug.CurrentGenePosition += (int)Map.WorldMap[destination.Y, destination.X].CellType + 1;

        return isComplete;
    }

    private static bool Move(Bug bug)
    {
        switch (Map.WorldMap[Destination.Y, Destination.X].CellType)
        {
            case CellEnum.TypeOfCell.Empty:
                {
                    bug.Coordinate = Destination; // Куча событий +
                    break;
                }
            case CellEnum.TypeOfCell.Food:
                {
                    bug.Coordinate = Destination;  // Куча событий +
                    bug.Health += 10;
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
        return false;
    }

    private static bool Take(Bug bug)
    {
        switch (Map.WorldMap[Destination.Y, Destination.X].CellType)
        {
            case CellEnum.TypeOfCell.Food:
                {
                    Map.WorldMap[Destination.Y, Destination.X].CellType = CellEnum.TypeOfCell.Empty;
                    bug.Health += 10;
                    break;
                }
            case CellEnum.TypeOfCell.Poison:
                {
                    Map.WorldMap[Destination.Y, Destination.X].CellType = CellEnum.TypeOfCell.Food;
                    break;
                }
        }

        return true;
    }

    private static bool Rotate(Bug bug)
    {
        bug.Direction = bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)] % 8;

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
       // Логика

        return false;
    }

    private static bool Attack(Bug bug)
    {
        // Логика

        return true;
    }

    private static bool Share(Bug bug)
    {
        // Логика

        return false;
    }

    private static bool GenomJump(Bug bug)
    {
        // Логика

        return false;
    }
}

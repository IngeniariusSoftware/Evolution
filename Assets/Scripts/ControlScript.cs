using System;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

public class ControlScript : MonoBehaviour
{
    public static BugCollection bugs = new BugCollection();

    public static List<Bug> childs = new List<Bug>();

    public static List<Bug> DeadBugs = new List<Bug>();

    void Start()
    {
        StartGame();
    }

    public static void StartGame()
    {
        Map.CreateMap();
        RenderingScript.InitializeObjects();
        bugs = new BugCollection(Data.BugCount);
    }

    public static void LoadGame()
    {
        Data.CurrentGameStep = 0;
        RenderingScript.ClearRenderingObjects();
        foreach (Cell cell in Map.WorldMap)
        {
            if (cell.LinkedBug != null)
            {
                cell.LinkedBug = null;
                cell.Content = Cell.TypeOfCell.Empty;
            }
        }

        SavesManager.LoadGame();
        foreach (Bug bug in bugs.Bugs)
        {
            bug.Health = 50;
            bug.LastPosition = null;
            Cell emptyCell = Map.FindEmptyCell(Cell.TypeOfCell.Bug);
            emptyCell.LinkedBug = bug;
            emptyCell.Content = Cell.TypeOfCell.Bug;
            bug.CurrentPosition = emptyCell.Coordinate;
        }

        SavesManager.NeedLoadGame = false;
    }

    public static void NextTurn()
    {
        if (RenderingScript.RenderingMode != RenderModeEnum.RenderingType.Rewind)
        {
            Data.CurrentGameStep++;
            Map.RegionRefreshMap();
            bugs.StartExecution();
            bugs.AddBug(childs);
            bugs.DeleteBugs();
            if (bugs.CountBugs <= Data.BugCount && Map.CellLists[(int)Cell.TypeOfCell.Empty].Count > 0)
            {
                bugs.NewGeneration();
                Data.CurrentGameStep = 0;
            }
        }
        else
        {
            int currentGeneration = bugs.GenerationNumber;
            while (bugs.GenerationNumber == currentGeneration)
            {
                Data.CurrentGameStep++;
                Map.RegionRefreshMap();
                bugs.StartExecution();
                bugs.AddBug(childs);
                bugs.DeleteBugs();
                if (bugs.CountBugs <= Data.BugCount && Map.CellLists[(int)Cell.TypeOfCell.Empty].Count > 0)
                {
                    bugs.NewGeneration();
                    Data.CurrentGameStep = 0;
                }
            }
        }
    }

    void Update()
    {
        if (RenderingScript.CurrentStepsRendering <= TimeManager.TimeSpeed
            && RenderingScript.RenderingMode != RenderModeEnum.RenderingType.Rewind)
        {
            RenderingScript.MaxStepsRendering = TimeManager.TimeSpeed;
        }

        if (RenderingScript.CurrentStepsRendering > RenderingScript.MaxStepsRendering)
        {
            if (RenderingScript.RenderingMode == RenderModeManager.RenderingMode)
            {
                if (RenderingScript.RenderingMode != RenderModeEnum.RenderingType.Rewind)
                {
                    RenderingScript.CurrentStepsRendering = 0;
                }

                if (SavesManager.NeedLoadGame)
                {
                    LoadGame();
                }
                else
                {
                    NextTurn();
                }
            }
            else
            {
                RenderingScript.CurrentStepsRendering = 0;
                if (SavesManager.NeedLoadGame)
                {
                    LoadGame();
                }
                else
                {
                    NextTurn();
                }

                RenderingScript.RenderingMode = RenderModeManager.RenderingMode;
                foreach (Cell cell in Map.WorldMap)
                {
                    RenderingScript.UpdateTypeCell(cell);
                }
            }
        }
        else
        {
            if (RenderingScript.CurrentStepsRendering == 0)
            {
                if (RenderingScript.RenderingMode != RenderModeEnum.RenderingType.Rewind)
                {
                    RenderingScript.StartRendering();
                }
            }
            else
            {
                if (RenderingScript.RenderingMode == RenderModeEnum.RenderingType.Rewind)
                {
                    RenderingScript.HideObjects();
                }
                else
                {
                    RenderingScript.UpdateObjects();
                }
            }

            RenderingScript.CurrentStepsRendering++;
        }
    }
}
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
        // очистка ренедера и карты
        RenderingScript.InitializeObjects();
        Map.CreateMap();
        bugs = new BugCollection(Data.BugCount);
    }

    public static void LoadGame()
    {
        Data.CurrentGameStep = 0;
        RenderingScript.ClearRenderingObjects();
        foreach (Cell cell in Data.WorldMap)
        {
            if (cell.LinkedBug != null)
            {
                cell.LinkedBug = null;
                cell.CellType = CellEnum.TypeOfCell.Empty;
            }
        }

        SavesManager.LoadGame();
        foreach (Bug bug in bugs.Bugs)
        {
            bug.Health = 50;
            bug.LastPosition = null;
            Cell emptyCell = Map.FindEmptyCell();
            emptyCell.LinkedBug = bug;
            emptyCell.CellType = CellEnum.TypeOfCell.Bug;
            bug.CurrentPosition = emptyCell.Coordinate;
        }

        SavesManager.NeedLoadGame = false;
    }

    public static void NextTurn()
    {
        if (RenderingScript.RenderingMode != RenderModeEnum.RenderingType.Rewind)
        {
            Data.CurrentGameStep++;
            Map.RefreshMap();
            bugs.StartExecution();
            bugs.AddBug(childs);
            bugs.DeleteBugs();
            if (bugs.CountBugs <= Data.BugCount)
            {
                bugs.NewGeneration();
                Data.CurrentGameStep = 0;
            }
        }
        else
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            while (100 > startTime.ElapsedMilliseconds)
            {
                Data.CurrentGameStep++;
                Map.RefreshMap();
                bugs.StartExecution();
                bugs.AddBug(childs);
                bugs.DeleteBugs();
                if (bugs.CountBugs <= Data.BugCount)
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
                foreach (Cell cell in Data.WorldMap)
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

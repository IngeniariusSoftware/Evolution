using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour
{
    public static BugCollection bugs = new BugCollection();

    public static List<Bug> childs = new List<Bug>();

    public static List<Bug> BestBugs = new List<Bug>();

    public static List<Bug> DeadBugs = new List<Bug>();

    void Start()
    {
        RenderingScript.InitializeObjects();
        Map.CreateMap();
        bugs = new BugCollection(Data.BugCount);
    }

    public static void LoadGame()
    {
        RenderingScript.ResetRendering();
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
        Data.CurrentGameStep++;
        Map.RefreshMap();
        bugs.StartExecution();
        bugs.AddBug(childs);
        if (bugs.CountBugs <= Data.BugCount / 10)
        {
            bugs.NewGeneration();
            Data.CurrentGameStep = 0;
        }
    }

    void Update()
    {
        RenderingScript.UpdateObjects();
        if (RenderingScript.CurrentStepsRendering > RenderingScript.MaxStepsRendering)
        {
            RenderingScript.CurrentStepsRendering = 0;
            RenderingScript.MaxStepsRendering = TimeManager.TimeSpeed;
            if (!SavesManager.NeedLoadGame && RenderingScript.RenderingMode == RenderModeManager.CurrentRenderingMode)
            {
                NextTurn();
            }

            if (SavesManager.NeedLoadGame)
            {
                Data.CurrentGameStep = 0;
                LoadGame();
            }

            if (RenderingScript.RenderingMode != RenderModeManager.CurrentRenderingMode)
            {
                RenderingScript.RenderingMode = RenderModeManager.CurrentRenderingMode;
                foreach (Cell cell in Data.WorldMap)
                {
                    RenderingScript.UpdateTypeCell(cell);
                }
            }
        }
    }
}

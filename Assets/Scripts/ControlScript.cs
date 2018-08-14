using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class ControlScript : MonoBehaviour
{
    public static BugCollection bugs = new BugCollection();

    public static List<Bug> childs = new List<Bug>();

    public static List<Bug> BestBugs = new List<Bug>();

    public static List<Bug> DeadBugs = new List<Bug>();

    public static ScrollRect Scroll;

    void Start()
    {
        RenderingScript.InitializeObjects();
        Map.CreateMap();
        bugs = new BugCollection(Data.BugCount);
    }

    public static void SaveGame(string nameSaveGame)
    {
        int number = 0;
        string nameGame = nameSaveGame;
        Data.SaveGames = Directory.GetFiles(Data.SavePath, "*.json").ToList();
        while (Data.SaveGames.Contains(Data.SavePath + nameSaveGame + ".json"))
        {
            number++;
            nameSaveGame = nameGame + " (" + number + ")";
        }

        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(BugCollection));
        FileStream bugsSaveFile = new FileStream(Data.SavePath + nameSaveGame + ".json", FileMode.Create);
        jsonSerializer.WriteObject(bugsSaveFile, bugs);
        bugsSaveFile.Close();
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

        FileStream bugsLoadFile = new FileStream(UIManageScript.LoadGameName, FileMode.Open);
        DataContractJsonSerializer jsonDeserializer = new DataContractJsonSerializer(typeof(BugCollection));
        bugs = (BugCollection)jsonDeserializer.ReadObject(bugsLoadFile);
        bugsLoadFile.Close();
        foreach (Bug bug in bugs.Bugs)
        {
            bug.Health = 50;
            bug.LastPosition = null;
            Cell emptyCell = Map.FindEmptyCell();
            emptyCell.LinkedBug = bug;
            emptyCell.CellType = CellEnum.TypeOfCell.Bug;
            bug.CurrentPosition = emptyCell.Coordinate;
        }

        UIManageScript.LoadGameName = null;
        UIManageScript.NeedLoadGame = false;
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
            RenderingScript.MaxStepsRendering = TimeManageScript.TimeSpeed;
            if (!UIManageScript.NeedLoadGame && RenderingScript.RenderingMode == UIManageScript.CurrentRenderingMode)
            {
                NextTurn();
            }

            if (UIManageScript.NeedLoadGame)
            {
                Data.CurrentGameStep = 0;
                LoadGame();
            }

            if (RenderingScript.RenderingMode != UIManageScript.CurrentRenderingMode)
            {
                RenderingScript.RenderingMode = UIManageScript.CurrentRenderingMode;
                foreach (Cell cell in Data.WorldMap)
                {
                    RenderingScript.UpdateTypeCell(cell);
                }
            }
        }
    }
}

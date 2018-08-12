using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
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
        string name = nameSaveGame;
        while (Data.SaveGames.Contains(nameSaveGame))
        {
            number++;
            nameSaveGame = name + "(" + number + ")";
        }

        FileStream bugsFile = new FileStream(Data.SavePath + nameSaveGame + ".gen", FileMode.Create);
        BinaryFormatter binForm = new BinaryFormatter();
        binForm.Serialize(bugsFile, bugs);
        bugsFile.Close();
    }

    public static void LoadGame(string nameLoadGame)
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



        RenderingScript.CurrentStepsRendering = 0;
        Map.CreateMap();
        // Загрузка жуков

        FileStream bugsFile = new FileStream(Data.SavePath + nameLoadGame + ".gen", FileMode.Create);
        BinaryFormatter binForm = new BinaryFormatter();

        // Распределение по карте
        // Выставить NUll last position, 50 жизней, текущие координаты

        NextTurn();
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
        }
    }

    void Update()
    {
        RenderingScript.UpdateObjects();
        if (RenderingScript.CurrentStepsRendering > RenderingScript.MaxStepsRendering)
        {
            RenderingScript.CurrentStepsRendering = 0;
            RenderingScript.MaxStepsRendering = TimeManageScript.TimeSpeed;
            if (RenderingScript.RenderingMode != UIManageScript.CurrentRenderingMode)
            {
                if (UIManageScript.NeedLoadGame)
                {
                    
                }
                RenderingScript.RenderingMode = UIManageScript.CurrentRenderingMode;
                foreach (Cell cell in Data.WorldMap)
                {
                    RenderingScript.UpdateTypeCell(cell);
                }
            }

            NextTurn();
        }
    }
}

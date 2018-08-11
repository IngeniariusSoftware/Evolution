using System.Collections.Generic;
using System.IO;
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

    public static void SaveGame()
    {
        // Сохранение жуков
        using (var fs = new FileStream("/save", FileMode.Create))
        {

        }
    }

    public static void LoadGame()
    {
        RenderingScript.ResetRendering();
        RenderingScript.CurrentStepsRendering = 0;
        Map.CreateMap();
        // Загрузка жуков
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

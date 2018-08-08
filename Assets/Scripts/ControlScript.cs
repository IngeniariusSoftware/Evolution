using System.Collections.Generic;
using System.Threading;
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

    public static void NextTurn()
    {
        bugs.StartExecution();
        bugs.AddBug(childs);
        Map.RefreshMap();
        if (bugs.Count <= Data.BugCount / 10)
        {
            bugs.NewGeneration();
        }
    }

    void Update()
    {
        RenderingScript.UpdateObjects();
        if (Data.CurrentStepsRendering > Data.MaxStepsRendering)
        {
            Data.CurrentStepsRendering = 0;
            Data.MaxStepsRendering = TimeManageScript.TimeSpeed;
            NextTurn();
        }
    }
}

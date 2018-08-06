using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Control : MonoBehaviour
{
    public static BugCollection bugs = new BugCollection();

    public static List<Bug> childs = new List<Bug>();

    void Start()
    {
        RenderingScript.InitializeObjects();
        Map.CreateMap();
        bugs = new BugCollection(Data.BugCount);
    }

    void Update()
    {
        bugs.StartExecution();
        bugs.AddBug(childs);
        Map.RefreshMap();
        if (bugs.Count == 10)
        {
            bugs.NewGeneration();
        }

        Thread.Sleep(50);
    }
}

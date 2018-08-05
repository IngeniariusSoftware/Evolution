using System.Threading;
using UnityEngine;

public class Control : MonoBehaviour
{
    public static BugCollection bugs = new BugCollection();

    void Start()
    {
        // RenderingScript.CreateCells();
        
        RenderingScript.InitializeObjects();
        Map.CreateMap();
        bugs = new BugCollection(Data.BugCount);
    }

    void Update()
    {
        bugs.StartExecution();
        Map.RefreshMap();
        if (bugs.Count == 10)
        {
            bugs.NewGeneration();
        }

        Thread.Sleep(50);
    }
}

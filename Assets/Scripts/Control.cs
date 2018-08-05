using System.Collections;
using System.Collections.Generic;

using Assets.Scripts;

using UnityEngine;

public class Control : MonoBehaviour
{
    void Start()
    {
       // RenderingScript.CreateCells();
        Map.CreateMap();
        RenderingScript.InitializeObjects();
    }
}

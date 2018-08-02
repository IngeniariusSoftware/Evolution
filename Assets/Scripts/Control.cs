using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private void Start()
    {
        RenderingScript.CreateCells();
        Map.CreateMap();
        RenderingScript.InitializeObjects();
    }
}

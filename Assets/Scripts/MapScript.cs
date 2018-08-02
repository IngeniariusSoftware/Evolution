using System;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public class MapScript : MonoBehaviour
{
    public Map Map { get; set; }

    // Use this for initialization
    private void Start()
    {
        /* Для минимальной связи с фронтом пришлось пойти на такую хитрость,
        так как мы знаем, что оба скрипта располагаются на одном объекте */
        gameObject.GetComponent<RenderingScript>().CreateCells(Map.SizeX, Map.SizeY);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) throw new NotImplementedException();
    }
}
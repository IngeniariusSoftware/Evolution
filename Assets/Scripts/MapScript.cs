using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapScript : MonoBehaviour
{
    public int SizeX = 100;

    public int SizeY = 100;

    private float CellSizeX = 2.56f;

    private float CellSizeY = 2.56f;



	// Use this for initialization
    void Start()
    {
        /* Для минимальной связи с фронтом пришлось пойти на такую хитрость,
        так как мы знаем, что оба скрипта располагаются на одном объекте */
       gameObject.GetComponent<RenderingScript>().CreateCells(SizeX, SizeY);


        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            throw new System.NotImplementedException();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingScript : MonoBehaviour
{

    // Не совсем красивое решение, так как можно считывать размеры картинки
    // и формировать размеры клетки, но у нас единый стандарт, так что без разницы
    /// <summary>
    /// Размер клеток по абсциссе
    /// </summary>
    private float CellSizeX = 2.56f;

    /// <summary>
    /// Размер клеток по ординате
    /// </summary>
    private float CellSizeY = 2.56f; 

    /// <summary>
    /// Префаб пустой клетки
    /// </summary>
    public Transform Empty;

    /// <summary>
    /// Префаб жука
    /// </summary>
    public Transform Bug;

    /// <summary>
    /// Префаб еды
    /// </summary>
    public Transform Food;

    /// <summary>
    /// Префаб яда
    /// </summary>
    public Transform Poison;

    /// <summary>
    /// Префаб стены
    /// </summary>
    public Transform Wall;

    /// <summary>
    /// Массив префабов всех объектов
    /// </summary>
    public Transform[,] MapCells;

    /// <summary>
    /// Обновить положения всех объектов на карте
    /// </summary> 
    //public void FrameUpdate(// Передается карта)
    //{

    //}

    /// <summary>
    /// Сгенировать карту из пустых клеток
    /// </summary>
    public void CreateCells(int sizeX, int sizeY)
    {
        /* Генерируем пустые клетки на экране
        в центре экрана точка отсчета, поэтому
        начинаем генерировать со смещением */
        for (int y = -sizeY / 2; y < sizeY / 2; y++)
        {
            for (int x = -sizeX / 2; x < sizeX / 2; x++)
            {
                Instantiate(Empty, new Vector3(x: x * CellSizeX, y: y * CellSizeY), new Quaternion(0, 0, 0, 0));
            }
        }
    }



}

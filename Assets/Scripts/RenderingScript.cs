using System;

using Boo.Lang;

using UnityEditor;

using UnityEngine;

public class RenderingScript : MonoBehaviour
{
    // Не совсем красивое решение, так как можно считывать размеры картинки
    // и формировать размеры клетки, но у нас единый стандарт, так что без разницы

    /// <summary>
    ///     Размер клеток по абсциссе
    /// </summary>
    public static readonly float CellSizeX = 2.56f;

    /// <summary>
    ///     Размер клеток по ординате
    /// </summary>
    public static readonly float CellSizeY = 2.56f;

    /// <summary>
    ///     Лист префабов
    /// </summary>
    public static Transform[]Objects = new Transform[5];

    /// <summary>
    ///     Массив префабов всех объектов
    /// </summary>
    private static Transform[,] MapObjects = new Transform[Map.SizeY, Map.SizeX];

    void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            Objects[i] = Resources.Load<GameObject>("Prefabs/" + CellEnum.GetCellType(i).ToString().Replace("TypeOfCell.", "")).transform;
        }
    }

    /// <summary>
    ///     Сгенировать карту из пустых клеток
    /// </summary>
    public static void CreateCells()
    {
        /* Генерируем пустые клетки на экране
        в центре экрана точка отсчета, поэтому
        начинаем генерировать со смещением */
        for (int y = -Map.SizeY / 2; y < Math.Ceiling(Map.SizeY / 2.0); y++)
        {
            for (int x = -Map.SizeX / 2; x < Math.Ceiling(Map.SizeX / 2.0); x++)
            {
                Instantiate(Objects[(int)CellEnum.TypeOfCell.Empty], new Vector3(x * CellSizeX, y * CellSizeY), new Quaternion(0, 0, 0, 0));
            }
        }
    }

    /// <summary>
    ///     Сгенировать все объекты на карте
    /// </summary>
    public static void InitializeObjects()
    {
        for (int x = 0; x < Map.SizeX; x++)
        {
            for (int y = 0; y < Map.SizeY; y++)
            {
                if (Map.WorldMap[y, x].CellType != CellEnum.TypeOfCell.Empty)
                {
                    MapObjects[y, x] = Instantiate(
                        Objects[(int)Map.WorldMap[y, x].CellType],
                        new Vector3((x - Map.SizeX / 2) * CellSizeX, (y - Map.SizeY / 2) * CellSizeY),
                        new Quaternion(0, 0, 0, 0));
                }
            }
        }
    }

    /// <summary>
    ///     Обновить положения объекта на карте
    /// </summary>
    public static void UpdateObjects(Cell cell)
    {
        MapObjects[cell.Y, cell.X].transform.position = new Vector3(
            (cell.Y - Map.SizeY / 2) * CellSizeY,
            (cell.X - Map.SizeX / 2) * CellSizeX);
    }
}
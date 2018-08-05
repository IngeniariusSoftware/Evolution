using System;

using Boo.Lang;

using UnityEditor;

using UnityEngine;

public class RenderingScript : MonoBehaviour
{
    // Не совсем красивое решение, так как можно считывать размеры картинки
    // и формировать размеры клетки, но у нас единый стандарт, так что без разницы



    /// <summary>
    ///     Лист префабов
    /// </summary>
    public static Transform[]Objects = new Transform[5];

    /// <summary>
    ///     Массив префабов всех объектов
    /// </summary>
    private static Transform[,] MapObjects = new Transform[Data.MapSize.Y, Data.MapSize.X];

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
    //public static void CreateCells()
    //{
    //    /* Генерируем пустые клетки на экране
    //    в центре экрана точка отсчета, поэтому
    //    начинаем генерировать со смещением */
    //    for (int y = -Data.MapSize.Y / 2; y < Math.Ceiling(Data.MapSize.Y / 2.0); y++)
    //    {
    //        for (int x = -Data.MapSize.X / 2; x < Math.Ceiling(Data.MapSize.X / 2.0); x++)
    //        {
    //            Instantiate(Objects[(int)CellEnum.TypeOfCell.Empty], new Vector3(x * Data.CellSizeX, y * Data.CellSizeY), new Quaternion(0, 0, 0, 0));
    //        }
    //    }
    //}

    /// <summary>
    ///     Сгенировать все объекты на карте
    /// </summary>
    public static void InitializeObjects()
    {
        for (int x = 0; x < Data.MapSize.X; x++)
        {
            for (int y = 0; y < Data.MapSize.Y; y++)
            {
                    MapObjects[y, x] = Instantiate(
                        Objects[(int)Map.WorldMap[y, x].CellType],
                        new Vector3((x - Data.MapSize.X / 2) * Data.CellSizeX, (y - Data.MapSize.Y / 2) * Data.CellSizeY),
                        new Quaternion(0, 0, 0, 0));
            }
        }
    }

    /// <summary>
    ///     Обновить положения объекта на карте
    /// </summary>
    public static void UpdateObjects(Cell cell)
    {
        MapObjects[cell.Coordinate.Y, cell.Coordinate.X].transform.position = new Vector3(
            (cell.Coordinate.Y - Data.MapSize.Y / 2) * Data.CellSizeY,
            (cell.Coordinate.X - Data.MapSize.X / 2) * Data.CellSizeX);
    }
}
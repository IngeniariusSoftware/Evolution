using System;
using System.Collections.Generic;

using UnityEngine;

using Random = System.Random;

public static class Map
{
    #region Constants

    /// <summary>
    ///     Размер карты по y и по x (количество клеток)   
    /// </summary>
    public static readonly Coordinates Size = new Coordinates(40, 80);

    /// <summary>
    ///     Процент от общего числа клеток различных объектов на карте 
    /// </summary>
    public static readonly float[] PercentObjects = { 0, 0.07f, 0.07f, 0.2f, 0.05f, 0, 0.05f, 0.05f, 0f, 0 };

    /// <summary>
    ///     Общее количетсво клеток на карте
    /// </summary>
    public static readonly int AllCellCount = Size.X * Size.Y;

    /// <summary>
    ///     Количество типов объектов на карте
    /// </summary>
    public static int[] CountTypeObjects =
        {
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Empty]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Berry]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Poison]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Wall]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Mineral]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.MineralBerry]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Bamboo]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Sun]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Prickle]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Bug])
        };

    public static void RecalculateAllCountTypeObjects()
    {
        for (int i = 0; i < CountTypeObjects.Length; i++)
        {
            CountTypeObjects[i] = (int)(AllCellCount * PercentObjects[i]);
        }
    }

    public static void RecalculateSingleTypeObject(int i)
    {
        CountTypeObjects[i] = (int)(AllCellCount * PercentObjects[i]);
    }

    public static void CleanMap(int itemIndex)
    {
        if (Data.CurrentCountObjects[itemIndex] > CountTypeObjects[itemIndex])
        {
            var countItemsToDelete = Data.CurrentCountObjects[itemIndex] - CountTypeObjects[itemIndex];
            foreach (var cell in Data.WorldMap)
            {
                if ((int) cell.CellType == itemIndex)
                    cell.CellType = CellEnum.TypeOfCell.Empty;
            }
        }
    }

    public static System.Collections.Generic.List<System.Collections.Generic.List<Coordinates>> CellLists = new System.Collections.Generic.List<System.Collections.Generic.List<Coordinates>>();

    #endregion

    /// <summary>
    ///     Поддержание уровня еды, яда, стен и минералов на карте
    /// </summary>
    public static void RefreshMap()
    {
        if (true) // Тип генерации карты
        {

            for (int i = 1; i < CountTypeObjects.Length; i++)
            {
                if (Data.CurrentCountObjects[i] < CountTypeObjects[i]
                    && CellLists[(int)CellEnum.TypeOfCell.Empty].Count > 0)
                {
                    Cell checkCell;
                    if (CellLists[i].Count > 0)
                    {
                        Coordinates anotherCell = CellLists[i][Data.Rnd.Next(0, CellLists[i].Count)];
                        checkCell = Data.WorldMap[anotherCell.Y, anotherCell.X];
                    }
                    else
                    {
                        checkCell = FindEmptyCell();
                        checkCell.CellType = CellEnum.GetCellType(i);
                    }

                    int tryCount = 0;
                    while (Data.CurrentCountObjects[i] < CountTypeObjects[i]
                           && CellLists[(int)CellEnum.TypeOfCell.Empty].Count > 0)
                    {
                        if (tryCount < 50)
                        {
                            checkCell = FindEmptyCell(checkCell.Coordinate);
                        }
                        else
                        {
                            checkCell = FindEmptyCell();
                        }

                        if (checkCell != null)
                        {
                            checkCell.CellType = CellEnum.GetCellType(i);
                            tryCount = 0;
                        }
                        else
                        {
                            tryCount++;
                            Coordinates anotherCell = CellLists[i][Data.Rnd.Next(0, CellLists[i].Count)];
                            checkCell = Data.WorldMap[anotherCell.Y, anotherCell.X];
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < CountTypeObjects.Length; i++)
            {
                while (CellLists[(int)CellEnum.TypeOfCell.Empty].Count > 0
                       && Data.CurrentCountObjects[i] < CountTypeObjects[i])
                {
                    if (i == (int)CellEnum.TypeOfCell.Bamboo)
                    {
                        Cell checkCell = FindEmptyCell();
                        checkCell.CellType = CellEnum.TypeOfCell.Bamboo;
                        while (Data.Rnd.Next(0, 50) != 0 && CellLists[(int)CellEnum.TypeOfCell.Empty].Count > 0
                                                         && Data.CurrentCountObjects[i] < CountTypeObjects[i]
                                                         && checkCell != null)
                        {
                            checkCell = FindEmptyCell(checkCell.Coordinate);
                            if (checkCell != null)
                            {
                                checkCell.CellType = CellEnum.TypeOfCell.Bamboo;
                            }
                        }
                    }
                    else
                    {
                        FindEmptyCell().CellType = CellEnum.GetCellType(i);
                    }
                }
            }
        }
    }

    public static bool CheckRadius(Cell cell, CellEnum.TypeOfCell cellType)
    {
        for (int i = 0; i < 8; i++)
        {
            Coordinates checkCoordinate = cell.Coordinate + Coordinates.CoordinateShift[i];
            if (Data.WorldMap[checkCoordinate.Y, checkCoordinate.X].CellType == cellType)
            {
                return true;
            }
        }

        return false;
    }

    public static Cell FindEmptyCell()
    {
        Coordinates randomCoordinate = CellLists[(int)CellEnum.TypeOfCell.Empty][Data.Rnd.Next(
            0,
            CellLists[(int)CellEnum.TypeOfCell.Empty].Count)];
        return Data.WorldMap[randomCoordinate.Y, randomCoordinate.X];
    }

    /// <summary>
    /// Ищет свободную клетку вокруг клетки, с указанными координатами
    /// </summary>
    /// <param name="coordinate"> Координаты клетки, вокруг которой нужно искать </param>
    /// <returns></returns>
    public static Cell FindEmptyCell(Coordinates coordinate)
    {
        // Вместо обхода по часовой стрелки вокруг клетки, будем использовать рандомные сдвиги с запоминанием
        List<byte> randomShifts = new List<byte>(8);
        for (byte i = 0; i < randomShifts.Capacity; i++)
        {
            randomShifts.Add(i);
        }

        // Ищем свободную клетку до тех пор, пока не проверим все 8 позиций вокруг клетки, либо найдем пустую
        while (randomShifts.Count > 0)
        {
            byte shift = (byte)Data.Rnd.Next(0, randomShifts.Count);
            var checkCoordinate = coordinate + Coordinates.CoordinateShift[randomShifts[shift]];
            if (checkCoordinate.Y > -1 && checkCoordinate.Y < Size.Y && checkCoordinate.X > -1
                && checkCoordinate.X < Size.X && Data.WorldMap[checkCoordinate.Y, checkCoordinate.X].CellType
                == CellEnum.TypeOfCell.Empty)
            {
                return Data.WorldMap[checkCoordinate.Y, checkCoordinate.X];
            }

            randomShifts.Remove(randomShifts[shift]);
        }

        // Если не нашли ни одной пустой клетки, вернем null
        return null;
    }

    /// <summary>
    ///     Заполнение клеток карты каким-либо типом клетки, генерация стен
    /// </summary>
    public static void CreateMap()
    {
        InitializeCellLists();
        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                if (x == 0 || x == Size.X - 1 || y == 0 || y == Size.Y - 1)
                {
                    Data.WorldMap[y, x] = new Cell(new Coordinates(y, x), CellEnum.TypeOfCell.Wall);
                }
                else
                {
                    Data.WorldMap[y, x] = new Cell(new Coordinates(y, x), CellEnum.TypeOfCell.Empty);
                }
            }
        }

        RefreshMap();
    }

    /// <summary>
    /// Инициализия листа листов координат всех клеток карты
    /// </summary>
    public static void InitializeCellLists()
    {
        for (int i = 0; i < CountTypeObjects.Length; i++)
        {
            CellLists.Add(new System.Collections.Generic.List<Coordinates>());
        }   
    }

    public static void UpdateCellList(Cell cell, CellEnum.TypeOfCell newType)
    {
        CellLists[(int)cell.CellType].Remove(cell.Coordinate);
        CellLists[(int)newType].Add(cell.Coordinate);
        RenderingScript.UpdateTypeCell(cell);
    }
}


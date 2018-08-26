using System;

using UnityEngine;

public static class Map
{
    #region Constants

    /// <summary>
    ///     Размер карты по y и по x (количество клеток)   
    /// </summary>
    public static readonly Coordinates Size = new Coordinates(30, 30);

    /// <summary>
    ///     Процент от общего числа клеток различных объектов на карте 
    /// </summary>
    public static readonly float[] PercentObjects = { 0, 0.1f, 0.06f, 0.14f, 0.1f, 0, 0.04f, 0 };

    /// <summary>
    ///     Общее количетсво клеток на карте
    /// </summary>
    public static readonly int AllCellCount = Size.X * Size.Y;

    /// <summary>
    ///     Количество типов объектов на карте
    /// </summary>
    public static readonly int[] CountTypeObjects =
        {
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Empty]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Berry]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Poison]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Wall]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Mineral]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.MineralBerry]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Bamboo]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Bug])
        };

    #endregion

    /// <summary>
    ///     Поддержание уровня еды, яда, стен и минералов на карте
    /// </summary>
    public static void RefreshMap()
    {
        for (int i = 0; i < CountTypeObjects.Length; i++)
        {
            while (Data.CurrentCountObjects[i] < CountTypeObjects[i])
            {
                if (i == (int)CellEnum.TypeOfCell.Bamboo)
                {
                    bool find = false;
                    for (int j = 0; j < 50 && !find; j++)
                    {
                        Cell checkCell = FindEmptyCell();
                        if (CheckRadius(checkCell, CellEnum.TypeOfCell.Bamboo))
                        {
                            checkCell.CellType = CellEnum.GetCellType(i);
                            find = true;
                        }
                    }

                    if (!find)
                    {
                        FindEmptyCell().CellType = CellEnum.GetCellType(i);
                    }
                }
                else
                {
                    FindEmptyCell().CellType = CellEnum.GetCellType(i);
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
        Coordinates randomPosition;
        do
        {
            randomPosition = Coordinates.RandomCoordinates(Size.Y, Size.X);
        }
        while (Data.WorldMap[randomPosition.Y, randomPosition.X].CellType != CellEnum.TypeOfCell.Empty);

        return Data.WorldMap[randomPosition.Y, randomPosition.X];
    }

    /// <summary>
    ///     Заполнение клеток карты каким-либо типом клетки, генерация стен
    /// </summary>
    public static void CreateMap()
    {
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
}


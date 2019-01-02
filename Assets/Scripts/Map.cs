using System;
using System.Collections.Generic;

public static class Map
{
    #region Constants

    /// <summary>
    /// Размер карты по y и по x (количество клеток)   
    /// </summary>
    public static readonly Coordinates Size = new Coordinates(60, 120);

    /// <summary>
    /// Карта мира, состоящая из клеток
    /// </summary>
    public static Cell[] WorldMap = new Cell[Size.Y * Size.X];


    /// <summary>
    /// Возвращает клетку карты по заданным координатам 
    /// </summary>
    /// <param name="y"> Координата по x </param>
    /// <param name="x"> Координата по y </param>
    /// <returns> Клетка карты </returns>
    public static Cell GetMapCell(int y, int x)
    {
        return WorldMap[(Size.X * y) + x];
    }

    /// <summary>
    /// Общее количетсво клеток на карте
    /// </summary>
    public static readonly int AllCellCount = Size.X * Size.Y;

    /// <summary>
    /// Количество типов объектов на карте
    /// </summary>
    public static int[] CountTypeObjects =
        {
            (int)(AllCellCount * 0), (int)(AllCellCount * 0.015), (int)(AllCellCount * 0.015), (int)(AllCellCount * 0),
            (int)(AllCellCount * 0.015), (int)(AllCellCount * 0), (int)(AllCellCount * 0.015), (int)(AllCellCount * 0.015),
            (int)(AllCellCount * 0.05), (int)(AllCellCount * 0.015), (int)(AllCellCount * 0), (int)(AllCellCount * 0.05),
            (int)(AllCellCount * 0.05), (int)(AllCellCount * 0.05), (int)(AllCellCount * 0.05), (int)(AllCellCount * 0.05),
            (int)(AllCellCount * 0)
        };

    public static void CleanMap(int itemIndex)
    {
        if (CellLists[itemIndex].Count > CountTypeObjects[itemIndex])
        {
            var countItemsToDelete = CellLists[itemIndex].Count - CountTypeObjects[itemIndex];
            for (int i = 0; i < countItemsToDelete; i++)
            {
                var randomCoordinate = CellLists[itemIndex][Data.Rnd.Next(0, CellLists[itemIndex].Count)];
                GetMapCell(randomCoordinate.Y, randomCoordinate.X).Content = Cell.TypeOfCell.Empty;
            }
        }
    }

    public static List<List<Coordinates>> CellLists = new List<List<Coordinates>>();

    #endregion

    /// <summary>
    /// Создание поверхности
    /// </summary>
    public static void CreateGround()
    {
        for (byte i = (byte)Cell.TypeOfCell.Desert; i < (byte)Cell.TypeOfCell.Basalt + 1; i++)
        {
            if (CellLists[i].Count < CountTypeObjects[i] && CellLists[(byte)Cell.TypeOfCell.Sea].Count > 0)
            {
                Cell checkCell;
                if (CellLists[i].Count > 0)
                {
                    Coordinates anotherCell = CellLists[i][Data.Rnd.Next(0, CellLists[i].Count)];
                    checkCell = GetMapCell(anotherCell.Y, anotherCell.X);
                }
                else
                {
                    checkCell = FindSeaCell();
                    checkCell.Surface = (Cell.TypeOfCell)i;
                }

                int tryCount = 10;
                while (CellLists[i].Count < CountTypeObjects[i] && CellLists[(int)Cell.TypeOfCell.Sea].Count > 0 && tryCount > 0)
                {
                    if (FindSeaCell(checkCell.Coordinate) != null)
                    {
                        checkCell = FindSeaCell(checkCell.Coordinate);
                        checkCell.Surface = (Cell.TypeOfCell)i;
                        tryCount = 10;
                    }
                    else
                    {
                        Coordinates anotherCell = CellLists[i][Data.Rnd.Next(0, CellLists[i].Count)];
                        checkCell = GetMapCell(anotherCell.Y, anotherCell.X);
                        tryCount--;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Поддержание уровня еды, яда, стен и минералов на карте, случаное распределение
    /// </summary>
    public static void RandomRefreshMap()
    {
        for (int i = 1; i < (byte)Cell.TypeOfCell.Prickle + 1; i++)
        {
            int tryCount = 10;
            while (CellLists[(int)Cell.TypeOfCell.Empty].Count > 0 && CellLists[i].Count < CountTypeObjects[i]
                                                                   && tryCount > 0)
            {
                Cell newCell = FindEmptyCell((Cell.TypeOfCell)i);
                if (newCell != null)
                {
                    newCell.Content = (Cell.TypeOfCell)i;
                }
                else
                {
                    tryCount--;
                }
            }
        }
    }

    /// <summary>
    /// Поддержание уровня еды, яда, стен и минералов на карте, распределение регионами
    /// </summary>
    public static void RegionRefreshMap()
    {
        for (byte i = 1; i < (byte)Cell.TypeOfCell.Prickle + 1; i++)
        {
            if (CellLists[i].Count < CountTypeObjects[i] && CellLists[(byte)Cell.TypeOfCell.Empty].Count > 0)
            {
                Cell checkCell;
                if (CellLists[i].Count > 0)
                {
                    Coordinates anotherCell = CellLists[i][Data.Rnd.Next(0, CellLists[i].Count)];
                    checkCell = GetMapCell(anotherCell.Y, anotherCell.X);

                }
                else
                {
                    checkCell = FindEmptyCell((Cell.TypeOfCell)i);
                    if (checkCell != null)
                    {
                        checkCell.Content = (Cell.TypeOfCell)i;
                    }
                }

                byte tryCount = 0;
                while (CellLists[i].Count > 0 && CellLists[i].Count < CountTypeObjects[i]
                                              && CellLists[(int)Cell.TypeOfCell.Empty].Count > 0
                                              && tryCount < 5)
                {
                    checkCell = tryCount < 1 ? FindEmptyCell(checkCell) : FindEmptyCell((Cell.TypeOfCell)i);

                    if (checkCell != null)
                    {
                        checkCell.Content = (Cell.TypeOfCell)i;
                        tryCount = 0;
                    }
                    else
                    {
                        tryCount++;
                        Coordinates anotherCell = CellLists[i][Data.Rnd.Next(0, CellLists[i].Count)];
                        checkCell = GetMapCell(anotherCell.Y, anotherCell.X);
                    }
                }
            }
        }
    }

    public static bool CheckRadius(Cell cell, Cell.TypeOfCell cellType)
    {
        for (int i = 0; i < 8; i++)
        {
            Coordinates checkCoordinate = cell.Coordinate + Coordinates.CoordinateShift[i];
            if (GetMapCell(checkCoordinate.Y, checkCoordinate.X).Content == cellType)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Ищет свободную пустую клетку
    /// </summary>
    /// <returns></returns>
    public static Cell FindEmptyCell(Cell.TypeOfCell content)
    {
        int tryCount = 20;
        while (tryCount > 0)
        {
            Coordinates randomCoordinate =
                CellLists[(int)Cell.TypeOfCell.Empty][Data.Rnd.Next(0, CellLists[(int)Cell.TypeOfCell.Empty].Count)];
            if (Cell.IsFriendlyGround(GetMapCell(randomCoordinate.Y, randomCoordinate.X).Surface, content))
            {
                return GetMapCell(randomCoordinate.Y, randomCoordinate.X);
            }
            else
            {
                tryCount--;
            }
        }

        return null;
    }

    /// <summary>
    /// Ищет свободную клетку вокруг клетки, с указанными координатами
    /// </summary>
    /// <param name="cell"> Клетка, вокруг которой нужно искать </param>
    /// <returns></returns>
    public static Cell FindEmptyCell(Cell cell)
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
            var checkCoordinate = cell.Coordinate + Coordinates.CoordinateShift[randomShifts[shift]];
            if (checkCoordinate.Y > -1 && checkCoordinate.Y < Size.Y && checkCoordinate.X > -1
                && checkCoordinate.X < Size.X
                && GetMapCell(checkCoordinate.Y, checkCoordinate.X).Content == Cell.TypeOfCell.Empty
                && Cell.IsFriendlyGround(GetMapCell(checkCoordinate.Y, checkCoordinate.X).Surface, cell.Content))
            {
                return GetMapCell(checkCoordinate.Y, checkCoordinate.X);
            }

            randomShifts.Remove(randomShifts[shift]);
        }

        // Если не нашли ни одной пустой клетки, вернем null
        return null;
    }

    /// <summary>
    /// Ищет свободную клетку моря
    /// </summary>
    /// <returns></returns>
    public static Cell FindSeaCell()
    {
        Coordinates randomCoordinate =
            CellLists[(int)Cell.TypeOfCell.Sea][Data.Rnd.Next(0, CellLists[(int)Cell.TypeOfCell.Sea].Count)];
        return GetMapCell(randomCoordinate.Y, randomCoordinate.X);
    }

    /// <summary>
    /// Ищет свободную клетку моря вокруг клетки, с указанными координатами
    /// </summary>
    /// <param name="coordinate"> Координаты клетки, вокруг которой нужно искать </param>
    /// <returns></returns>
    public static Cell FindSeaCell(Coordinates coordinate)
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
                && checkCoordinate.X < Size.X
                && GetMapCell(checkCoordinate.Y, checkCoordinate.X).Surface == Cell.TypeOfCell.Sea
                && GetMapCell(checkCoordinate.Y, checkCoordinate.X).Content == Cell.TypeOfCell.Empty)
            {
                return GetMapCell(checkCoordinate.Y, checkCoordinate.X);
            }

            randomShifts.Remove(randomShifts[shift]);
        }

        // Если не нашли ни одной пустой клетки, вернем null
        return null;
    }

    /// <summary>
    /// Заполнение клеток карты каким-либо типом клетки, генерация стен
    /// </summary>
    public static void CreateMap()
    {
        InitializeCellLists();

        for (int y = 0; y < Size.Y; y++)
        {
            for (int x = 0; x < Size.X; x++)
            {
                if (x == 0 || x == Size.X - 1 || y == 0 || y == Size.Y - 1)
                {
                    WorldMap[(y * Size.X) + x] = new Cell(
                        new Coordinates(y, x),
                        Cell.TypeOfCell.Empty,
                        Cell.TypeOfCell.Wall);
                }
                else
                {
                    WorldMap[(y * Size.X) + x] = new Cell(
                        new Coordinates(y, x),
                        Cell.TypeOfCell.Sea,
                        Cell.TypeOfCell.Empty);
                }
            }
        }

        CreateGround();
        RandomRefreshMap();
    }

    /// <summary>
    /// Инициализия листа листов координат всех клеток карты
    /// </summary>
    public static void InitializeCellLists()
    {
        for (int i = 0; i < CountTypeObjects.Length; i++)
        {
            CellLists.Add(new List<Coordinates>());
        }
    }

    /// <summary>
    /// Обновляет клетку в листе клеток и просит пересовать её
    /// </summary>
    /// <param name="cell"> Клетка, которую обновляем </param>
    /// <param name="newType"> Новый тип этой клетки </param>
    public static void UpdateCellList(Cell cell, Cell.TypeOfCell newType)
    {
        if (cell.Surface != Cell.TypeOfCell.Empty)
        {
            if ((int)newType < (int)Cell.TypeOfCell.Sea || (int)newType > (int)Cell.TypeOfCell.Basalt)
            {
                CellLists[(int)cell.Content].Remove(cell.Coordinate);
            }
            else
            {
                CellLists[(int)cell.Surface].Remove(cell.Coordinate);
            }
        }

        if (cell.Surface != Cell.TypeOfCell.Empty || newType != Cell.TypeOfCell.Empty)
        {
            CellLists[(int)newType].Add(cell.Coordinate);
        }

        if ((int)newType < (int)Cell.TypeOfCell.Sea || (int)newType > (int)Cell.TypeOfCell.Basalt)
        {
            RenderingScript.UpdateTypeCell(cell);
        }
    }
}
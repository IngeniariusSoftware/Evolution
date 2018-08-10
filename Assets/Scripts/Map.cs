﻿public static class Map
{
    /// <summary>
    ///     Карта мира, состоящая из клеток
    /// </summary>
    public static Cell[,] WorldMap = new Cell[Data.MapSize.Y, Data.MapSize.X];

    /// <summary>
    ///     Поддержание уровня еды, яда, стен и минералов на карте
    /// </summary>
    public static void RefreshMap()
    {
        // Тут из глобала надо сюда перенести
        for (int i = 0; i < Data.MaxCountObjects.Length; i++)
        {
            while (Data.CurrentCountObjects[i] < Data.MaxCountObjects[i])
            {
                Coordinates randomPosition = Coordinates.RandomCoordinates(Data.MapSize.Y, Data.MapSize.X);
                if (Map.WorldMap[randomPosition.Y, randomPosition.X].CellType == CellEnum.TypeOfCell.Empty)
                {
                    Map.WorldMap[randomPosition.Y, randomPosition.X].CellType = CellEnum.GetCellType(i);
                }
            }
        }
    }

    /// <summary>
    ///     Заполнение клеток карты каким-либо типом клетки, генерация стен
    /// </summary>
    public static void CreateMap()
    {
        for (int x = 0; x < Data.MapSize.X; x++)
        {
            for (int y = 0; y < Data.MapSize.Y; y++)
            {
                if (x == 0 || x == Data.MapSize.X - 1 || y == 0 || y == Data.MapSize.Y - 1)
                {
                    WorldMap[y, x] = new Cell(new Coordinates(y, x), CellEnum.TypeOfCell.Wall);
                }
                else
                {
                    WorldMap[y, x] = new Cell(new Coordinates(y, x), CellEnum.TypeOfCell.Empty);
                }
            }
        }

        RefreshMap();
    }
}


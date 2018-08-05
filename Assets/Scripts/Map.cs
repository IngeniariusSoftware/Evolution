using System.Threading;

public static class Map
{
    public static Cell[,] WorldMap = new Cell[Data.MapSize.Y, Data.MapSize.X];

    //public IEnumerator GetEnumerator()
    //{
    //    if (WorldMap != null)
    //    {
    //        for (int x = 0; x < Data.MapSize.X; x++)
    //        {
    //            for (int y = 0; y < Data.MapSize.Y; y++)
    //            {
    //                yield return WorldMap[y, x];
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// Поддержание уровня еды, яда на карте
    /// </summary>
    public static void RefreshMap()
    {
        while (Data.CurrentCountFood < Data.MaxCountFood)
        {
            Coordinates randomPosition = Coordinates.RandomCoordinates(Data.MapSize.Y, Data.MapSize.X);
            if (Map.WorldMap[randomPosition.Y, randomPosition.X].CellType == CellEnum.TypeOfCell.Empty)
            {
                Map.WorldMap[randomPosition.Y, randomPosition.X].CellType = CellEnum.TypeOfCell.Food;
            }
        }

        while (Data.CurrentCountPoison < Data.MaxCountPoison)
        {
            Coordinates randomPosition = Coordinates.RandomCoordinates(Data.MapSize.Y, Data.MapSize.X);
            if (Map.WorldMap[randomPosition.Y, randomPosition.X].CellType == CellEnum.TypeOfCell.Empty)
            {
                Map.WorldMap[randomPosition.Y, randomPosition.X].CellType = CellEnum.TypeOfCell.Poison;
            }
        }
    }


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

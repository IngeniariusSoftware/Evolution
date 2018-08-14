public static class Map
{
    #region Constants

    /// <summary>
    ///     Размер карты по y и по x (количество клеток)   
    /// </summary>
    public static readonly Coordinates Size = new Coordinates(70, 120);

    /// <summary>
    ///     Процент от общего числа клеток различных объектов на карте
    /// </summary>
    public static readonly float[] PercentObjects = { 0, 0.06f, 0.06f, 0.08f, 0.03f, 0, 0.04f, 0 };

    /// <summary>
    ///     Общее количетсво клеток на карте
    /// </summary>
    private static readonly int AllCellCount = Size.X * Size.Y;

    /// <summary>
    ///     Максимально возможное количество различных объектов на карте
    /// </summary>
    public static readonly int[] MaxCountObjects =
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
        for (int i = 0; i < MaxCountObjects.Length; i++)
        {
            while (Data.CurrentCountObjects[i] < MaxCountObjects[i])
            {
                 FindEmptyCell().CellType = CellEnum.GetCellType(i);
            }
        }
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


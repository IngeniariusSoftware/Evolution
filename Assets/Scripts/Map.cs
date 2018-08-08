using System.Threading;

public static class Map
{
    /// <summary>
    /// ����� ����, ��������� �� ������
    /// </summary>
    public static Cell[,] WorldMap = new Cell[Data.MapSize.Y, Data.MapSize.X];

    /// <summary>
    /// ����������� ������ ���, ���, ���� � ��������� �� �����
    /// </summary>
    public static void RefreshMap()
    {
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
    /// ���������� ������ ����� �����-���� ����� ������, ��������� ����
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

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map
{
    public static Coordinates Size = new Coordinates(100, 100);

    public static Cell[,] WorldMap =  new Cell[Size.Y, Size.X];

    public IEnumerator GetEnumerator()
    {
        if (WorldMap != null)
        {
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    yield return WorldMap[y, x];
                }
            }
        }
    }
    

    public static void CreateMap()
    {
        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                WorldMap[y, x] = new Cell(new Coordinates(y, x), CellEnum.GetCellType(Random.Range(0,5)));
            }
        }
    }
}

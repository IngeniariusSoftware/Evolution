using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map
{
    public static int SizeY = 100;

    public static int SizeX = 100;

    public static Cell[,] WorldMap =  new Cell[SizeY, SizeX];

    public IEnumerator GetEnumerator()
    {
        if (WorldMap != null)
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    yield return WorldMap[y, x];
                }
            }
        }
    }
    

    public static void CreateMap()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                WorldMap[y, x] = new Cell(y, x, CellEnum.GetCellType(Random.Range(0,5)));
            }
        }
    }
}

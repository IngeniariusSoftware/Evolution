using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Map:IEnumerable
    {
        public static int SizeX = 100;

        public static int SizeY = 100;

        private static float CellSizeX = 2.56f;

        private static float CellSizeY = 2.56f;

        public Cell[,] map { get; set; }
        public IEnumerator GetEnumerator()
        {
            if (map != null)
            {
                for (int i = 0; i < SizeX; i++)
                {
                    for (int j = 0; j < SizeY; j++)
                    {
                        yield return map[i, j];
                    }
                }
            }
        }
    }
}

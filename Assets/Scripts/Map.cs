using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Map:IEnumerable
    {
        public int SizeX = 100;

        public int SizeY = 100;

        private float CellSizeX = 2.56f;

        private float CellSizeY = 2.56f;

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

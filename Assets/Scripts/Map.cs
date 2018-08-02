using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Map
    {
        public int SizeX = 100;

        public int SizeY = 100;

        private float CellSizeX = 2.56f;

        private float CellSizeY = 2.56f;

        public Cell[,] map { get; set; }
    }
}

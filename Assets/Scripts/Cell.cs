using System;
using Assets.Scripts;
using UnityEngine;

public class Cell
{
    public int Y { get; set; }

    public int X { get; set; }

    public Bug LinkedBug { get; set; }

    public CellEnum.TypeOfCell CellType;

    public Cell(int y, int x, CellEnum.TypeOfCell cellType, Bug bug = null)
    {
        Y = y;
        X = x;
        CellType = cellType;
        LinkedBug = bug;
        //if (cellType == CellEnum.TypeOfCell.Bug && bug == null)
        //{
        //    throw new Exception("Мы по ходу жука потеряли");
        //}
    }
}
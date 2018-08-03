using System;
using Assets.Scripts;
using UnityEngine;

public class Cell
{
    public Coordinates Coordinate { get; set; }

    public Bug LinkedBug { get; set; }

    public CellEnum.TypeOfCell CellType; // Вот тут запилить события, когда клетку изменяешь, что-нибудь навешать

    public Cell(Coordinates coordinate, CellEnum.TypeOfCell cellType, Bug bug = null)
    {
        Coordinate.Y = coordinate.Y;
        Coordinate.X = coordinate.X;
        CellType = cellType;
        LinkedBug = bug;
        //if (cellType == CellEnum.TypeOfCell.Bug && bug == null)
        //{
        //    throw new Exception("Мы по ходу жука потеряли");
        //}
    }
}
using System;
using Assets.Scripts;
using UnityEngine;

public class Cell
{
    public Coordinates Coordinate { get; set; }
    

    public Bug LinkedBug { get; set; }

    private CellEnum.TypeOfCell _cellType = CellEnum.TypeOfCell.Empty;

    public CellEnum.TypeOfCell CellType
    {
        get
        {
            return _cellType;
        }

        set
        {
            switch (_cellType)
            {
                case CellEnum.TypeOfCell.Food:
                    {
                        Data.CurrentCoutFood--;
                        break;
                    }
                case CellEnum.TypeOfCell.Poison:
                    {
                        Data.CurrentCoutPoison--;
                        break;
                    }
            }

            switch (value)
            {
                case CellEnum.TypeOfCell.Food:
                    {
                        Data.CurrentCoutFood++;
                        break;
                    }
                case CellEnum.TypeOfCell.Poison:
                    {
                        Data.CurrentCoutPoison++;
                        break;
                    }
            }

            _cellType = value;
        }
    }


    public Cell(Coordinates coordinate, CellEnum.TypeOfCell cellType, Bug bug = null)
    {
        Coordinate = coordinate;
        CellType = cellType;
        LinkedBug = bug;
        //if (cellType == CellEnum.TypeOfCell.Bug && bug == null)
        //{
        //    throw new Exception("Мы по ходу жука потеряли");
        //}
    }
}
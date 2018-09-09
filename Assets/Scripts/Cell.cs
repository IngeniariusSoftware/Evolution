using System;

[Serializable]
public class Cell
{
    #region Constants

    /// <summary>
    ///     Размер клетки по абсциссе для корректного отображения спрайта
    /// </summary>
    public static readonly float SizeX = 2.56f;

    /// <summary>
    ///     Размер клетки по ординате для корректного отображения спрайта
    /// </summary>
    public static readonly float SizeY = 2.56f;

    #endregion

    public Coordinates Coordinate { get; set; }

    public Bug LinkedBug { get; set; }

    private CellEnum.TypeOfCell _cellType;

    public CellEnum.TypeOfCell CellType
    {
        get
        {
            return _cellType;
        }

        set
        {
            if (_cellType == CellEnum.TypeOfCell.Empty)
            {
                if (value != CellEnum.TypeOfCell.Empty)
                {
                    Data.CountFillCell++;
                }
            }
            else
            {
                if (value == CellEnum.TypeOfCell.Empty)
                {
                    Data.CountFillCell--;
                }

                if (_cellType != CellEnum.TypeOfCell.Bug)
                {
                    Data.CurrentCountObjects[(int)_cellType]--;
                }
            }

            if (value != CellEnum.TypeOfCell.Empty)
            {
                if (value != CellEnum.TypeOfCell.Bug)
                {
                    Data.CurrentCountObjects[(int)value]++;
                }
            }

            Map.UpdateCellList(this, value);
            _cellType = value;
        }
    }

    public Cell()
    {
        Coordinate = null;
        LinkedBug = null;
        CellType = CellEnum.TypeOfCell.Empty;
    }

    public Cell(Coordinates coordinate, CellEnum.TypeOfCell cellType, Bug bug = null)
    {
        Coordinate = coordinate;
        LinkedBug = bug;
        CellType = cellType;
    }
}
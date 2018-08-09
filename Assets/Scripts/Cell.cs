public class Cell
{
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
            if (_cellType != CellEnum.TypeOfCell.Empty && _cellType != CellEnum.TypeOfCell.Bug)
            {
                Data.CurrentCountObjects[(int)_cellType]--;
            }

            if (value != CellEnum.TypeOfCell.Empty && value != CellEnum.TypeOfCell.Bug)
            {
                Data.CurrentCountObjects[(int)value]++;
            }

            _cellType = value;
            RenderingScript.UpdateTypeCell(this);
        }
    }

    public Cell(Coordinates coordinate, CellEnum.TypeOfCell cellType, Bug bug = null)
    {
        Coordinate = coordinate;
        LinkedBug = bug;
        CellType = cellType;
    }
}
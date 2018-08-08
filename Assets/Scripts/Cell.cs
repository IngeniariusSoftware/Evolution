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
            switch (_cellType)
            {
                case CellEnum.TypeOfCell.Berry:
                    {
                        Data.CurrentCountBerry--;
                        break;
                    }
                case CellEnum.TypeOfCell.Poison:
                    {
                        Data.CurrentCountPoison--;
                        break;
                    }
                case CellEnum.TypeOfCell.Wall:
                    {
                        Data.CurrentCountWall--;
                        break;
                    }
                case CellEnum.TypeOfCell.Mineral:
                    {
                        Data.CurrentCountMineral--;
                        break;
                    }
            }

            switch (value)
            {
                case CellEnum.TypeOfCell.Berry:
                    {
                        Data.CurrentCountBerry++;
                        break;
                    }
                case CellEnum.TypeOfCell.Poison:
                    {
                        Data.CurrentCountPoison++;
                        break;
                    }
                case CellEnum.TypeOfCell.Wall:
                    {
                        Data.CurrentCountWall++;
                        break;
                    }
                case CellEnum.TypeOfCell.Mineral:
                    {
                        Data.CurrentCountMineral++;
                        break;
                    }
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
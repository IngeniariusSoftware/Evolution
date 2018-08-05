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
                case CellEnum.TypeOfCell.Food:
                    {
                        Data.CurrentCountFood--;
                        break;
                    }
                case CellEnum.TypeOfCell.Poison:
                    {
                        Data.CurrentCountPoison--;
                        break;
                    }
            }

            switch (value)
            {
                case CellEnum.TypeOfCell.Food:
                    {
                        Data.CurrentCountFood++;
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
            }

            _cellType = value;
            RenderingScript.UpdateTypeCell(this);
        }
    }


    public Cell(Coordinates coordinate, CellEnum.TypeOfCell cellType, Bug bug = null)
    {
        Coordinate = coordinate;
        LinkedBug = bug;
        //if (cellType == CellEnum.TypeOfCell.Bug && bug == null)
        //{
        //    throw new Exception("Мы по ходу жука потеряли");
        //}
        CellType = cellType;
    }
}
public class CellEnum
{
    public enum TypeOfCell
    {
        Empty,

        Berry,

        Poison,

        Wall,

        Mineral,

        MineralBerry,

        Bug
    }

    public static TypeOfCell GetCellType(int number)
    {
        switch (number)
        {
            case 0:
                {
                    return TypeOfCell.Empty;
                }

            case 1:
                {
                    return TypeOfCell.Berry;
                }

            case 2:
                {
                    return TypeOfCell.Poison;
                }

            case 3:
                {
                    return TypeOfCell.Wall;
                }

            case 4:
                {
                    return TypeOfCell.Mineral;
                }

            case 5:
                {
                    return TypeOfCell.MineralBerry;
                }

            case 6:
                {
                    return TypeOfCell.Bug;
                }
        }

        return TypeOfCell.Empty;
    }
}

public class CellEnum
{
    public enum TypeOfCell
    {
        Empty = 0,

        Food,

        Poison,

        Wall,

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
                    return TypeOfCell.Food;
                }
            case 2:
                {
                    return TypeOfCell.Poison;
                }
            case 3:
                {
                    return TypeOfCell.Bug;
                }
            case 4:
                {
                    return TypeOfCell.Wall;
                }
        }

        return TypeOfCell.Empty;
    }

}

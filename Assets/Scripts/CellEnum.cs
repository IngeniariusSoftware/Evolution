public class CellEnum
{
    public enum TypeOfCell
    {
        Empty,

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
                    return TypeOfCell.Wall;
                }
            case 4:
                {
                    return TypeOfCell.Bug;
                }
        }

        return TypeOfCell.Empty;
    }
}

public class CellEnum
{
    public enum TypeOfCell
    {
        Empty = 0, 

        Berry = 1,

        Poison,

        Wall,

        Mineral,

        MineralBerry,

        Bamboo,

        Bug
    }

    public static TypeOfCell GetCellType(int number)
    {
        return (TypeOfCell)number;
    }
}

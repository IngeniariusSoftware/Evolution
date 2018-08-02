using Assets.Scripts;

public class Cell
{
    public Bug bug { get; set; }

    public int PosX { get; set; }
    
    public int PosY { get; set; }

    public CellEnum TypeOfEntity { get; set; }
}
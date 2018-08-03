public class Coordinates
{
    public int Y { get; set; }

    public int X { get; set; }

    public static Coordinates[] CoordinateShift = new Coordinates[]
                                                      {
                                                          new Coordinates(-1, 0), new Coordinates(-1, 1),
                                                          new Coordinates(0, 1), new Coordinates(1, 1),
                                                          new Coordinates(1, 0), new Coordinates(1, -1),
                                                          new Coordinates(0, -1), new Coordinates(-1, -1)
                                                      };

    public Coordinates(int y, int x)
    {
        Y = y;
        X = x;
    }
}

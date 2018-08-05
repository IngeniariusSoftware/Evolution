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

    public static Coordinates RandomCoordinates(int rangeY, int rangeX)
    {
        return new Coordinates(Data.Rnd.Next(0, rangeY), Data.Rnd.Next(0, rangeX));
    }

    public static Coordinates operator +(Coordinates coordinate1, Coordinates coordinate2)
    {
        return new Coordinates(coordinate1.Y + coordinate2.Y, coordinate1.X + coordinate2.X);
    }
}

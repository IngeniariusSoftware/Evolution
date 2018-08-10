public class Bug
{
    // Зачем здесь логика на спавн???
    public Bug(Genome genome = null, Coordinates currentPosition = null)
    {
        if (currentPosition == null)
            do
            {
                currentPosition = Coordinates.RandomCoordinates(Data.MapSize.Y, Data.MapSize.X);
            } while (Map.WorldMap[currentPosition.Y, currentPosition.X].CellType != CellEnum.TypeOfCell.Empty);

        CurrentPosition = currentPosition;
        Map.WorldMap[currentPosition.Y, currentPosition.X].CellType = CellEnum.TypeOfCell.Bug;
        if (genome == null)
            Gene = new Genome();
        else
            Gene = genome;

        Health = Data.StartBugHealth;
        LifeTime = 0;
        CurrentGenePosition = 0;
        Direction = Data.Rnd.Next(0, 8);
        Map.WorldMap[currentPosition.Y, currentPosition.X].LinkedBug = this;
    }

    #region BugAttributes

    public int Direction { get; set; }

    public int LifeTime { get; set; }

    private int _health;

    public int Health
    {
        get { return _health; }

        set
        {
            if (value > -1 && value <= Data.MaxBugHealth)
                _health = value;
            else
                _health = 0;
        }
    }

    public Genome Gene { get; set; }

    #endregion

    #region Genome

    private int _currentGenePosition;

    public int CurrentGenePosition
    {
        get { return _currentGenePosition; }

        set { _currentGenePosition = value % Data.LengthGenome; }
    }

    private static bool GenomJump(Bug bug)
    {
        bug.CurrentGenePosition = bug.NextGenePosition(bug.Gene.genome[bug.CurrentGenePosition]);
        return false;
    }

    public int NextGenePosition(int shift = 1)
    {
        return (CurrentGenePosition + shift) % Data.LengthGenome;
    }

    #endregion

    #region BugPosition

    public Coordinates LastPosition { get; set; }

    public Coordinates CurrentPosition { get; set; }

    private static Cell DestinationCell;

    #endregion

    public void StartAction()
    {
        LastPosition = CurrentPosition;
        var countSteps = 0;
        var isEnd = false;
        Health--;
        LifeTime++;
        while (countSteps < Data.MaxStepsBug && !isEnd)
        {
            isEnd = DoCommand(this);
            countSteps++;
        }

        //Умирает если зациклился 
        if (countSteps == Data.MaxStepsBug && !isEnd) Health = 0;
    }

    public static bool DoCommand(Bug bug)
    {
        var destination =
            Coordinates.CoordinateShift[CalculateShift(bug)] + bug.CurrentPosition;
        DestinationCell = Map.WorldMap[destination.Y, destination.X];

        // Если комманда действие, то выполнить
        if (MasBugCommands.Length > bug.Gene.genome[bug.CurrentGenePosition])
            return MasBugCommands[bug.Gene.genome[bug.CurrentGenePosition]].Invoke(bug);
        // Иначе сделать переход по геному
        return GenomJump(bug);
    }

    public delegate bool BugCommand(Bug bug);

    //TODO Команды  Move, Rotate, CheckCell, Take, Multiply, Push, CheckHealth, Attack, Share, Photosynthesize , IsFriend Вспомогательная команда, поэтому не вносится
    public static BugCommand[] MasBugCommands = {Move, Rotate, CheckCell, Take, Multiply, Push, CheckHealth, Attack, Share};

    private static int CalculateShift(Bug bug)
    {
        var res = 0;
        // С учетом текущего поворота
        res = (bug.Direction + bug.Gene.genome[bug.NextGenePosition()]) % 8;
        return res;
    }

    #region BugCommands

    private static bool Move(Bug bug)
    {
        bug.CurrentGenePosition += (int) DestinationCell.CellType + 1;
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug)) bug.CurrentGenePosition++;

        switch (DestinationCell.CellType)
        {
            case CellEnum.TypeOfCell.Empty:
            {
                Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
                Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = CellEnum.TypeOfCell.Empty;
                bug.CurrentPosition = DestinationCell.Coordinate;
                DestinationCell.CellType = CellEnum.TypeOfCell.Bug;
                DestinationCell.LinkedBug = bug;
                break;
            }

            case CellEnum.TypeOfCell.Berry:
            {
                Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
                Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = CellEnum.TypeOfCell.Empty;
                bug.CurrentPosition = DestinationCell.Coordinate;
                DestinationCell.CellType = CellEnum.TypeOfCell.Bug;
                DestinationCell.LinkedBug = bug;
                bug.Health += Data.BerryValue;
                break;
            }

            case CellEnum.TypeOfCell.MineralBerry:
            {
                Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].LinkedBug = null;
                Map.WorldMap[bug.CurrentPosition.Y, bug.CurrentPosition.X].CellType = CellEnum.TypeOfCell.Empty;
                bug.CurrentPosition = DestinationCell.Coordinate;
                DestinationCell.CellType = CellEnum.TypeOfCell.Bug;
                DestinationCell.LinkedBug = bug;
                bug.Health += Data.MineralBerryValue;
                break;
            }

            case CellEnum.TypeOfCell.Poison:
            {
                bug.Health = 0;
                break;
            }
        }

        return true;
    }

    private static bool CheckCell(Bug bug)
    {
        bug.CurrentGenePosition += (int) DestinationCell.CellType + 1;
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug)) bug.CurrentGenePosition++;

        return false;
    }

    private static bool Take(Bug bug)
    {
        bug.CurrentGenePosition += (int) DestinationCell.CellType + 1;
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug)) bug.CurrentGenePosition++;

        switch (DestinationCell.CellType)
        {
            case CellEnum.TypeOfCell.Berry:
            {
                DestinationCell.CellType = CellEnum.TypeOfCell.Empty;
                bug.Health += Data.BerryValue;
                break;
            }

            case CellEnum.TypeOfCell.MineralBerry:
            {
                DestinationCell.CellType = CellEnum.TypeOfCell.Empty;
                bug.Health += Data.MineralBerryValue;
                break;
            }

            case CellEnum.TypeOfCell.Poison:
            {
                DestinationCell.CellType = CellEnum.TypeOfCell.Berry;
                break;
            }
        }

        return true;
    }

    private bool IsFriendBug(Bug bug)
    {
        var isDifference = false;
        for (var i = 0; i < Gene.genome.Length; i++)
            if (Gene.genome[i] != bug.Gene.genome[i])
            {
                if (isDifference)
                    return false;
                isDifference = true;
            }

        return true;
    }

    private static bool Rotate(Bug bug)
    {
        bug.Direction = bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)] % 8;
        bug.CurrentGenePosition += 2;
        return false;
    }

    private static bool Multiply(Bug bug)
    {
        bug.Health -= Data.MuptiplyCost;
        var isBorn = false;
        for (var i = 0; i < 8 && !isBorn; i++)
        {
            var birthCoordinate = bug.CurrentPosition + Coordinates.CoordinateShift[i];
            if (Map.WorldMap[birthCoordinate.Y, birthCoordinate.X].CellType == CellEnum.TypeOfCell.Empty)
            {
                var childBug = new Bug(
                    new Genome(bug.Gene.GenomeMutate(Data.Rnd.Next(0, 2))),
                    birthCoordinate);
                ControlScript.childs.Add(childBug);
                isBorn = true;
                childBug.Health = bug.Health / 2;
            }
        }

        bug.CurrentGenePosition++;
        bug.Health = bug.Health / 2;
        return true;
    }

    private static bool Push(Bug bug)
    {
        bug.CurrentGenePosition += (int) DestinationCell.CellType + 1;
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug)) bug.CurrentGenePosition++;

        if (DestinationCell.CellType != CellEnum.TypeOfCell.Wall)
        {
            if (DestinationCell.CellType == CellEnum.TypeOfCell.Bamboo)
            {
                if (Data.Rnd.Next(0, 3) == 0)
                    DestinationCell.CellType = CellEnum.TypeOfCell.Poison;
                else
                    DestinationCell.CellType = CellEnum.TypeOfCell.Berry;
            }
            else
            {
                var pushDestination =
                    Coordinates.CoordinateShift[(bug.Direction + bug.Gene.genome[bug.NextGenePosition(1)]) % 8]
                    + DestinationCell.Coordinate;
                var pushDestinationCell = Map.WorldMap[pushDestination.Y, pushDestination.X];
                if (pushDestinationCell.CellType == CellEnum.TypeOfCell.Empty)
                {
                    pushDestinationCell.CellType = DestinationCell.CellType;
                    if (pushDestinationCell.CellType == CellEnum.TypeOfCell.Bug)
                    {
                        pushDestinationCell.LinkedBug = DestinationCell.LinkedBug;
                        pushDestinationCell.LinkedBug.CurrentPosition = pushDestination;
                        DestinationCell.LinkedBug = null;
                    }

                    DestinationCell.CellType = CellEnum.TypeOfCell.Empty;
                }
            }
        }

        return true;
    }

    private static bool CheckHealth(Bug bug)
    {
        if (bug.Health > bug.Gene.genome[bug.NextGenePosition(1)] * Data.MaxBugHealth / Data.LengthGenome)
            bug.CurrentGenePosition += 2;
        else
            bug.CurrentGenePosition += 3;

        return false;
    }

    private static bool Attack(Bug bug)
    {
        bug.CurrentGenePosition += (int) DestinationCell.CellType + 1;
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug)) bug.CurrentGenePosition++;

        switch (DestinationCell.CellType)
        {
            case CellEnum.TypeOfCell.Mineral:
            {
                DestinationCell.CellType = CellEnum.TypeOfCell.MineralBerry;
                break;
            }

            case CellEnum.TypeOfCell.Bug:
            {
                if (DestinationCell.LinkedBug != null)
                {
                    DestinationCell.LinkedBug.Health -= 5;
                    bug.Health += 5;
                }

                break;
            }

            case CellEnum.TypeOfCell.Wall:
            {
                break;
            }

            default:
            {
                DestinationCell.CellType = CellEnum.TypeOfCell.Empty;
                break;
            }
        }

        return true;
    }

    private static bool Share(Bug bug)
    {
        bug.CurrentGenePosition += (int) DestinationCell.CellType + 1;
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.CurrentGenePosition++;
            neighbourBug.Health += 5;
            bug.Health -= 5;
        }

        return false;
    }

    private static bool Photosynthesize(Bug bug)
    {
        bug.CurrentGenePosition += (int) DestinationCell.CellType + 1;
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug)) bug.CurrentGenePosition++;

        if (DestinationCell.CellType == CellEnum.TypeOfCell.Mineral) bug.Health += 5;

        return true;
    }

    #endregion
}
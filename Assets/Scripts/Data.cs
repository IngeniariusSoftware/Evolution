using Assets.Scripts;


/// <summary>
/// Главный класс из которого берутся данные
/// </summary>
public static class Data
{
    #region Переменные

    /// <summary>
    ///     Количество команд в геноме
    /// </summary>
    public static readonly int LengthGenome = 64;

    /// <summary>
    ///     Максимальное количество жизней жука
    /// </summary>
    public static readonly int MaxBugHealth = 128;

    /// <summary>
    ///     Количество жизней, которое есть у жука при создании мира
    /// </summary>
    public static readonly int StartBugHealth = 50;

    /// <summary>
    ///     Количество жизней, которое получаает жук, если съест еду
    /// </summary>
    public static readonly int FoodValue = 10;

    /// <summary>
    ///     Количество клеток по x и y   
    /// </summary>
    public static readonly Coordinates MapSize = new Coordinates(50, 50);

    /// <summary>
    ///     Процент еды на карте
    /// </summary>
    public static readonly float PercentFood = 0.05f;

    /// <summary>
    ///      Процент яда на карте
    /// </summary>
    public static readonly float PercentPoison = 0.05f;

    /// <summary>
    ///     Текущее количество еды на карте
    /// </summary>
    public static int CurrentCoutFood = 0;

    /// <summary>
    ///    Текущее количество яда на карте
    /// </summary>
    public static int CurrentCoutPoison = 0;

    /// <summary>
    ///     Максимально возможное количество еды на карте
    /// </summary>
    public static readonly int MaxCountFood = (int)(MapSize.X * MapSize.Y * PercentFood);
    
    /// <summary>
    ///     Максимально возможное количество яда на карте
    /// </summary>
    public static readonly int MaxCountPoison = (int)(MapSize.X * MapSize.Y * PercentPoison);

    /// <summary>
    ///     Размер клетки по абсциссе для корректного отображения спрайта
    /// </summary>
    public static readonly float CellSizeX = 2.56f;

    /// <summary>
    ///     Размер клетки по ординате для корректного отображения спрайта
    /// </summary>
    public static readonly float CellSizeY = 2.56f;

    #endregion

    public static void StartBugSelection()
    {
        BugsCollection.StartExecution();
    }

    public static BugCollection BugsCollection;
}
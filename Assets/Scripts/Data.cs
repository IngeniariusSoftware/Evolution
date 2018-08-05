using System;

/// <summary>
/// Главный класс из которого берутся данные
/// </summary>
public static class Data
{
    #region Переменные

    /// <summary>
    ///     Переменная для генерации случайный значений
    /// </summary>
    public static readonly Random Rnd = new Random();

    /// <summary>
    ///     Начальное количество жуков
    /// </summary>
    public static readonly int BugCount = 100;

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
    public static readonly Coordinates MapSize = new Coordinates(35, 60);

    /// <summary>
    ///     Процент еды на карте
    /// </summary>
    public static readonly float PercentFood = 0.1f;

    /// <summary>
    ///      Процент яда на карте
    /// </summary>
    public static readonly float PercentPoison = 0.1f;

    /// <summary>
    ///      Процент яда на карте
    /// </summary>
    public static readonly float PercentWall = 0.05f;

    /// <summary>
    ///     Текущее количество еды на карте
    /// </summary>
    public static int CurrentCountFood = 0;

    /// <summary>
    ///    Текущее количество яда на карте
    /// </summary>
    public static int CurrentCountPoison = 0;

    /// <summary>
    ///    Текущее количество яда на карте
    /// </summary>
    public static int CurrentCountWall = 0;

    /// <summary>
    ///     Максимально возможное количество еды на карте
    /// </summary>
    public static readonly int MaxCountFood = (int)(MapSize.X * MapSize.Y * PercentFood);

    /// <summary>
    ///     Максимально возможное количество яда на карте
    /// </summary>
    public static readonly int MaxCountPoison = (int)(MapSize.X * MapSize.Y * PercentPoison);

    /// <summary>
    ///     Максимально возможное количество яда на карте
    /// </summary>
    public static readonly int MaxCountWall = (int)(MapSize.X * MapSize.Y * PercentWall);

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
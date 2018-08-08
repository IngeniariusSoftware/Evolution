using System;

/// <summary>
/// Главный класс из которого берутся данные
/// </summary>
public static class Data
{
    #region Переменные

    /// <summary>
    ///     Начальное количество жуков
    /// </summary>
    public static int CurrentStepsRendering = 0;

    /// <summary>
    ///     Начальное количество жуков
    /// </summary>
    public static readonly int MaxStepsRendering = 24;

    /// <summary>
    ///     Начальное количество жуков
    /// </summary>
    public static readonly int MaxStepsBug = 25;

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
    public static readonly int MaxBugHealth = 256;

    /// <summary>
    ///     Количество жизней, которое есть у жука при создании мира
    /// </summary>
    public static readonly int StartBugHealth = 50;

    /// <summary>
    ///     Количество жизней, необходимое на размножение жука
    /// </summary>
    public static readonly int MuptiplyCost = 30;

    /// <summary>
    ///     Количество жизней, которое получаает жук, если съест минеральную ягоду
    /// </summary>
    public static readonly int MineralBerryValue = 30;

    /// <summary>
    ///     Количество жизней, которое получаает жук, если съест обычную ягоду
    /// </summary>
    public static readonly int BerryValue = 10;

    /// <summary>
    ///     Количество клеток по x и y   
    /// </summary>
    public static readonly Coordinates MapSize = new Coordinates(70, 120);

    /// <summary>
    ///     Процент обычных ягод на карте
    /// </summary>
    public static readonly float PercentBerry = 0.06f;

    /// <summary>
    ///      Процент яда на карте
    /// </summary>
    public static readonly float PercentPoison = 0.06f;

    /// <summary>
    ///      Процент минералов на карте
    /// </summary>
    public static readonly float PercentMineral = 0.04f;

    /// <summary>
    ///      Процент яда на карте
    /// </summary>
    public static readonly float PercentWall = 0.08f;

    /// <summary>
    ///     Максимально возможное количество минералов на карте
    /// </summary>
    public static readonly int MaxCountMineral = (int)(MapSize.X * MapSize.Y * PercentMineral);

    /// <summary>
    ///     Максимально возможное количество обычных ягод на карте
    /// </summary>
    public static readonly int MaxCountBerry = (int)(MapSize.X * MapSize.Y * PercentBerry);

    /// <summary>
    ///     Максимально возможное количество яда на карте
    /// </summary>
    public static readonly int MaxCountPoison = (int)(MapSize.X * MapSize.Y * PercentPoison);

    /// <summary>
    ///     Максимально возможное количество стен на карте
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

    /// <summary>
    ///     Текущее количество минералов на карте
    /// </summary>
    public static int CurrentCountMineral = 0;

    /// <summary>
    ///     Текущее количество обычных ягод на карте
    /// </summary>
    public static int CurrentCountBerry = 0;

    /// <summary>
    ///    Текущее количество яда на карте
    /// </summary>
    public static int CurrentCountPoison = 0;

    /// <summary>
    ///    Текущее количество стен на карте
    /// </summary>
    public static int CurrentCountWall = 0;

    #endregion

    public static BugCollection BugsCollection;
}
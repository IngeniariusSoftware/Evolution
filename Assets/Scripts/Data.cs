using System;

/// <summary>
/// Класс, предназначенный для хранения основных данных
/// </summary>
public static class Data
{
    #region Переменные
    
    /// <summary>
    ///     Максимальное количество команд, которое может выполнить жук за один ход
    /// </summary>
    public static readonly int MaxStepsBug = 25;

    /// <summary>
    ///     Переменная для генерации случайный значений
    /// </summary>
    public static readonly Random Rnd = new Random();

    /// <summary>
    ///     Начальное количество жуков на карте
    /// </summary>
    public static readonly int BugCount = 100;

    /// <summary>
    ///     Количество ячеек в геноме жука
    /// </summary>
    public static readonly int LengthGenome = 64;

    /// <summary>
    ///     Максимальное количество жизней у жука
    /// </summary>
    public static readonly int MaxBugHealth = 256;

    /// <summary>
    ///     Количество жизней, с которым появляется жук
    /// </summary>
    public static readonly int StartBugHealth = 50;

    /// <summary>
    ///     Количество жизней, на которое уменьшается жизнь жука-родителя при генерации нового жука
    /// </summary>
    public static readonly int MuptiplyCost = 30;

    /// <summary>
    ///     Количество жизней, которое получаает жук, съедая минеральную ягоду
    /// </summary>
    public static readonly int MineralBerryValue = 30;

    /// <summary>
    ///     Количество жизней, которое получаает жук, съедая обычную ягоду
    /// </summary>
    public static readonly int BerryValue = 10;

    /// <summary>
    ///     Размер карты по y и по x (количество клеток)   
    /// </summary>
    public static readonly Coordinates MapSize = new Coordinates(70, 120);

    /// <summary>
    ///     Процент от общего числа клеток различных объектов на карте
    /// </summary>
    public static readonly float[] PercentObjects = {0, 0.06f, 0.06f, 0.08f, 0.03f, 0, 0};

    /// <summary>
    ///     Общее количетсво клеток на карте
    /// </summary>
    private static readonly int AllCellCount = MapSize.X * MapSize.Y;

    /// <summary>
    ///     Максимально возможное количество различных объектов на карте
    /// </summary>
    public static readonly int[] MaxCountObjects =
        {
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Empty]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Berry]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Poison]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Wall]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Mineral]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.MineralBerry]),
            (int)(AllCellCount * PercentObjects[(int)CellEnum.TypeOfCell.Bug])
        };

    /// <summary>
    ///     Текущее количество различных объектов на карте
    /// </summary>
    public static readonly int[] CurrentCountObjects = { 0, 0, 0, 0, 0, 0, 0 };

    /// <summary>
    ///     Размер клетки по абсциссе для корректного отображения спрайта
    /// </summary>
    public static readonly float CellSizeX = 2.56f;

    /// <summary>
    ///     Размер клетки по ординате для корректного отображения спрайта
    /// </summary>
    public static readonly float CellSizeY = 2.56f;

    /// <summary>
    ///     Текущий шаг отрисовки
    /// </summary>
    public static int CurrentStepsRendering = 0;

    /// <summary>
    ///     Максимальный шаг отрисовки (Влияет на плавность отрисовки хода)
    /// </summary>
    public static int MaxStepsRendering = 24;

    #endregion
}
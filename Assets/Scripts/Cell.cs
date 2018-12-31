using System;

/// <summary>
/// Класс клетки на карте
/// </summary>
[Serializable]
public class Cell
{
    /// <summary>
    /// Типы всех клеток
    /// </summary>
    public enum TypeOfCell
    {
        /// <summary>
        /// Пустая клетка, основа карты
        /// </summary>
        Empty = 0,

        /// <summary>
        /// Обычная ягода, основная еда
        /// </summary>
        Berry = 1,

        /// <summary>
        /// Яд, если жук съедает, то умирает, но может превратить в еду
        /// </summary>
        Poison,

        /// <summary>
        /// Непроходимое препятствие
        /// </summary>
        Wall,


        /// <summary>
        /// Кристалл из которого выпадает минеральная года при разрушении
        /// </summary>
        Mineral,

        /// <summary>
        /// Кристальная ягода, очень питательная, но добывается только из кристаллов
        /// </summary>
        MineralBerry,

        /// <summary>
        /// Заросли бамбука, которые можно разрушить и оттуда выпадет либо яд, либо еда
        /// </summary>
        Bamboo,

        /// <summary>
        /// Солнце, источник большого количества энергии
        /// </summary>
        Sun,

        /// <summary>
        /// Водоросли, съедобная сущность, появляющаяся в море
        /// </summary>
        Seaweed,

        /// <summary>
        /// Колючка, препятствие, которое лучше не трогать
        /// </summary>
        Prickle,

        /// <summary>
        /// Клетка с морем, можно плавать
        /// </summary>
        Sea,

        /// <summary>
        /// Пустынная поверность
        /// </summary>
        Desert,

        /// <summary>
        /// Грунтовая поверхность
        /// </summary>
        Ground,

        /// <summary>
        /// Поверхность, покрытая травой
        /// </summary>
        Grass,

        /// <summary>
        /// Поверхность, заросшая джунглями
        /// </summary>
        Jungle,

        /// <summary>
        /// Базальтовая поверхность
        /// </summary>
        Basalt,

        /// <summary>
        /// Жук, пытается выжить в этом мире
        /// </summary>
        Bug
    }

    #region Constants

    /// <summary>
    /// Размер клетки по абсциссе для корректного отображения спрайта
    /// </summary>
    public const float SizeX = 2.56f;

    /// <summary>
    /// Размер клетки по ординате для корректного отображения спрайта
    /// </summary>
    public const float SizeY = 2.56f;

    #endregion

    #region Fields

    /// <summary>
    /// Содержимое клетки
    /// </summary>
    private TypeOfCell content;

    /// <summary>
    /// Поверхность клетки
    /// </summary>
    private TypeOfCell surface;

    #endregion

    #region Constructors

    /// <summary>
    /// Создание пустой клетки
    /// </summary>
    public Cell()
    {
        Coordinate = null;
        LinkedBug = null;
        Content = TypeOfCell.Empty;
        Surface = TypeOfCell.Sea;
    }

    /// <summary>
    /// Проверка совместимости сущности и почвы
    /// </summary>
    /// <returns></returns>
    public static bool IsFriendlyGround(TypeOfCell surface, TypeOfCell content)
    {
        switch (content)
        {
            case TypeOfCell.Mineral:
                {
                    return surface == TypeOfCell.Basalt;
                }

            case TypeOfCell.Berry:
                {
                    return surface == TypeOfCell.Grass || surface == TypeOfCell.Ground;
                }

            case TypeOfCell.Poison:
                {
                    return surface == TypeOfCell.Desert || surface == TypeOfCell.Ground;
                }

            case TypeOfCell.Sun:
                {
                    return surface == TypeOfCell.Grass;
                }

            case TypeOfCell.Bamboo:
                {
                    return surface == TypeOfCell.Jungle;
                }

            case TypeOfCell.Wall:
                {
                    return surface == TypeOfCell.Jungle;
                }

            case TypeOfCell.Prickle:
                {
                    return surface == TypeOfCell.Desert;
                }

            case TypeOfCell.Seaweed:
                {
                    return surface == TypeOfCell.Sea;
                }

            case TypeOfCell.Bug:
                {
                    return surface != TypeOfCell.Sea;
                }

            default:
                {
                    return true;
                }
        }
    }

    /// <summary>
    /// Создание клетки с указанными параметрами
    /// </summary>
    /// <param name="coordinate"> Координаты клетки на карте </param>
    /// <param name="content"> Тип клетки </param>
    /// <param name="bug"> Жук, находящийся в клетке, если есть </param>
    public Cell(Coordinates coordinate, TypeOfCell surface, TypeOfCell content, Bug bug = null)
    {
        Coordinate = coordinate;
        LinkedBug = bug;
        Surface = surface;
        Content = content;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Координаты клетки на карте
    /// </summary>
    public Coordinates Coordinate { get; set; }

    /// <summary>
    /// Ссылка на жука, который находится в клетке
    /// </summary>
    public Bug LinkedBug { get; set; }

    /// <summary>
    /// Содержимое клеток
    /// </summary>
    public TypeOfCell Content
    {
        get => content;

        set
        {
            Map.UpdateCellList(this, value);
            content = value;
        }
    }

    /// <summary>
    /// Поврехность клеток
    /// </summary>
    public TypeOfCell Surface
    {
        get => surface;


        set
        {
            Map.UpdateCellList(this, value);
            surface = value;
        }
    }

    #endregion
}
using System;

/// <summary>
/// Класс клетки на карте
/// </summary>
[Serializable]
public class Cell
{
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
        Poison = 2,

        /// <summary>
        /// Непроходимое препятствие
        /// </summary>
        Wall = 3,


        /// <summary>
        /// Кристалл из которого выпадает минеральная года при разрушении
        /// </summary>
        Mineral = 4,

        /// <summary>
        /// Кристальная ягода, очень питательная, но добывается только из кристаллов
        /// </summary>
        MineralBerry = 5,


        /// <summary>
        /// Заросли бамбука, которые можно разрушить и оттуда выпадет либо яд, либо еда
        /// </summary>
        Bamboo = 6,

        /// <summary>
        /// Солнце, источник большого количества энергии
        /// </summary>
        Sun = 7,

        /// <summary>
        /// Колючка, препятствие, которое лучше не трогать
        /// </summary>
        Pricle = 8,

        /// <summary>
        /// Клетка с морем, можно плавать
        /// </summary>
        //Sea = 9,

        /// <summary>
        /// Пустынная поверность
        /// </summary>
        //Desert = 10,

        /// <summary>
        /// Грунтовая поверхность
        /// </summary>
        //Ground = 11,

        /// <summary>
        /// Поверхность, покрытая травой
        /// </summary>
        //Grass = 12,

        /// <summary>
        /// Поверхность, заросшая джунглями
        /// </summary>
        //Jungle = 13,

        /// <summary>
        /// Базальтовая поверхность
        /// </summary>
        //Basalt = 14,

        /// <summary>
        /// Жук, пытается выжить в этом мире
        /// </summary>
        Bug = 9
    }

    public static TypeOfCell GetCellType(int number)
    {
        return (TypeOfCell)number;
    }

    #region Constants

    /// <summary>
    ///     Размер клетки по абсциссе для корректного отображения спрайта
    /// </summary>
    public const float SizeX = 2.56f;

    /// <summary>
    /// Размер клетки по ординате для корректного отображения спрайта
    /// </summary>
    public const float SizeY = 2.56f;

    #endregion

    #region Fields

    /// <summary>
    /// Тип клетки
    /// </summary>
    private TypeOfCell cellType;

    #endregion

    #region Constructors

    /// <summary>
    /// Создание пустой клетки
    /// </summary>
    public Cell()
    {
        Coordinate = null;
        LinkedBug = null;
        CellType = TypeOfCell.Empty;
    }

    /// <summary>
    /// Создание клетки с указанными параметрами
    /// </summary>
    /// <param name="coordinate"> Координаты клетки на карте </param>
    /// <param name="cellType"> Тип клетки </param>
    /// <param name="bug"> Жук, находящийся в клетке, если есть </param>
    public Cell(Coordinates coordinate, TypeOfCell cellType, Bug bug = null)
    {
        Coordinate = coordinate;
        LinkedBug = bug;
        CellType = cellType;
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
    /// Учет всех клеток на карте во время измения типа клетки
    /// </summary>
    public TypeOfCell CellType
    {
        get
        {
            return cellType;
        }

        set
        {
            if (cellType == TypeOfCell.Empty)
            {
                if (value != TypeOfCell.Empty)
                {
                    Data.CountFillCell++;
                }
            }
            else
            {
                if (value == TypeOfCell.Empty)
                {
                    Data.CountFillCell--;
                }

                if (cellType != TypeOfCell.Bug)
                {
                    Data.CurrentCountObjects[(int)cellType]--;
                }
            }

            if (value != TypeOfCell.Empty)
            {
                if (value != TypeOfCell.Bug)
                {
                    Data.CurrentCountObjects[(int)value]++;
                }
            }

            Map.UpdateCellList(this, value);
            cellType = value;
        }
    }

    #endregion
}
using System;
using System.Diagnostics.CodeAnalysis;

[Serializable]
public class Cell
{
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
    private CellEnum.TypeOfCell cellType;

    #endregion

    #region Constructors

    /// <summary>
    /// Создание пустой клетки
    /// </summary>
    public Cell()
    {
        Coordinate = null;
        LinkedBug = null;
        CellType = CellEnum.TypeOfCell.Empty;
    }

    /// <summary>
    /// Создание клетки с указанными параметрами
    /// </summary>
    /// <param name="coordinate"> Координаты клетки на карте </param>
    /// <param name="cellType"> Тип клетки </param>
    /// <param name="bug"> Жук, находящийся в клетке, если есть </param>
    public Cell(Coordinates coordinate, CellEnum.TypeOfCell cellType, Bug bug = null)
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
    public CellEnum.TypeOfCell CellType
    {
        get
        {
            return cellType;
        }

        set
        {
            if (cellType == CellEnum.TypeOfCell.Empty)
            {
                if (value != CellEnum.TypeOfCell.Empty)
                {
                    Data.CountFillCell++;
                }
            }
            else
            {
                if (value == CellEnum.TypeOfCell.Empty)
                {
                    Data.CountFillCell--;
                }

                if (cellType != CellEnum.TypeOfCell.Bug)
                {
                    Data.CurrentCountObjects[(int)cellType]--;
                }
            }

            if (value != CellEnum.TypeOfCell.Empty)
            {
                if (value != CellEnum.TypeOfCell.Bug)
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
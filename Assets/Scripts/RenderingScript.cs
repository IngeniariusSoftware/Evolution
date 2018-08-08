using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/// <summary>
/// Скрипт для отрисовки всех объектов карты
/// </summary>
public class RenderingScript : MonoBehaviour
{
    public static Color Capacity = new Color(1, 1, 1, 0);

    /// <summary>
    ///     Лист всех клеток на отрисовку
    /// </summary>
    public static List<Cell> RendredCells = new List<Cell>();

    /// <summary>
    ///     Лист жуков, которых необходимо отрисовать
    /// </summary>
    public static List<Bug> RendredCellsBug = new List<Bug>();

    /// <summary>
    ///     Лист жуков-объектов для отрисовки на карте
    /// </summary>
    public static List<GameObject> RenderingBugs = new List<GameObject>();

    /// <summary>
    ///     Лист все объектов для отрисовка на карте
    /// </summary>
    public static List<GameObject> RenderingObjects = new List<GameObject>();

    /// <summary>
    ///     Лист всех используемых изображений
    /// </summary>
    public static List<Sprite> Sprites = new List<Sprite>();

    /// <summary>
    ///     Объект карты, на который вешаются изображения
    /// </summary>
    public static GameObject Object;

    /// <summary>
    ///     Массив всех объектов Unity на карте
    /// </summary>
    private static Transform[,] MapObjects = new Transform[Data.MapSize.Y, Data.MapSize.X];

    /// <summary>
    ///     Инициализация стандартного объекта карты, загрузка всех необходимых изображений
    /// </summary>
    void Awake()
    {
        Object = Resources.Load<GameObject>("Empty");
        for (int i = 0; i < 7; i++)
        {
            Sprites.Add(
                Resources.Load<Sprite>("Sprites/" + CellEnum.GetCellType(i).ToString().Replace("TypeOfCell.", "")));
        }
    }

    /// <summary>
    ///     Сгенировать все объекты на карте
    /// </summary>
    public static void InitializeObjects()
    {
        for (int x = 0; x < Data.MapSize.X; x++)
        {
            for (int y = 0; y < Data.MapSize.Y; y++)
            {
                MapObjects[y, x] = Instantiate(
                    Object.transform,
                    new Vector3((x - Data.MapSize.X / 2) * Data.CellSizeX, (y - Data.MapSize.Y / 2) * Data.CellSizeY),
                    new Quaternion(0, 0, 0, 0));

            }
        }
    }

    /// <summary>
    ///     Сгенировать объекты для отрисовки, каждый шаг обновлять их положение и прозрачность
    /// </summary>
    public static void UpdateObjects()
    {
        Capacity.a = (float)Data.CurrentStepsRendering / Data.MaxStepsRendering;
        if (Data.CurrentStepsRendering == 0)
        {
            foreach (Cell rendredCell in RendredCells)
            {
                if (rendredCell.LinkedBug != null)
                {
                    if (rendredCell.LinkedBug.LastPosition != null)
                    {
                        RenderingBugs.Add(
                            Instantiate(
                                Object,
                                new Vector3(
                                    (rendredCell.LinkedBug.LastPosition.X - Data.MapSize.X / 2) * Data.CellSizeX,
                                    (rendredCell.LinkedBug.LastPosition.Y - Data.MapSize.Y / 2) * Data.CellSizeY,
                                    -3),
                                new Quaternion(0, 0, 0, 0)));
                        RenderingBugs.Last().GetComponent<SpriteRenderer>().sprite =
                            Sprites[(int)CellEnum.TypeOfCell.Bug];
                        MapObjects[rendredCell.LinkedBug.LastPosition.Y, rendredCell.LinkedBug.LastPosition.X]
                            .GetComponent<SpriteRenderer>().sprite = Sprites[(int)CellEnum.TypeOfCell.Empty];
                        RendredCellsBug.Add(rendredCell.LinkedBug);
                    }
                    else
                    {
                        RenderingObjects.Add(
                            Instantiate(
                                Object,
                                new Vector3(
                                    (rendredCell.Coordinate.X - Data.MapSize.X / 2) * Data.CellSizeX,
                                    (rendredCell.Coordinate.Y - Data.MapSize.Y / 2) * Data.CellSizeY,
                                    -3),
                                new Quaternion(0, 0, 0, 0)));
                        RenderingObjects.Last().GetComponent<SpriteRenderer>().sprite =
                            Sprites[(int)CellEnum.TypeOfCell.Bug];
                    }

                }
                else
                {
                    RenderingObjects.Add(
                        Instantiate(
                            Object,
                            new Vector3(
                                (rendredCell.Coordinate.X - Data.MapSize.X / 2) * Data.CellSizeX,
                                (rendredCell.Coordinate.Y - Data.MapSize.Y / 2) * Data.CellSizeY,
                                -2),
                            new Quaternion(0, 0, 0, 0)));
                    RenderingObjects.Last().GetComponent<SpriteRenderer>().sprite = Sprites[(int)rendredCell.CellType];
                    RenderingObjects.Last().GetComponent<SpriteRenderer>().color = Capacity;
                }
            }

            Data.CurrentStepsRendering++;
        }
        else
        {
            if (Data.CurrentStepsRendering < Data.MaxStepsRendering)
            {
                for (int i = 0; i < RenderingBugs.Count; i++)
                {
                    Vector2 currentPosition = new Vector2(
                        RendredCellsBug[i].CurrentPosition.X,
                        RendredCellsBug[i].CurrentPosition.Y);
                    Vector2 lastPosition = new Vector2(
                        RendredCellsBug[i].LastPosition.X,
                        RendredCellsBug[i].LastPosition.Y);
                    RenderingBugs[i].transform.position +=
                        (Vector3)((currentPosition - lastPosition) * Data.CellSizeX
                                  / Data.MaxStepsRendering); // Размер клетки только по х, опасно
                }

                foreach (GameObject renderingObject in RenderingObjects)
                {
                    renderingObject.GetComponent<SpriteRenderer>().color = Capacity;
                }
            }
            else
            {

                foreach (Cell rendredCell in RendredCells)
                {
                    MapObjects[rendredCell.Coordinate.Y, rendredCell.Coordinate.X].GetComponent<SpriteRenderer>().sprite
                        = Sprites[(int)rendredCell.CellType];
                }


                foreach (var renderingBug in RenderingBugs)
                {
                    Destroy(renderingBug);
                }

                foreach (var renderingObject in RenderingObjects)
                {
                    Destroy(renderingObject);
                }

                RenderingObjects.Clear();
                RendredCells.Clear();
                RenderingBugs.Clear();
                RendredCellsBug.Clear();
            }

            Data.CurrentStepsRendering++;
        }
    }

    /// <summary>
    ///     Занести объект в лист для дальнейшей отрисовки
    /// </summary>
    /// <param name="cell"> Клетка, в которой изменилось значение, и её необходимо отрисовать </param>
    public static void UpdateTypeCell(Cell cell)
    {
        if (!RendredCells.Contains(cell)) 
        {
            RendredCells.Add(cell);
        }
        else
        {
            RendredCells[RendredCells.IndexOf(cell)] = cell;
        }
    }
}
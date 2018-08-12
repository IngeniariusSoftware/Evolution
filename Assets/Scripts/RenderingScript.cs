using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/// <summary>
/// Скрипт для отрисовки всех объектов карты
/// </summary>
public class RenderingScript : MonoBehaviour
{
    #region Constants 

    /// <summary>
    ///     Максимальный шаг отрисовки (Влияет на плавность отрисовки хода)
    /// </summary>
    public static int MaxStepsRendering = 24;

    #endregion

    /// <summary>
    ///     Текущий шаг отрисовки
    /// </summary>
    public static int CurrentStepsRendering;

    /// <summary>
    ///     Режим отрисовки
    /// </summary>
    public static bool RenderingMode = true;

    /// <summary>
    /// Прозрачность изображений на данном шаге
    /// </summary>
    public static float Capacity = 0;

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
    private static Transform[,] MapObjects = new Transform[Map.Size.Y, Map.Size.X];

    /// <summary>
    ///     Инициализация стандартного объекта карты, загрузка всех необходимых изображений
    /// </summary>
    void Awake()
    {
        Object = Resources.Load<GameObject>("Empty");
        for (int i = 0; i < Map.MaxCountObjects.Length; i++)
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
        for (int x = 0; x < Map.Size.X; x++)
        {
            for (int y = 0; y < Map.Size.Y; y++)
            {
                MapObjects[y, x] = Instantiate(
                    Object.transform,
                    new Vector3((x - Map.Size.X / 2) * Cell.SizeX, (y - Map.Size.Y / 2) * Cell.SizeY),
                    new Quaternion(0, 0, 0, 0));

            }
        }
    }

    /// <summary>
    ///     Сгенировать объекты для отрисовки, каждый шаг обновлять их положение и прозрачность
    /// </summary>
    public static void UpdateObjects()
    {
        Capacity = (float)CurrentStepsRendering / MaxStepsRendering;
        if (CurrentStepsRendering == 0)
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
                                    (rendredCell.LinkedBug.LastPosition.X - Map.Size.X / 2) * Cell.SizeX,
                                    (rendredCell.LinkedBug.LastPosition.Y - Map.Size.Y / 2) * Cell.SizeY,
                                    -3),
                                new Quaternion(0, 0, 0, 0)));
                        RenderingBugs.Last().GetComponent<SpriteRenderer>().sprite =
                            Sprites[(int)CellEnum.TypeOfCell.Bug];
                        if (RenderingMode)
                        {
                            RenderingBugs.Last().GetComponent<SpriteRenderer>().color = rendredCell.LinkedBug.color;
                        }
                        else
                        {
                            RenderingBugs.Last().GetComponent<SpriteRenderer>().color = new Color(
                                1,
                                (float)(Bug.MaxBugHealth - rendredCell.LinkedBug.Health) / Bug.MaxBugHealth,
                                (float)(Bug.MaxBugHealth - rendredCell.LinkedBug.Health) / Bug.MaxBugHealth,
                                1);
                        }

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
                                    (rendredCell.Coordinate.X - Map.Size.X / 2) * Cell.SizeX,
                                    (rendredCell.Coordinate.Y - Map.Size.Y / 2) * Cell.SizeY,
                                    -3),
                                new Quaternion(0, 0, 0, 0)));
                        RenderingObjects.Last().GetComponent<SpriteRenderer>().sprite =
                            Sprites[(int)CellEnum.TypeOfCell.Bug];
                        Color color = RenderingObjects.Last().GetComponent<SpriteRenderer>().color;
                        if (RenderingMode)
                        {
                            color = rendredCell.LinkedBug.color;
                        }
                        else
                        {
                            color = new Color(
                                1,
                                (float)(Bug.MaxBugHealth - rendredCell.LinkedBug.Health) / Bug.MaxBugHealth,
                                (float)(Bug.MaxBugHealth - rendredCell.LinkedBug.Health) / Bug.MaxBugHealth,
                                1);
                        }

                        color.a = Capacity;
                        RenderingObjects.Last().GetComponent<SpriteRenderer>().color = color;
                    }
                }
                else
                {
                    RenderingObjects.Add(
                        Instantiate(
                            Object,
                            new Vector3(
                                (rendredCell.Coordinate.X - Map.Size.X / 2) * Cell.SizeX,
                                (rendredCell.Coordinate.Y - Map.Size.Y / 2) * Cell.SizeY,
                                -2),
                            new Quaternion(0, 0, 0, 0)));
                    if (RenderingMode)
                    {
                        RenderingObjects.Last().GetComponent<SpriteRenderer>().sprite =
                            Sprites[(int)rendredCell.CellType];
                    }
                    else
                    {
                        RenderingObjects.Last().GetComponent<SpriteRenderer>().sprite =
                            Sprites[(int)CellEnum.TypeOfCell.Empty];
                    }

                    Color color = RenderingObjects.Last().GetComponent<SpriteRenderer>().color;
                    color.a = Capacity;
                    RenderingObjects.Last().GetComponent<SpriteRenderer>().color = color;
                }
            }

            CurrentStepsRendering++;
        }
        else
        {
            if (CurrentStepsRendering < MaxStepsRendering)
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
                        (Vector3)((currentPosition - lastPosition) * Cell.SizeX
                                  / MaxStepsRendering); // Размер клетки только по х, опасно
                }

                foreach (GameObject renderingObject in RenderingObjects)
                {
                    Color color = renderingObject.GetComponent<SpriteRenderer>().color;
                    color.a = Capacity;
                    renderingObject.GetComponent<SpriteRenderer>().color = color;
                }
            }
            else
            {
                ResetRendering();
            }

            CurrentStepsRendering++;
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

    public static void ResetRendering()
    {
        foreach (Cell renderedCell in RendredCells)
        {
            if (RenderingMode)
            {
                if (renderedCell.LinkedBug != null)
                {
                    MapObjects[renderedCell.Coordinate.Y, renderedCell.Coordinate.X].GetComponent<SpriteRenderer>()
                        .sprite = Sprites[(int)CellEnum.TypeOfCell.Bug];
                    MapObjects[renderedCell.Coordinate.Y, renderedCell.Coordinate.X].GetComponent<SpriteRenderer>()
                        .color = renderedCell.LinkedBug.color;
                }
                else
                {
                    MapObjects[renderedCell.Coordinate.Y, renderedCell.Coordinate.X].GetComponent<SpriteRenderer>()
                        .sprite = Sprites[(int)renderedCell.CellType];
                    MapObjects[renderedCell.Coordinate.Y, renderedCell.Coordinate.X].GetComponent<SpriteRenderer>()
                        .color = Color.white;
                }
            }
            else
            {
                if (renderedCell.LinkedBug != null)
                {
                    MapObjects[renderedCell.Coordinate.Y, renderedCell.Coordinate.X].GetComponent<SpriteRenderer>()
                        .color = new Color(
                        1,
                        (float)(Bug.MaxBugHealth - renderedCell.LinkedBug.Health) / Bug.MaxBugHealth,
                        (float)(Bug.MaxBugHealth - renderedCell.LinkedBug.Health) / Bug.MaxBugHealth,
                        1);
                    MapObjects[renderedCell.Coordinate.Y, renderedCell.Coordinate.X].GetComponent<SpriteRenderer>()
                        .sprite = Sprites[(int)CellEnum.TypeOfCell.Bug];
                }
                else
                {
                    MapObjects[renderedCell.Coordinate.Y, renderedCell.Coordinate.X].GetComponent<SpriteRenderer>()
                        .sprite = Sprites[(int)CellEnum.TypeOfCell.Empty];
                    MapObjects[renderedCell.Coordinate.Y, renderedCell.Coordinate.X].GetComponent<SpriteRenderer>()
                        .color = Color.white;
                }
            }
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
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using UnityEngine;

[DataContract]
public class Bug
{
    #region Constants

    /// <summary>
    /// Количество жизней, с которым появляется жук
    /// </summary>
    public const int StartBugHealth = 50;

    /// <summary>
    /// Максимальное количество жизней у жука
    /// </summary>
    public const int MaxBugHealth = 256;

    /// <summary>
    /// Количество жизней, на которое уменьшается жизнь жука-родителя при генерации нового жука
    /// </summary>
    public const int MuptiplyCost = 50;

    /// <summary>
    /// Количество жизней, которое получаает жук, съедая минеральную ягоду
    /// </summary>
    public const int MineralBerryValue = 75;

    /// <summary>
    /// Количество жизней, которое получаает жук, съедая обычную ягоду
    /// </summary>
    public const int BerryValue = 50;

    /// <summary>
    /// Количество жизней, которое получаает жук, съедая водоросли
    /// </summary>
    public const int SeaweedValue = 100;

    /// <summary>
    /// Максимальное количество шагов, которое жук может прожить
    /// </summary>
    public const int MaxLifeTime = 512;

    #endregion

    #region Constructors

    /// <summary>
    /// Создание жука
    /// </summary>
    /// <param name="bugColor"> Цвет жука </param>
    /// <param name="genome"> Геном жука, если есть </param>
    /// <param name="currentPosition"> Позиция создания жука, если есть </param>
    public Bug(Color bugColor, Genome genome = null, Coordinates currentPosition = null)
    {
        if (currentPosition == null)
        {
            currentPosition = Map.FindEmptyCell(Cell.TypeOfCell.Bug)?.Coordinate;
        }

        if (currentPosition == null)
        {
            currentPosition = Map.FindSeaCell().Coordinate;
        }

        CurrentPosition = currentPosition;
        Map.GetMapCell(currentPosition.Y, currentPosition.X).Content = Cell.TypeOfCell.Bug;
        if (genome == null)
        {
            Gene = new Genome();
        }
        else
        {
            Gene = genome;
        }

        Health = StartBugHealth;
        Gene.CurrentGenePosition = 0;
        Direction = Data.Rnd.Next(0, 8);
        color = bugColor;
        Map.GetMapCell(currentPosition.Y, currentPosition.X).LinkedBug = this;
    }


    #endregion

    #region Fields

    /// <summary>
    ///     Количество шагов, которое прожил жук
    /// </summary>
    public int currentLifeTime = 0;

    [DataMember]
    private Color _color;

    private int _health = 50;

    #endregion

    #region Properties

    public Color color
    {
        get
        {
            return _color;
        }

        set
        {
            _color = value;

            if (value.b < 0)
            {
                _color.b = 0;
            }

            if (value.b > 1)
            {
                _color.b = 1;
            }

            if (value.g < 0)
            {
                _color.g = 0;
            }

            if (value.g > 1)
            {
                _color.g = 1;
            }

            if (value.r < 0)
            {
                _color.r = 0;
            }

            if (value.r > 1)
            {
                _color.r = 1;
            }

            if (value.a < 0)
            {
                _color.a = 0;
            }

            if (value.a > 1)
            {
                _color.a = 1;
            }
        }
    }

    [DataMember]
    public int Direction { get; set; }

    [DataMember]
    public int LifeTime
    {
        get
        {
            return currentLifeTime;
        }

        set
        {
            if (value >= MaxLifeTime)
            {
                currentLifeTime = MaxLifeTime;
            }
            else
            {
                currentLifeTime = value;
            }
        }
    }

    [DataMember]
    public int Health
    {
        get
        {
            return _health;
        }

        set
        {
            if (value >= MaxBugHealth)
            {
                _health = MaxBugHealth;
            }

            if (value > -1 && value <= MaxBugHealth)
            {
                _health = value;
            }

            if (value < 0)
            {
                _health = 0;
            }
        }
    }

    [DataMember]
    public Genome Gene { get; set; }

    public Coordinates LastPosition { get; set; }

    public Coordinates CurrentPosition { get; set; }

    #endregion

    private static Cell DestinationCell;

    /// <summary>
    /// Массив делегатов, который хранит команды жука
    /// </summary>
    public void StartAction()
    {
        LastPosition = CurrentPosition;
        int countSteps = 0;
        bool isEnd = false;
        Health--;
        LifeTime++;
        color = new Color(color.r - 0.0002f, color.g - 0.0002f, color.b - 0.0002f);
        while (countSteps < BugCollection.MaxStepsBug && !isEnd)
        {
            isEnd = DoCommand(this);
            countSteps++;
        }

        // Убить жука, если он превысил допустимое количество команд за один ход
        //if (countSteps == BugCollection.MaxStepsBug && !isEnd)
        //{
        //    Health = 0;
        //}
    }

    /// <summary>
    /// Считает промежуточные данные, необходимые для выполнения дальнейших команд
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    public static bool DoCommand(Bug bug)
    {
        Coordinates destination = Coordinates.CoordinateShift[bug.CalculateShift()] + bug.CurrentPosition;
        DestinationCell = Map.GetMapCell(destination.Y, destination.X);
        if (Math.Abs(bug.Health - MaxBugHealth) < 10)
        {
            return Multiply(bug);
        }
        else
        {
            // Если данному номеру генома присвоена команда, тогда необходимо её выполнить
            if (MasBugCommands.Length > bug.Gene.genome[bug.Gene.CurrentGenePosition])
            {
                return MasBugCommands[bug.Gene.genome[bug.Gene.CurrentGenePosition]].Invoke(bug);
            }
            else
            {
                // Иначе сделать переход по геному
                bug.Gene.CurrentGenePosition++;
                return GenomJump(bug);
            }
        }
    }

    /// <summary>
    ///     Делегат, аналогичный всем командам жука, необходим для хранения той или иной команды
    /// </summary>
    public delegate bool BugCommand(Bug bug);

    /// <summary>
    ///     Массив делегатов, которые хранят команды жука
    /// </summary>
    public static BugCommand[] MasBugCommands =
        {
            Move, Rotate, CheckCell, Take, Multiply, Push, CheckHealth, Photosynthesize, CheckHealthNeighbor, Attack,
            Share, Swim
        };

    /// <summary>
    /// Метод, который считает направление жука с учётом его текущего положения и следующей ячейки генома
    /// </summary>
    private int CalculateShift()
    {
        // С учетом текущего поворота
        int direction = (Direction + Gene.genome[Gene.NextGenePosition()]) % 8;
        return direction;
    }

    #region BugCommands

    /// <summary>
    /// Жук пытается сходить на определённую клетку рядом с собой
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool Move(Bug bug)
    {
        CheckCell(bug);
        if (DestinationCell.Surface != Cell.TypeOfCell.Sea)
        {
            switch (DestinationCell.Content)
            {
                case Cell.TypeOfCell.Empty:
                    {
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).LinkedBug = null;
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).Content = Cell.TypeOfCell.Empty;
                        bug.CurrentPosition = DestinationCell.Coordinate;
                        DestinationCell.Content = Cell.TypeOfCell.Bug;
                        DestinationCell.LinkedBug = bug;
                        break;
                    }

                case Cell.TypeOfCell.Berry:
                    {
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).LinkedBug = null;
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).Content = Cell.TypeOfCell.Empty;
                        bug.CurrentPosition = DestinationCell.Coordinate;
                        DestinationCell.Content = Cell.TypeOfCell.Bug;
                        DestinationCell.LinkedBug = bug;
                        bug.Health += BerryValue;
                        bug.color = new Color(bug.color.r, bug.color.g + 0.01f, bug.color.b);
                        break;
                    }

                case Cell.TypeOfCell.MineralBerry:
                    {
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).LinkedBug = null;
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).Content = Cell.TypeOfCell.Empty;
                        bug.CurrentPosition = DestinationCell.Coordinate;
                        DestinationCell.Content = Cell.TypeOfCell.Bug;
                        DestinationCell.LinkedBug = bug;
                        bug.Health += MineralBerryValue;
                        bug.color = new Color(bug.color.r, bug.color.g, bug.color.b + 0.01f);
                        break;
                    }

                case Cell.TypeOfCell.Prickle:
                    {
                        bug.Health -= 10;
                        break;
                    }

                case Cell.TypeOfCell.Poison:
                    {
                        bug.Health = 0;
                        break;
                    }
            }
        }

        return true;
    }

    /// <summary>
    /// Жук проверяет определённую клетку рядом с собой
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool CheckCell(Bug bug)
    {
        bug.Gene.CurrentGenePosition += (int)DestinationCell.Content + 1;
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null && neighbourBug.IsFriendBug(bug))
        {
            bug.Gene.CurrentGenePosition++;
        }

        bug.Gene.NextGenePosition(bug.Gene.genome[bug.Gene.CurrentGenePosition]);
        return false;
    }

    /// <summary>
    /// Жук пытается взять содержимое определённой клетки рядом с собой
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool Take(Bug bug)
    {
        CheckCell(bug);
        switch (DestinationCell.Content)
        {
            case Cell.TypeOfCell.Berry:
                {
                    DestinationCell.Content = Cell.TypeOfCell.Empty;
                    bug.Health += BerryValue;
                    bug.color = new Color(bug.color.r, bug.color.g + 0.01f, bug.color.b);
                    break;
                }

            case Cell.TypeOfCell.MineralBerry:
                {
                    DestinationCell.Content = Cell.TypeOfCell.Empty;
                    bug.Health += MineralBerryValue;
                    bug.color = new Color(bug.color.r, bug.color.g, bug.color.b + 0.01f);
                    break;
                }

            case Cell.TypeOfCell.Poison:
                {
                    DestinationCell.Content = Cell.TypeOfCell.Berry;
                    break;
                }

            case Cell.TypeOfCell.Seaweed:
                {
                    DestinationCell.Content = Cell.TypeOfCell.Empty;
                    bug.Health += SeaweedValue;
                    bug.color = new Color(bug.color.r, bug.color.g, bug.color.b + 0.05f);
                    break;
                }
        }

        return true;
    }

    /// <summary>
    /// Комадна сравнивает геномы текущего и переданного жуков, если геном отличается больше чем на 1 ячейку, считается, что жуки не родственники
    /// </summary>
    /// <param name="bug"> Жук, которого надо проверить на родство с нашим  </param>
    public bool IsFriendBug(Bug bug)
    {
        bool isDifference = false;
        for (int i = 0; i < Gene.genome.Length; i++)
        {
            if (Gene.genome[i] != bug.Gene.genome[i])
            {
                if (isDifference)
                {
                    return false;
                }
                else
                {
                    isDifference = true;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Жук поворачивается определённое количество раз
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool Rotate(Bug bug)
    {
        bug.Direction = (bug.Direction + bug.Gene.genome[bug.Gene.NextGenePosition()]) % 8;
        bug.Gene.CurrentGenePosition += 2;
        return false;
    }

    /// <summary>
    /// Жук создает одного потомка рядом с собой
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool Multiply(Bug bug)
    {
        bug.Health -= MuptiplyCost;
        bool isBorn = false;

        // Вместо обхода по часовой стрелки вокруг клетки, будем использовать рандомные сдвиги с запоминанием
        List<byte> randomShifts = new List<byte>(8);
        for (byte i = 0; i < randomShifts.Capacity; i++)
        {
            randomShifts.Add(i);
        }

        // Ищем свободную клетку до тех пор, пока не проверим все 8 позиций вокруг клетки, либо найдем пустую
        while (randomShifts.Count > 0 && !isBorn)
        {
            byte shift = (byte)Data.Rnd.Next(0, randomShifts.Count);
            Coordinates birthCoordinate = bug.CurrentPosition + Coordinates.CoordinateShift[randomShifts[shift]];
            if (Map.GetMapCell(birthCoordinate.Y, birthCoordinate.X).Content == Cell.TypeOfCell.Empty)
            {
                Bug childBug = new Bug(bug.color, new Genome(bug.Gene, bug.LifeTime), birthCoordinate);
                ControlScript.childs.Add(childBug);
                isBorn = true;
                childBug.Health = Math.Min(MuptiplyCost, bug.Health);
            }

            randomShifts.Remove(randomShifts[shift]);
        }

        if (!isBorn)
        {
            bug.Health = 0;
        }

        bug.Gene.CurrentGenePosition++;
        return true;
    }

    /// <summary>
    /// Жук пытается толкнуть содержимое определённой клетки рядом с собой
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool Push(Bug bug)
    {
        CheckCell(bug);
        if (DestinationCell.Content != Cell.TypeOfCell.Wall)
        {
            if (DestinationCell.Content == Cell.TypeOfCell.Bamboo)
            {
                if (Data.Rnd.Next(0, 3) == 0)
                {
                    DestinationCell.Content = Cell.TypeOfCell.Poison;
                }
                else
                {
                    DestinationCell.Content = Cell.TypeOfCell.Berry;
                }
            }
            else
            {
                var pushDestination =
                    Coordinates.CoordinateShift[(bug.Direction + bug.Gene.genome[bug.Gene.NextGenePosition()]) % 8]
                    + DestinationCell.Coordinate;
                var pushDestinationCell = Map.GetMapCell(pushDestination.Y, pushDestination.X);
                if (pushDestinationCell.Content == Cell.TypeOfCell.Empty)
                {
                    pushDestinationCell.Content = DestinationCell.Content;
                    if (pushDestinationCell.Content == Cell.TypeOfCell.Bug)
                    {
                        pushDestinationCell.LinkedBug = DestinationCell.LinkedBug;
                        pushDestinationCell.LinkedBug.CurrentPosition = pushDestination;
                        DestinationCell.LinkedBug = null;
                    }

                    DestinationCell.Content = Cell.TypeOfCell.Empty;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Жук проверяет текущее количесвто своих жизней
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool CheckHealth(Bug bug)
    {
        if (bug.Health > bug.Gene.genome[bug.Gene.NextGenePosition()] * MaxBugHealth / Genome.LengthGenome)
        {
            bug.Gene.CurrentGenePosition += 2;
        }
        else
        {
            bug.Gene.CurrentGenePosition += 3;
        }

        bug.Gene.NextGenePosition(bug.Gene.genome[bug.Gene.CurrentGenePosition]);
        return false;
    }

    /// <summary>
    /// Жук пытается атаковать содрежимое определённой клетки рядом с собой
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool Attack(Bug bug)
    {
        CheckCell(bug);
        switch (DestinationCell.Content)
        {
            case Cell.TypeOfCell.Mineral:
                {
                    DestinationCell.Content = Cell.TypeOfCell.MineralBerry;
                    break;
                }

            case Cell.TypeOfCell.Bug:
                {
                    if (DestinationCell.LinkedBug != null)
                    {
                        bug.Health += 5;
                        bug.color = new Color(bug.color.r + 0.001f, bug.color.g, bug.color.b);
                        DestinationCell.LinkedBug.Health -= 5;
                    }

                    break;
                }

            case Cell.TypeOfCell.Wall:
                {
                    break;
                }

            case Cell.TypeOfCell.Prickle:
                {
                    DestinationCell.Content = Cell.TypeOfCell.Empty;
                    bug.Health -= 10;
                    break;
                }

            case Cell.TypeOfCell.Poison:
                {
                    bug.Health = 0;
                    break;
                }

            default:
                {
                    DestinationCell.Content = Cell.TypeOfCell.Empty;
                    break;
                }
        }

        return true;
    }

    /// <summary>
    /// Жук пытается поделиться едой с содержимым определённой клетки рядом с собой
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool Share(Bug bug)
    {
        CheckCell(bug);
        var neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null)
        {
            if (bug.Health < 10)
            {
                neighbourBug.Health += bug.Health;
            }

            bug.Health -= 10;
        }

        return true;
    }

    /// <summary>
    /// Жук пытается сравнить свою жизнь, с жизнью соседнего жука
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool CheckHealthNeighbor(Bug bug)
    {
        Bug neighbourBug = DestinationCell.LinkedBug;
        if (neighbourBug != null)
        {
            if (neighbourBug.IsFriendBug(bug))
            {
                if (neighbourBug.Health > bug.Health)
                {
                    bug.Gene.CurrentGenePosition += 2;
                }
                else
                {
                    bug.Gene.CurrentGenePosition += 3;
                }
            }
            else
            {
                if (neighbourBug.Health > bug.Health)
                {
                    bug.Gene.CurrentGenePosition += 4;
                }
                else
                {
                    bug.Gene.CurrentGenePosition += 5;
                }
            }
        }
        else
        {
            bug.Gene.CurrentGenePosition++;
        }

        bug.Gene.NextGenePosition(bug.Gene.genome[bug.Gene.CurrentGenePosition]);
        return false;
    }

    /// <summary>
    /// Жук может получать +2 к жизни, если применит команду к солнцу
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool Photosynthesize(Bug bug)
    {
        CheckCell(bug);
        if (DestinationCell.Content == Cell.TypeOfCell.Sun)
        {
            bug.Health += 6;
            bug.color = new Color(bug.color.r + 0.001f, bug.color.g + 0.001f, bug.color.b);
            //Определённый шанс, что жук сломает солнце
            if (Data.Rnd.Next(0, 200) == 0)
            {
                DestinationCell.Content = Cell.TypeOfCell.Empty;
            }
        }

        return true;
    }

    private static bool Swim(Bug bug)
    {
        CheckCell(bug);
        if (DestinationCell.Surface == Cell.TypeOfCell.Sea)
        {
            switch (DestinationCell.Content)
            {
                case Cell.TypeOfCell.Empty:
                    {
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).LinkedBug = null;
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).Content = Cell.TypeOfCell.Empty;
                        bug.CurrentPosition = DestinationCell.Coordinate;
                        DestinationCell.Content = Cell.TypeOfCell.Bug;
                        DestinationCell.LinkedBug = bug;
                        break;
                    }

                case Cell.TypeOfCell.Seaweed:
                    {
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).LinkedBug = null;
                        Map.GetMapCell(bug.CurrentPosition.Y, bug.CurrentPosition.X).Content = Cell.TypeOfCell.Empty;
                        bug.CurrentPosition = DestinationCell.Coordinate;
                        DestinationCell.Content = Cell.TypeOfCell.Bug;
                        DestinationCell.LinkedBug = bug;
                        bug.Health += SeaweedValue;
                        bug.color = new Color(bug.color.r, bug.color.g + 0.01f, bug.color.b);
                        break;
                    }
            }
        }

        return true;
    }


    /// <summary>
    /// Выполняет прыжок по геному в зависимости от параметра
    /// </summary>
    /// <param name="bug"> Жук, который сейчас ходит  </param>
    private static bool GenomJump(Bug bug)
    {
        bug.Gene.CurrentGenePosition += bug.Gene.genome[bug.Gene.CurrentGenePosition];
        return false;
    }

    #endregion
}
using System;
using System.Collections.Generic;

/// <summary>
/// Класс, предназначенный для хранения основных данных
/// </summary>
public static class Data
{
    #region Constants
    /// <summary>
    ///             
    /// </summary>
    public static readonly string SavePath = @"BugGenerations\";

    /// <summary>
    ///     Переменная для генерации случайный значений
    /// </summary>
    public static readonly Random Rnd = new Random();

    /// <summary>
    ///     Начальное количество жуков на карте
    /// </summary>
    public static readonly int BugCount = 100;

    /// <summary>
    ///     Карта мира, состоящая из клеток
    /// </summary>
    public static Cell[,] WorldMap = new Cell[Map.Size.Y, Map.Size.X];

    /// <summary>
    ///     Текущее количество различных объектов на карте
    /// </summary>
    public static readonly int[] CurrentCountObjects = new int[Map.MaxCountObjects.Length];

    /// <summary>
    ///     Текущий ход
    /// </summary>
    public static int CurrentGameStep = 0;

    /// <summary>
    ///     Количество мертвых жуков
    /// </summary>
    public static int NumberDeadBugs = 0;



    public static List<string> SaveGames = new List<string>();

    #endregion
}
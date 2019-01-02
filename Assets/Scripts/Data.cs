using System;
using System.Collections.Generic;

/// <summary>
/// Класс, предназначенный для хранения основных данных
/// </summary>
public static class Data
{
    #region Constants

    /// <summary>
    /// Переменная для генерации случайный значений
    /// </summary>
    public static readonly Random Rnd = new Random();

    /// <summary>
    /// Начальное количество жуков на карте
    /// </summary>
    public static readonly int BugCount = 20;

    /// <summary>
    /// Текущий ход
    /// </summary>
    public static int CurrentGameStep = 0;

    /// <summary>
    /// Количество мертвых жуков
    /// </summary>
    public static int NumberDeadBugs = 0;

    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за логику меню, в котором отображается основная статистика игры
/// </summary>
public class StatisticsMenu : MonoBehaviour
{
    /// <summary>
    /// Массив полей текста, на которые выводится информация о текущей статистики игры
    /// </summary>
    public Text[] StatisticsText;

    /// <summary>
    /// Каждый кадр обновляется статистика
    /// </summary>
    void Update()
    {

        StatisticsText[0].text = "Поколение: " + ControlScript.bugs.GenerationNumber;
        StatisticsText[1].text = "Шаг: " + Data.CurrentGameStep;
        StatisticsText[2].text = "Жуков: " + ControlScript.bugs.CountBugs;
        StatisticsText[3].text = "Мёртв. жуков: " + Data.NumberDeadBugs;
        StatisticsText[4].text = "Обыч. ягод: " + Map.CellLists[(int)Cell.TypeOfCell.Berry].Count;
        StatisticsText[5].text = "Яда: " + Map.CellLists[(int)Cell.TypeOfCell.Poison].Count;
        StatisticsText[6].text = "Стен: " + Map.CellLists[(int)Cell.TypeOfCell.Wall].Count;
        StatisticsText[7].text = "Минералов: " + Map.CellLists[(int)Cell.TypeOfCell.Mineral].Count;
        StatisticsText[8].text = "Минер. ягод: " + Map.CellLists[(int)Cell.TypeOfCell.MineralBerry].Count;
        StatisticsText[9].text = "Бамбука: " + Map.CellLists[(int)Cell.TypeOfCell.Bamboo].Count;
    }
}
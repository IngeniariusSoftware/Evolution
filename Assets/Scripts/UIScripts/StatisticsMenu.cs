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
        StatisticsText[4].text = "Обыч. ягод: " + Data.CurrentCountObjects[(int)CellEnum.TypeOfCell.Berry];
        StatisticsText[5].text = "Яда: " + Data.CurrentCountObjects[(int)CellEnum.TypeOfCell.Poison];
        StatisticsText[6].text = "Стен: " + Data.CurrentCountObjects[(int)CellEnum.TypeOfCell.Wall];
        StatisticsText[7].text = "Минералов: " + Data.CurrentCountObjects[(int)CellEnum.TypeOfCell.Mineral];
        StatisticsText[8].text = "Минер. ягод: " + Data.CurrentCountObjects[(int)CellEnum.TypeOfCell.MineralBerry];
        StatisticsText[9].text = "Бамбука: " + Data.CurrentCountObjects[(int)CellEnum.TypeOfCell.Bamboo];
    }
}

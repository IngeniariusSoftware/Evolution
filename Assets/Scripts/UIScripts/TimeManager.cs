using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

/// <summary>
/// Класс, отвечающий за регулирование скорости игры
/// </summary>
public class TimeManager : MonoBehaviour
{
    /// <summary>
    /// Поле отвечающие за количество шагов отрисовки
    /// </summary>
    public static int TimeSpeed = RenderingScript.MaxStepsRendering;

    /// <summary>
    /// Метод вызывается, когда меняется значение ползунка
    /// </summary>
    public void ChangeTimeSped()
    {
        TimeSpeed = (int)gameObject.GetComponent<Slider>().value;
    }
}

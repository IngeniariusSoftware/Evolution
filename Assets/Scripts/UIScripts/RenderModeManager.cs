using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс для управления режимами отрисовки
/// </summary>
public class RenderModeManager : MonoBehaviour
{
    /// <summary>
    /// Текущий режим отрисовки
    /// </summary>
    public static bool CurrentRenderingMode = RenderingScript.RenderingMode;

    /// <summary>
    /// Переключатель режима отрисовки
    /// </summary>
    public Toggle NormalViewToggle;

    /// <summary>
    /// Переключатель режима отрисовки
    /// </summary>
    public Toggle EnergyViewToggle;

    /// <summary>
    /// Метод изменения режима отрисовки, основан на состояниях переключателей
    /// </summary>
    public void ChangeRenderingMode()
    {
        if (EnergyViewToggle.isOn && NormalViewToggle.isOn)
        {
            if (CurrentRenderingMode)
            {
                NormalViewToggle.isOn = false;
            }
            else
            {
                EnergyViewToggle.isOn = false;
            }

            CurrentRenderingMode = !CurrentRenderingMode;
        }
        else
        {
            if (!EnergyViewToggle.isOn && !NormalViewToggle.isOn)
            {
                if (CurrentRenderingMode)
                {
                    EnergyViewToggle.isOn = true;
                }
                else
                {
                    NormalViewToggle.isOn = true;
                }

                CurrentRenderingMode = !CurrentRenderingMode;
            }
        }
    }
}

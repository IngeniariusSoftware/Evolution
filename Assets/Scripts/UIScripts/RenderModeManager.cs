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
    /// Режим отрисовки
    /// </summary>
    public static RenderModeEnum.RenderingType RenderingMode = RenderModeEnum.RenderingType.Normal;
    
    /// <summary>
    /// Переключатель обычного режима отрисовки
    /// </summary>
    public Toggle NormalViewToggle;

    /// <summary>
    /// Переключатель энергетического режима отрисовки
    /// </summary>
    public Toggle EnergyViewToggle;

    /// <summary>
    /// Переключатель ускоренного режима отрисовки 
    /// </summary>
    public Toggle RewindViewToggle;

    /// <summary>
    /// Переключатель режима отрисовки
    /// </summary>
    private static bool IsSessionBlocked;

    /// <summary>
    /// Методы изменения режима отрисовки, основан на состояниях переключателей
    /// </summary>
    public void OnRewindViewToggle()
    {
        if (!IsSessionBlocked)
        {
            RenderingMode = RenderModeEnum.RenderingType.Rewind;
            IsSessionBlocked = true;
            if (RewindViewToggle.isOn)
            {
                EnergyViewToggle.isOn = false;
                NormalViewToggle.isOn = false;
            }
            else
            {
                RewindViewToggle.isOn = true;
            }
        }
        else
        {
            IsSessionBlocked = false;
        }
    }

    public void OnNormalViewToggle()
    {
        if (!IsSessionBlocked)
        {
            RenderingMode = RenderModeEnum.RenderingType.Normal;
            IsSessionBlocked = true;
            if (NormalViewToggle.isOn)
            {
                EnergyViewToggle.isOn = false;
                RewindViewToggle.isOn = false;
            }
            else
            {
                NormalViewToggle.isOn = true;
            }
        }
        else
        {
            IsSessionBlocked = false;
        }
    }

    public void OnEnergyViewToggle()
    {
        if (!IsSessionBlocked)
        {
            RenderingMode = RenderModeEnum.RenderingType.Energy;
            IsSessionBlocked = true;
            if (EnergyViewToggle.isOn)
            {
                RewindViewToggle.isOn = false;
                NormalViewToggle.isOn = false;
            }
            else
            {
                EnergyViewToggle.isOn = true;
            }
        }
        else
        {
            IsSessionBlocked = false;
        }
    }
}

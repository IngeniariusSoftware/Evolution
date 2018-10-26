using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за логику меню сохранения игры
/// </summary>
public class SaveGameMenu : MonoBehaviour
{
    #region elementsUI

    /// <summary>
    /// Поле для имени сохранения
    /// </summary>
    public InputField SaveNameField;

    /// <summary>
    /// Кнопка для сохранения игры
    /// </summary>
    public Transform SaveButton;

    /// <summary>
    /// Меню сохранения игры
    /// </summary>
    public Transform Menu;

    #endregion

    /// <summary>
    /// Проверка нужно ли закрыть меню
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Menu.GetComponent<CanvasGroup>().alpha == 1)
        {
            Close();
        }
    }

    /// <summary>
    /// Метод, который вызывает сохранения игры
    /// </summary>
    public void Save()
    {
        Close();
        SavesManager.SaveGame(SaveNameField.text);
    }

    /// <summary>
    /// Метод, для проверки пустоты поля игры
    /// </summary>
    public void CheckField()
    {
        if (SaveNameField.text != null)
        {
            SaveButton.GetComponent<CanvasGroup>().alpha = 1;
            SaveButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            SaveButton.GetComponent<CanvasGroup>().alpha = 0;
            SaveButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    /// <summary>
    /// Метод, который открывает меню сохранения игры
    /// </summary>
    public void Open()
    {
        CameraManager.IsActiveUI = true;
        Menu.GetComponent<CanvasGroup>().alpha = 1;
        Menu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        CheckField();
    }

    /// <summary>
    /// Метод, который закрывает меню сохранения игры
    /// </summary>
    public void Close()
    {
        CameraManager.IsActiveUI = false;
        Menu.GetComponent<CanvasGroup>().alpha = 0;
        Menu.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}

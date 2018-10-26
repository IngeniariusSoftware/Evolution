using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за логику меню загрузки сохраненной игры
/// </summary>
public class LoadGameMenu : MonoBehaviour
{
    #region elementsUI

    /// <summary>
    /// Панель загрузуженных игр
    /// </summary>
    public Dropdown SaveGamesList;

    /// <summary>
    /// Кнопка удаления сохранения
    /// </summary>
    public Transform DeleteButton;

    /// <summary>
    /// Кнопки загрузки сохранения
    /// </summary>
    public Transform LoadButton;

    /// <summary>
    /// Меню загрузки игры
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
    /// Метод, который вызывает загрузку игры
    /// </summary>
    public void Load()
    {
        Close();
        SavesManager.NameLoadGame = SaveGamesList.options[SaveGamesList.value].text;
        SavesManager.NeedLoadGame = true;
    }


    /// <summary>
    /// Метод, который вызывает удаление игры
    /// </summary>
    public void Delete()
    {
        SavesManager.DeleteGame(SaveGamesList.options[SaveGamesList.value].text);
        Open();
    }

    /// <summary>
    /// Метод, который открывает меню загрузки
    /// </summary>
    public void Open()
    {
        CameraManager.IsActiveUI = true;
        SaveGamesList.value = 0;
        Menu.GetComponent<CanvasGroup>().alpha = 1;
        Menu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        SaveGamesList.ClearOptions();
        SaveGamesList.AddOptions(SavesManager.GetSaveGames());
        if (SaveGamesList.options.Count == 0)
        {
            List<string> emptySaves = new List<string> { "Сохранения отсутствуют" };
            SaveGamesList.AddOptions(emptySaves);
            DeleteButton.GetComponent<CanvasGroup>().alpha = 0.5f;
            DeleteButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            LoadButton.GetComponent<CanvasGroup>().alpha = 0.5f;
            LoadButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            DeleteButton.GetComponent<CanvasGroup>().alpha = 1;
            DeleteButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            LoadButton.GetComponent<CanvasGroup>().alpha = 1;
            LoadButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    /// <summary>
    /// Метод, который закрывает меню загрузки
    /// </summary>
    public void Close()
    {
        CameraManager.IsActiveUI = false;
        Menu.GetComponent<CanvasGroup>().alpha = 0;
        Menu.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}

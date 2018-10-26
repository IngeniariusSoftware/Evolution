using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за логику меню паузы в игре
/// </summary>
public class PauseGameMenu : MonoBehaviour
{
    #region elementsUI

    /// <summary>
    /// Меню паузы игры
    /// </summary>
    public Transform Menu;
    
    /// <summary>
    /// Открыто ли основное меню
    /// </summary>
    private bool _isOpen = false;

    #endregion

    /// <summary>
    /// Проверка нужно ли закрыть или открыть меню
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    /// <summary>
    /// Метод старта новый игры
    /// </summary>
    public void StartNewGame() //TODO разработать логику
    {
        Debug.Log("Новая игра");
        Close();
    }

    /// <summary>
    /// Метод, который открывает меню паузы
    /// </summary>
    public void Open()
    {
        CameraManager.IsActiveUI = true;
        Menu.GetComponent<CanvasGroup>().alpha = 1;
        Menu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _isOpen = true;
    }

    /// <summary>
    /// Метод, который закрывает меню паузы
    /// </summary>
    public void Close()
    {
        CameraManager.IsActiveUI = false;
        Menu.GetComponent<CanvasGroup>().alpha = 0;
        Menu.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _isOpen = false;
    }

    /// <summary>
    /// Метод для закрытия приложения
    /// </summary>
    public void Exit()
    {
        Debug.Log("Выход из приложения");
        Application.Quit();
    }
}

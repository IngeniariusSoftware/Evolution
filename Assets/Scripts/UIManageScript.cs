using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class UIManageScript : MonoBehaviour
{
    public InputField NameSaveGameField;

    public Toggle NormalViewToggle;

    public Toggle EnergyViewToggle;

    public Transform MainMenu;

    public Transform LoadGameMenu;

    public Transform SaveGameMenu;

    public Dropdown DropdownSaveGames;

    public Transform DeleteGameButton;

    public Transform LoadGameButton;

    public Transform SaveGameButtonYes;

    public Text[] StatisticsText;

    private static bool _showMainMenu;

    private static bool _showLoadMenu;

    private static bool _showSaveMenu;

    public static bool CurrentRenderingMode = RenderingScript.RenderingMode;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_showMainMenu)
            {
                _showMainMenu = false;
                MainMenu.GetComponent<CanvasGroup>().alpha = 0;
                MainMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                _showMainMenu = true;
                MainMenu.GetComponent<CanvasGroup>().alpha = 1;
                MainMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

            if (_showLoadMenu)
            {
                _showLoadMenu = false;
                LoadGameMenu.GetComponent<CanvasGroup>().alpha = 0;
                LoadGameMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }

            if (_showSaveMenu)
            {
                _showSaveMenu = false;
                SaveGameMenu.GetComponent<CanvasGroup>().alpha = 0;
                SaveGameMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void ClickContinue()
    {
        if (_showMainMenu)
        {
            _showMainMenu = false;
            MainMenu.GetComponent<CanvasGroup>().alpha = 0;
            MainMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void ClickLoadGameNo()
    {
        if (_showLoadMenu)
        {
            _showLoadMenu = false;
            LoadGameMenu.GetComponent<CanvasGroup>().alpha = 0;
            LoadGameMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        if (!_showMainMenu)
        {
            _showMainMenu = true;
            MainMenu.GetComponent<CanvasGroup>().alpha = 1;
            MainMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void ClickSaveGameNo()
    {
        if (_showSaveMenu)
        {
            _showSaveMenu = false;
            SaveGameMenu.GetComponent<CanvasGroup>().alpha = 0;
            SaveGameMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        if (!_showMainMenu)
        {
            _showMainMenu = true;
            MainMenu.GetComponent<CanvasGroup>().alpha = 1;
            MainMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void ClickDeleteGabe() //TODO Разработать логику
    {
        
    }

    public void CheckField()
    {
        if (NameSaveGameField.text != "")
        {
            SaveGameButtonYes.GetComponent<CanvasGroup>().alpha = 1;
            SaveGameButtonYes.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            SaveGameButtonYes.GetComponent<CanvasGroup>().alpha = 0.5f;
            SaveGameButtonYes.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void ClickLoadGameYes() //TODO Разработать логику
    {
        Debug.Log("Загрузка поколения");
    }

    public void ClickSaveGameYes() //TODO Разработать логику
    {

        Debug.Log("Сохранение поколения");
    }

    public void ClickNewGame() //TODO Разработать логику
    {
        Debug.Log("Новая игра");
    }

    public void ClickLoadGame()
    {
        ClickContinue();
        DropdownSaveGames.ClearOptions();
        var files = Directory.GetFiles(Data.SavePath, "*.txt");
        DropdownSaveGames.AddOptions(files.ToList());
        if (DropdownSaveGames.options.Count == 0)
        {
            List<string> emptySaves = new List<string> { "Сохранения отсутствуют" };
            DropdownSaveGames.AddOptions(emptySaves);
            DeleteGameButton.GetComponent<CanvasGroup>().alpha = 0.5f;
            DeleteGameButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            LoadGameButton.GetComponent<CanvasGroup>().alpha = 0.5f;
            LoadGameButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            DeleteGameButton.GetComponent<CanvasGroup>().alpha = 1;
            DeleteGameButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            LoadGameButton.GetComponent<CanvasGroup>().alpha = 1;
            LoadGameButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        if (!_showLoadMenu)
        {
            _showLoadMenu = true;
            LoadGameMenu.GetComponent<CanvasGroup>().alpha = 1;
            LoadGameMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void ClickSaveGame()
    {
        ClickContinue();
        if (!_showSaveMenu)
        {
            _showSaveMenu = true;
            SaveGameMenu.GetComponent<CanvasGroup>().alpha = 1;
            SaveGameMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

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

    public void Exit()
    {
        Debug.Log("Выход из приложения");
        Application.Quit();
    }
}

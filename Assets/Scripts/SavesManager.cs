using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;

/// <summary>
/// Класс, для управления сохранениями игры
/// </summary>
public class SavesManager : MonoBehaviour
{
    /// <summary>
    /// Путь для сохранения поколений жуков
    /// </summary>
    private static string _savePath = @"BugGenerations\";

    /// <summary>
    /// Расширение, в котором сохраняется файл с поколениями жуков
    /// </summary>
    private static string _format = ".json";

    /// <summary>
    /// Имя сохранения, которое необходимо загрузить
    /// </summary>
    public static string NameLoadGame;

    /// <summary>
    /// Переменная необходимая для ожидания момента для загрузки сохранения
    /// </summary>
    public static bool NeedLoadGame;
 
    /// <summary>
    /// Метод возвращает лист сохранений из папки
    /// </summary>
    public static List<string> GetSaveGames()
    {
        string[] files = Directory.GetFiles(_savePath, "*" + _format);
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace(_savePath, string.Empty);
            files[i] = files[i].Replace(_format, string.Empty);
        }

        return files.ToList();
    }

    /// <summary>
    /// Метод сохарняет файл с указанным именем
    /// </summary>
    public static void SaveGame(string nameSaveGame)
    {
        int number = 0;
        string nameGame = nameSaveGame;
        List<string> saveGames = GetSaveGames();
        while (saveGames.Contains(nameSaveGame))
        {
            number++;
            nameSaveGame = nameGame + " (" + number + ")";
        }

        using (var bugsFile = new FileStream(_savePath + nameSaveGame + ".json", FileMode.Create))
        {
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(bugsFile, Encoding.UTF8, true, true, "  "))
            {
                var serializer = new DataContractJsonSerializer(typeof(BugCollection));
                serializer.WriteObject(writer, ControlScript.bugs);
                writer.Flush();
            }
        }
    }

    /// <summary>
    /// Метод для загрузки сохранения
    /// </summary>
    public static void LoadGame()
    {
        if (NeedLoadGame)
        {
            FileStream bugsLoadFile = new FileStream(_savePath + NameLoadGame + _format, FileMode.Open);
            DataContractJsonSerializer jsonDeserializer = new DataContractJsonSerializer(typeof(BugCollection));
            ControlScript.bugs = (BugCollection)jsonDeserializer.ReadObject(bugsLoadFile);
            bugsLoadFile.Close();
            NeedLoadGame = false;
        }
        else
        {
            NeedLoadGame = true;
        }
    }

    /// <summary>
    /// Метод для удаления сохранения
    /// </summary>
    public static void DeleteGame(string nameSaveGame)
    {
        File.Delete(_savePath + nameSaveGame + _format);
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class DataHelper
{

    /// <summary>
    /// Creates a savegame and returns it.
    /// </summary>
    /// <returns></returns>
    private static Savegame GetSavegame()
    {
        NumberField numberField = NumberField.Instance;

        //Create number list
        List<SerializableNumberField> numberFields = NumberHelper.CreateSerNumberFieldList(ref numberField);

        //PlayerPrefs
        int oldSavegameTimestamp = PlayerPrefs.GetInt("SavegameTimestamp");
        string gameName = PlayerPrefs.GetString("SavegameName");
        int gameMode = PlayerPrefs.GetInt("SavegameMode");

        //Delete old savegame
        DeleteSavegame(oldSavegameTimestamp);

        //Returns a new savegame
        return new Savegame(numberFields,
            numberField.GetStrikedLines(), 
            gameName, 
            gameMode, 
            GameplayController.Instance.passedTime,
            numberField.GetBackLog(), 
            numberField.GetStrikedPairs(),
            numberField.GetUndoCount());
    }


    /// <summary>
    /// Saves the progress from the current game on the given savegame-index.
    /// </summary>
    /// <param name="_savegameIndex"></param>
    public static void SaveProgress()
    {
        Savegame savegame = GetSavegame();

        string jsonString = JsonUtility.ToJson(savegame);
        string dataPath = Path.Combine(Application.persistentDataPath, "Savegame_" + savegame.timestamp + ".txt");

        using (StreamWriter streamWriter = File.CreateText(dataPath))
        {
            streamWriter.Write(jsonString);
        }
    }


    /// <summary>
    /// Returns the loaded savegame from the given savegame-index.
    /// </summary>
    /// <param name="_savegameIndex"></param>
    /// <returns></returns>
    public static Savegame LoadProgress(int _savegameTimestamp)
    {
        Savegame loadedSavegame = null;
        string dataPath = Path.Combine(Application.persistentDataPath, "Savegame_" + _savegameTimestamp + ".txt");

        if (File.Exists(dataPath))
        {
            using (StreamReader streamReader = File.OpenText(dataPath))
            {
                string jsonString = streamReader.ReadToEnd();
                loadedSavegame = JsonUtility.FromJson<Savegame>(jsonString);
            }
        }
        return loadedSavegame;
    }


    /// <summary>
    /// Delete the savegame on the given savegame-index.
    /// </summary>
    /// <param name="_savegameTimestamp"></param>
    public static void DeleteSavegame(int _savegameTimestamp)
    {
        string dataPath = Path.Combine(Application.persistentDataPath, "Savegame_" + _savegameTimestamp + ".txt"); ;

        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
        }
    }


    /// <summary>
    /// Returns a list of existing savegame informations.
    /// </summary>
    /// <param name="_maxSavegames"></param>
    /// <returns></returns>
    public static List<SavegameInformation> GetSavegameInformations()
    {
        List<SavegameInformation> savegameInformations = new List<SavegameInformation>();
        List<Savegame> savegames = new List<Savegame>();

        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] fileInfo = info.GetFiles();

        foreach (FileInfo file in fileInfo)
        {
            using (StreamReader streamReader = file.OpenText())
            {
                string jsonString = streamReader.ReadToEnd();
                savegames.Add(JsonUtility.FromJson<Savegame>(jsonString));
            }
        }

        //Sorting
        savegames.Sort((y, x) => x.timestamp.CompareTo(y.timestamp));

        for (int i = 0; i < savegames.Count; i++)
        {
            Savegame s = savegames[i];
            savegameInformations.Add(new SavegameInformation(s.timestamp, s.gameName, s.gameMode));
        }

        return savegameInformations;
    }


    /// <summary>
    /// Deletes all savegames.
    /// </summary>
    public static void DeleteAllSavegames()
    {
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] fileInfo = info.GetFiles();

        foreach (FileInfo file in fileInfo)
        {
            file.Delete();
        }
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;


public static class DataHelper
{
    /// <summary>
    /// Saves the progress from the current game on the given savegame-index.
    /// </summary>
    /// <param name="_savegameIndex"></param>
    public static void SaveProgress(int _savegameIndex)
    {
        Savegame savegame = GameplayController.Instance.GetSavegame();
        savegame.index = _savegameIndex;

        string jsonString = JsonUtility.ToJson(savegame);
        string dataPath = Path.Combine(Application.persistentDataPath, "Savegame_" + _savegameIndex + ".txt");

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
    public static Savegame LoadProgress(int _savegameIndex)
    {
        Savegame loadedSavegame = null;

        string dataPath = Path.Combine(Application.persistentDataPath, "Savegame_" + _savegameIndex + ".txt");

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
    /// <param name="_savegameIndex"></param>
    public static void DeleteSavegame(int _savegameIndex)
    {
        string dataPath = Path.Combine(Application.persistentDataPath, "Savegame_" + _savegameIndex + ".txt"); ;

        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
        }
    }


    /// <summary>
    /// Looks for a savegame with a given index, if found, it returns true.
    /// </summary>
    /// <param name="_savegameIndex"></param>
    /// <returns></returns>
    public static bool IsSavegameAlive(int _savegameIndex)
    {
        bool found = false;

        string dataPath = Path.Combine(Application.persistentDataPath, "Savegame_" + _savegameIndex + ".txt"); ;

        if (File.Exists(dataPath))
        {
            found = true;
        }
        return found;
    }


    /// <summary>
    /// Returns a list of existing savegame indices.
    /// </summary>
    /// <param name="_maxSavegames"></param>
    /// <returns></returns>
    public static List<SavegameInformation> GetSavegameIndices(int _maxSavegames) 
    {
        List<SavegameInformation> savegamesIndices = new List<SavegameInformation>();
        List<Savegame> savegames = new List<Savegame>();

        for (int i = 0; i < _maxSavegames; i++)
        {
            string dataPath = Path.Combine(Application.persistentDataPath, "Savegame_" + i + ".txt");

            if (File.Exists(dataPath))
            {
                using (StreamReader streamReader = File.OpenText(dataPath))
                {
                    string jsonString = streamReader.ReadToEnd();
                    savegames.Add(JsonUtility.FromJson<Savegame>(jsonString));
                }
            }
        }

        //Sorting
        savegames.Sort((y, x) => DateTime.Compare(JsonUtility.FromJson<JsonDateTime>(x.jsonTimestamp), JsonUtility.FromJson<JsonDateTime>(y.jsonTimestamp)));

        for (int i = 0; i < savegames.Count; i++)
        {
            Savegame s = savegames[i];
            savegamesIndices.Add(new SavegameInformation(s.index, s.jsonTimestamp, SerializeSpriteHelper.DeserializeSprite(s.jsonSprite)));
        }

        return savegamesIndices;
    }


    /// <summary>
    /// Deletes all savegames.
    /// </summary>
    public static void DeleteAllSavegames(int _maxSavegames = 10)
    {
        for (int i = 0; i < _maxSavegames; i++)
        {
            DeleteSavegame(i);
        }
    }
}

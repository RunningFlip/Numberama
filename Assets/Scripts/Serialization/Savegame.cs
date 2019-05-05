using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public class Savegame
{
    //Timestamp
    public int timestamp;

    //Settings
    public string gameName;
    public int gameMode;

    //Timer
    public float passedTime;

    //Stats
    public int pairsFound;
    public int undoCount;

    //Numberfields
    public List<SerializableNumberField> serializedNumberFields;

    //Striked lines
    public List<int> strikedLines;

    //Backlog
    public SerializedBackLog serializedBackLog;

    //Screenshot
    public SerializedSprite jsonSprite;


    public Savegame(List<SerializableNumberField> _serializedNumberFields, List<int> _strikedLines, string _gameName, int _gameMode, float _passedTime, BackLog _backLog, int _pairsFound, int _undos)
    {
        //Delete old savegame
        DataHelper.DeleteSavegame(PlayerPrefs.GetInt("SavegameTimestamp"));

        //Timestamp
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        timestamp = (int)(DateTime.UtcNow - epochStart).TotalSeconds;

        //Settings
        gameName = _gameName;
        gameMode = _gameMode;

        //Timer
        passedTime = _passedTime;

        //Countings
        pairsFound = _pairsFound;
        undoCount = _undos;

        //Components
        strikedLines = new List<int>(_strikedLines);
        serializedNumberFields = _serializedNumberFields;
        serializedBackLog = new SerializedBackLog(_backLog);

        //Screenshot
        //TakeScreenshot();
    }


    /// <summary>
    /// Takes a screenshot and deserialize it.
    /// </summary>
    private void TakeScreenshot()
    {
        //Take screenshot
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        //Convert to sprite
        Sprite screenshot = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        //Clean up
        UnityEngine.Object.Destroy(texture);

        //Serialize
        jsonSprite = SerializeSpriteHelper.SerializeSprite(screenshot);
    }
}

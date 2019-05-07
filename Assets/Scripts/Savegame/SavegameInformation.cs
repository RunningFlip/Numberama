using UnityEngine;


public class SavegameInformation
{
    public string gameName = "None";
    public int gameMode = -1;

    public int timestamp;
    public Sprite screenshot; 


    public SavegameInformation(int _timestamp, string _gameName, int _gameMode, Sprite _screenshot = null)
    {
        gameName = _gameName;
        gameMode = _gameMode;

        timestamp = _timestamp;
        screenshot = _screenshot;
    }
}

using UnityEngine;


public class SavegameInformation
{
    public int index;
    public string jsonTimestamp;
    public Sprite screenshot; 


    public SavegameInformation(int _index, Sprite _screenshot = null)
    {
        index = _index;
        screenshot = _screenshot;
    }


    public SavegameInformation(int _index, string _jsonTimestamp, Sprite _screenshot = null)
    {
        index = _index;
        jsonTimestamp = _jsonTimestamp;
        screenshot = _screenshot;
    }
}

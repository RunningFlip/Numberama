using System;
using UnityEngine;
using UnityEngine.UI;


public class SavegameButton : MonoBehaviour
{
    [Header("UI Elements")]
    public Button button;
    public Text text;
    public Image screenshotImage;
    [Space]
    public GameObject editSymbol;


    //Flag
    private bool inEditing;

    //Savegameindex
    private SavegameInformation savegameIndex;


    /// <summary>
    /// Setups the button.
    /// </summary>
    /// <param name="_jsonTimestamp"></param>
    public void ButtonSetup(string _jsonTimestamp)
    {
        SetTimestampLabel(_jsonTimestamp);
        SetScreenshotImage();
    }


    /// <summary>
    /// Converts the timestamp in a readable string.
    /// </summary>
    /// <param name="_jsonTimestamp"></param>
    private void SetTimestampLabel(string _jsonTimestamp)
    {
        DateTime stamp = JsonUtility.FromJson<JsonDateTime>(_jsonTimestamp);
        text.text = stamp.ToString();
    }


    /// <summary>
    /// Sets the screenshot picture in the background of the button image.
    /// </summary>
    private void SetScreenshotImage()
    {
        if (savegameIndex.screenshot != null) 
        {
            screenshotImage.sprite = savegameIndex.screenshot;
        }
    }


    /// <summary>
    /// Sets the savgameindex object.
    /// </summary>
    /// <param name="_savegameIndex"></param>
    public void SetSavegameIndex(SavegameInformation _savegameIndex)
    {
        savegameIndex = _savegameIndex;
    }


    /// <summary>
    /// Returns the savgameindex object.
    /// </summary>
    /// <returns></returns>
    public SavegameInformation GetSavegameIndex()
    {
        return savegameIndex;
    }


    /// <summary>
    /// Toggles the editing mode.
    /// </summary>
    public void ToggleEditMode()
    {
        inEditing = !inEditing;
        editSymbol.SetActive(inEditing);
    }
}

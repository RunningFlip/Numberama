using UnityEngine;
using UnityEngine.UI;


public class SavegameButton : MonoBehaviour
{
    [Header("UI Elements")]
    public Button button;
    public Text gameNameLabel;
    public Text gameModeLabel;
    public Image screenshotImage;
    [Space]
    public GameObject editSymbol;


    //Flag
    private bool inEditing;

    //Savegameindex
    private SavegameInformation savegameInformation;


    /// <summary>
    /// Setups the button.
    /// </summary>
    /// <param name="_jsonTimestamp"></param>
    public void ButtonSetup(string _gameName, int _gameMode)
    {
        SetGameName(_gameName, _gameMode);
        SetScreenshotImage();
    }


    /// <summary>
    /// Converts the timestamp in a readable string.
    /// </summary>
    /// <param name="_jsonTimestamp"></param>
    private void SetGameName(string _gameName, int _gameMode)
    {
        gameNameLabel.text = _gameName;
        gameModeLabel.text = _gameMode.ToString();
    }


    /// <summary>
    /// Sets the screenshot picture in the background of the button image.
    /// </summary>
    private void SetScreenshotImage()
    {
        if (savegameInformation.screenshot != null) 
        {
            screenshotImage.sprite = savegameInformation.screenshot;
        }
    }


    /// <summary>
    /// Sets the savgameindex object.
    /// </summary>
    /// <param name="_savegameInformation"></param>
    public void SetSavegameInformation(SavegameInformation _savegameInformation)
    {
        savegameInformation = _savegameInformation;
    }


    /// <summary>
    /// Returns the savgameindex object.
    /// </summary>
    /// <returns></returns>
    public SavegameInformation GetSavegameInformation()
    {
        return savegameInformation;
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

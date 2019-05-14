using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SavegameMenuController : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasObject;

    [Header("Buttons")]
    public Button loadMainMenuButton;
    public Button editSavegamesButton;

    [Header("Savegame Parent")]
    public GameObject savegameButtonPrefab;
    public Transform savegameParent;


    //Flag
    private bool inEditing;

    //Savegames
    private List<SavegameInformation> existingSavegameIndices;
    private List<SavegameButton> savegameButtons;


    private void Start()
    {
        //Buttons
        loadMainMenuButton.onClick.AddListener(     delegate { StartCoroutine(LoadingHelper.LoadSceneAsync(canvasObject, "MainmenuScene")); });
        editSavegamesButton.onClick.AddListener(    delegate { ToggleEditing();                                             });

        //Savegames
        savegameButtons = new List<SavegameButton>();
        existingSavegameIndices = DataHelper.GetSavegameInformations();
        SetupButtons();
    }


    /// <summary>
    /// Setups every button in menu, if it is needed.
    /// </summary>
    /// <param name="_status"></param>
    private void SetupButtons()
    {
        for (int i = 0; i < existingSavegameIndices.Count; i++)
        {
            AddSavegameButton(existingSavegameIndices[i]);
        }
    }


    /// <summary>
    /// Loads the game scene with/without savegame.
    /// </summary>
    /// <param name="_savegame"></param>
    private IEnumerator<object> LoadGame(SavegameInformation _savegameInformation)
    {
        //Creates a loadingscreen object
        LoadingHelper.ShowLoadingScreen(canvasObject.transform);

        //Setting prefs
        PlayerPrefs.SetInt("SavegameTimestamp", _savegameInformation.timestamp);
        PlayerPrefs.SetString("SavegameName", _savegameInformation.gameName);
        PlayerPrefs.SetInt("SavegameMode", _savegameInformation.gameMode);
        PlayerPrefs.Save();

        //Loads the game
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainGameScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Destroys the loadingscreen object
        LoadingHelper.RemoveLoadingScreen();
    }


    /// <summary>
    /// Adds a savegamebutton on a given index.
    /// </summary>
    /// <param name="_index"></param>
    public void AddSavegameButton(SavegameInformation _savegameInformation)
    {
        GameObject buttonObject = Instantiate(savegameButtonPrefab, Vector3.zero, Quaternion.identity, savegameParent);

        SavegameButton savegameButton = buttonObject.GetComponent<SavegameButton>();       
        savegameButton.SetSavegameInformation(_savegameInformation); 
        savegameButton.ButtonSetup(_savegameInformation.gameName, _savegameInformation.gameMode); 

        savegameButton.button.onClick.AddListener(delegate { StartCoroutine(LoadGame(_savegameInformation)); });

        savegameButtons.Add(savegameButton);
    }


    /// <summary>
    /// Removes a savegamebutton on a given index.
    /// </summary>
    /// <param name="_timestamp"></param>
    public void TryToDeleteSavegame(int _timestamp)
    {
        for (int i = 0; i < savegameButtons.Count; i++)
        {
            if (savegameButtons[i].GetSavegameInformation().timestamp == _timestamp)
            {
                SavegameButton sgButton = savegameButtons[i];
                DataHelper.DeleteSavegame(sgButton.GetSavegameInformation().timestamp);

                existingSavegameIndices.Remove(sgButton.GetSavegameInformation());
                savegameButtons.Remove(sgButton);  

                Destroy(sgButton.gameObject);

                if (savegameButtons.Count == 0)
                {
                    ToggleEditing();
                }
                break;
            }
        }
    }


    /// <summary>
    /// Toggles the editing mode.
    /// </summary>
    private void ToggleEditing()
    {
        inEditing = !inEditing;

        if (inEditing)
        {
            for (int i = 0; i < savegameButtons.Count; i++)
            {
                int timestamp = savegameButtons[i].GetSavegameInformation().timestamp;

                SavegameButton savegameButton = savegameButtons[i];
                savegameButton.button.onClick.RemoveAllListeners();
                savegameButton.button.onClick.AddListener(delegate { TryToDeleteSavegame(timestamp); });

                savegameButton.ToggleEditMode();
            }
        }
        else
        {
            for (int i = 0; i < savegameButtons.Count; i++)
            {
                SavegameInformation savegameInformation = savegameButtons[i].GetSavegameInformation();

                SavegameButton savegameButton = savegameButtons[i];
                savegameButton.button.onClick.RemoveAllListeners();
                savegameButton.button.onClick.AddListener(delegate { StartCoroutine(LoadGame(savegameInformation)); });

                savegameButton.ToggleEditMode();
            }
        }
    }
}

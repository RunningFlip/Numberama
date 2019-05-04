using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SavegameMenuController : MonoBehaviour
{
    [Header("Buttons")]
    public Button loadMainMenuButton;
    public Button newGameButton;
    public Button editSavegamesButton;

    [Header("Savegame Parent")]
    public GameObject savegameButtonPrefab;
    public Transform savegameParent;


    //Flag
    private bool inEditing;

    //Savegames
    private int maxSavegames = 10;
    private List<SavegameInformation> existingSavegameIndices;
    private List<SavegameButton> savegameButtons;


    private void Start()
    {
        //Buttons
        loadMainMenuButton.onClick.AddListener(     delegate { SceneManager.LoadScene("MainMenuScene"); });
        newGameButton.onClick.AddListener(          delegate { LoadGame();                              });
        editSavegamesButton.onClick.AddListener(    delegate { ToggleEditing();                         });

        //Savegames
        savegameButtons = new List<SavegameButton>();
        existingSavegameIndices = DataHelper.GetSavegameIndices(maxSavegames);
        SetupButtons();
    }


    /// <summary>
    /// Setups every button in menu, if it is needed.
    /// </summary>
    /// <param name="_status"></param>
    private void SetupButtons()
    {
        if (existingSavegameIndices.Count == maxSavegames)
        {
            newGameButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < existingSavegameIndices.Count; i++)
        {
            AddSavegameButton(existingSavegameIndices[i]);
        }
    }


    /// <summary>
    /// Loads the game scene with/without savegame.
    /// </summary>
    /// <param name="_savegame"></param>
    private void LoadGame(int _savegame = -1)
    {
        int savegame = _savegame;

        if (_savegame == -1)
        {
            List<int> indices = new List<int>();

            for (int i = 0; i < existingSavegameIndices.Count; i++)
            {
                indices.Add(existingSavegameIndices[i].index);
            }           

            for (int i = 0; i < maxSavegames; i++)
            {
                if (!indices.Contains(i))
                {
                    existingSavegameIndices.Add(new SavegameInformation(i));
                    savegame = i;
                    break;
                }
            }
        }
        PlayerPrefs.SetInt("SavegameIndex", savegame);
        SceneManager.LoadScene("MainGameScene");
    }


    /// <summary>
    /// Adds a savegamebutton on a given index.
    /// </summary>
    /// <param name="_index"></param>
    public void AddSavegameButton(SavegameInformation _savegameIndex)
    {
        GameObject buttonObject = Instantiate(savegameButtonPrefab, Vector3.zero, Quaternion.identity, savegameParent);

        SavegameButton savegameButton = buttonObject.GetComponent<SavegameButton>();       
        savegameButton.SetSavegameIndex(_savegameIndex);            //error might be here?! switch with next line!
        savegameButton.ButtonSetup(_savegameIndex.jsonTimestamp);   // <---

        savegameButton.button.onClick.AddListener(delegate { LoadGame(_savegameIndex.index); });

        savegameButtons.Add(savegameButton);
    }


    /// <summary>
    /// Removes a savegamebutton on a given index.
    /// </summary>
    /// <param name="_index"></param>
    public void TryToDeleteSavegame(int _index)
    {
        for (int i = 0; i < savegameButtons.Count; i++)
        {
            if (savegameButtons[i].GetSavegameIndex().index == _index)
            {
                SavegameButton sgButton = savegameButtons[i];
                DataHelper.DeleteSavegame(sgButton.GetSavegameIndex().index);

                existingSavegameIndices.Remove(sgButton.GetSavegameIndex());
                savegameButtons.Remove(sgButton);  

                Destroy(sgButton.gameObject);

                if (savegameButtons.Count < maxSavegames)
                {
                    newGameButton.gameObject.SetActive(true);
                }
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
        newGameButton.interactable = !inEditing;

        if (inEditing)
        {
            for (int i = 0; i < savegameButtons.Count; i++)
            {
                int index = savegameButtons[i].GetSavegameIndex().index;

                SavegameButton savegameButton = savegameButtons[i];
                savegameButton.button.onClick.RemoveAllListeners();
                savegameButton.button.onClick.AddListener(delegate { TryToDeleteSavegame(index); });

                savegameButton.ToggleEditMode();
            }
        }
        else
        {
            for (int i = 0; i < savegameButtons.Count; i++)
            {
                int index = savegameButtons[i].GetSavegameIndex().index;

                SavegameButton savegameButton = savegameButtons[i];
                savegameButton.button.onClick.RemoveAllListeners();
                savegameButton.button.onClick.AddListener(delegate { LoadGame(index); });

                savegameButton.ToggleEditMode();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [Header("Menu Buttons")]
    public Button showPopupButton;
    public Button loadSavegameMenuButton;
    public Button loadSettingsMenuButton;
    public Button loadHelpMenuButton;
    public Button loadTrphiesMenuButton;

    [Header("Panel")]
    public GameObject mainMenuPanel;
    public GameObject popupPanel;

    [Header("Popup")]
    public Button startNewGameButton;    
    public Button closePopupButton;
    [Space]
    public InputField gameNameInputField;
    public Dropdown gameModeDropDown;


    //Flag
    private bool popupActive;


    private void Start()
    {
        //Buttons
        showPopupButton.onClick.AddListener(            delegate { TogglePopup();                               });
        closePopupButton.onClick.AddListener(           delegate { TogglePopup();                               });
        startNewGameButton.onClick.AddListener(         delegate { StartNewGame();                               });

        loadSavegameMenuButton.onClick.AddListener(     delegate { SceneManager.LoadScene("SavegameMenuScene"); });
        loadSettingsMenuButton.onClick.AddListener(     delegate { SceneManager.LoadScene("SettingsScene");     });
        loadHelpMenuButton.onClick.AddListener(         delegate { SceneManager.LoadScene("HelpScene");         });
        loadTrphiesMenuButton.onClick.AddListener(      delegate { SceneManager.LoadScene("TrophiesScene");     });

        //Popup
        gameNameInputField.onValueChanged.AddListener(  delegate { ChecksInputField();                          });

        gameModeDropDown.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData("Normal"), new Dropdown.OptionData("Hardmode") });
        startNewGameButton.interactable = false;


        //DEBUG - resets the savegames
        //DataHelper.DeleteAllSavegames();
        //PlayerPrefs.DeleteAll();
    }


    /// <summary>
    /// Toggles the popup.
    /// </summary>
    private void TogglePopup()
    {
        popupActive = !popupActive;

        mainMenuPanel.SetActive(!popupActive);
        popupPanel.SetActive(popupActive);
    }


    /// <summary>
    /// Starts a new game.
    /// </summary>
    private void StartNewGame()
    {
        //PlayerPrefs
        PlayerPrefs.SetInt("SavegameTimestamp", -1);
        PlayerPrefs.SetString("SavegameName", gameNameInputField.text);
        PlayerPrefs.SetInt("SavegameMode", gameModeDropDown.value);
        PlayerPrefs.Save();

        //Scene
        SceneManager.LoadScene("MainGameScene");
    }


    /// <summary>
    /// Checks if the inputfield contains any text and sets the button interactability.
    /// </summary>
    private void ChecksInputField()
    {
        startNewGameButton.interactable = gameNameInputField.text != "";
    }
}

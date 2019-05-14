using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasObject;

    [Header("Panel")]
    public GameObject mainMenuPanel;
    public GameObject tutorialPanel;
    public GameObject achievmentsPanel;
    public GameObject settingsPanel;
    public GameObject popupPanel;

    [Header("Menu Buttons")]
    public Button showPopupButton;
    public Button loadSavegameMenuButton;
    public Button showSettingsPanelButton;
    public Button showTutorialPanelButton;
    public Button showAchievmentsPanelButton;
    [Space]
    public Button resetButton;

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
        MenuSetup();
    }


    /// <summary>
    /// Setups the menu listeners.
    /// </summary>
    public void MenuSetup()
    {
        //Buttons
        showPopupButton.onClick.AddListener(            delegate { TogglePopup();   });
        closePopupButton.onClick.AddListener(           delegate { TogglePopup();   });
        startNewGameButton.onClick.AddListener(         delegate { StartNewGame();  });

        loadSavegameMenuButton.onClick.AddListener(         delegate { StartCoroutine(LoadingHelper.LoadSceneAsync(canvasObject, "SavegameMenuScene")); });

        showSettingsPanelButton.onClick.AddListener(        delegate { GoToMenu(settingsPanel);     });
        showTutorialPanelButton.onClick.AddListener(        delegate { GoToMenu(tutorialPanel);     });
        showAchievmentsPanelButton.onClick.AddListener(     delegate { GoToMenu(achievmentsPanel);  });

        //Popup
        gameNameInputField.onValueChanged.AddListener(  delegate { ChecksInputField(); });

        gameModeDropDown.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData("Normal"), new Dropdown.OptionData("Schwer"), new Dropdown.OptionData("Zufall") });
        startNewGameButton.interactable = false;

        //Audio
        //MusicController.Instance.SetMusicType(MusicType.Menu);

        //DEBUG - resets the savegames
        resetButton.onClick.AddListener(delegate { DebugReset(); });
        //DebugReset();
    }


    /// <summary>
    /// Go to a given menu panel and disabled the main menu.
    /// </summary>
    /// <param name="_menuPanel"></param>
    private void GoToMenu(GameObject _menuPanel)
    {
        mainMenuPanel.SetActive(false);
        _menuPanel.SetActive(true);
    }


    /// <summary>
    /// Just DEBUG
    /// </summary>
    private void DebugReset()
    {
        DataHelper.DeleteAllSavegames();
        PlayerPrefs.DeleteAll();

        SettingsController.PlayerPrefSetup();
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
        StartCoroutine(LoadingHelper.LoadSceneAsync(canvasObject, "MainGameScene"));
    }


    /// <summary>
    /// Checks if the inputfield contains any text and sets the button interactability.
    /// </summary>
    private void ChecksInputField()
    {
        startNewGameButton.interactable = gameNameInputField.text != "";
    }
}

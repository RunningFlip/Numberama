using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [Header("Scene loader button")]
    public Button showPopupButton;
    public Button closePopupButton;
    public Button startNewGameButton;
    [Space]
    public Button loadSavegameMenuButton;
    public Button loadSettingsMenuButton;
    public Button loadHelpMenuButton;
    public Button loadStatisticsMenuButton;

    [Header("Panel")]
    public GameObject mainMenuPanel;
    public GameObject popupPanel;


    //Flag
    private bool popupActive;


    private void Start()
    {
        //Buttons
        showPopupButton.onClick.AddListener(            delegate { TogglePopup();                               });
        closePopupButton.onClick.AddListener(           delegate { TogglePopup();                               });
        startNewGameButton.onClick.AddListener(         delegate { LoadNewGame();                               });

        loadSavegameMenuButton.onClick.AddListener(     delegate { SceneManager.LoadScene("SavegameMenuScene"); });
        loadSettingsMenuButton.onClick.AddListener(     delegate { SceneManager.LoadScene("SettingsScene");     });
        loadHelpMenuButton.onClick.AddListener(         delegate { SceneManager.LoadScene("HelpScene");         });
        loadStatisticsMenuButton.onClick.AddListener(   delegate { SceneManager.LoadScene("StatisticsScene");   });

        //DEBUG - resets the savegames
        //DataHelper.DeleteAllSavegames();
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
    private void LoadNewGame()
    {
        //TODO

        //PlayerPrefs.SetInt("SavegameIndex", index);
        SceneManager.LoadScene("MainGameScene");
    }
}

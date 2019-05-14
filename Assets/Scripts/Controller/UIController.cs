using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasObject;

    [Header("Layout")]
    public GridLayoutGroup gridLayoutGroup;

    [Header("Buttons")]
    public Button menuButton;
    public Button moreFieldsButton;
    public Button undoStrikeButton;
    public Button hintButton;

    [Header("Labels")]
    public Text timerLabel;
    public Text pairValueText;
    public Text undoCountText;


    private void Start()
    {
        //Layout
        gridLayoutGroup.constraintCount = ParameterManager.Instance.GameParameter.maxLineLength;

        //Listeners
        menuButton.onClick.AddListener(         delegate { BackToGameMenu();                                    });
        moreFieldsButton.onClick.AddListener(   delegate { GameplayController.Instance.TrySpawnMoreNumbers();   });
        undoStrikeButton.onClick.AddListener(   delegate { GameplayController.Instance.UndoLastAction();        });
        hintButton.onClick.AddListener(         delegate { GameplayController.Instance.ShowNextHint();          });

        //Settings
        ApplySettings();
    }


    private void Update()
    {
        UpdateTimer();
    }


    /// <summary>
    /// Applies the settings to the game.
    /// </summary>
    private void ApplySettings()
    {
        int hintsActivated = PlayerPrefs.GetInt("hintsEnabled");
        int backtrackingEnabled = PlayerPrefs.GetInt("backtrackingEnabled");

        if (hintsActivated == 0)
        {
            hintButton.interactable = false;
            hintButton.gameObject.SetActive(false);
        }

        if (backtrackingEnabled == 0)
        {
            undoStrikeButton.interactable = false;
            undoStrikeButton.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Loads the mainMenu.
    /// </summary>
    private async void BackToGameMenu()
    {
        //Creates a loadingscreen object
        LoadingHelper.ShowLoadingScreen(canvasObject.transform);

        //Remove listener from button
        menuButton.onClick.RemoveListener(delegate { BackToGameMenu(); });

        //Check if the game is finished
        if (NumberField.Instance.IsFinished())
        {
            //Delete old savegame
            DataHelper.DeleteSavegame(PlayerPrefs.GetInt("SavegameTimestamp"));
        }
        else
        {
            //Save game
            if (NumberField.Instance.IsDirty())
            {
                await DataHelper.SaveProgress();
            }
        }

        //Load game menu
        SceneManager.LoadSceneAsync("MainMenuScene");   
    }


    /// <summary>
    /// Saves the current progress of the game.
    /// </summary>
    private async void SaveProgress()
    {
        Savegame savegame = null;

        string jsonString = JsonUtility.ToJson(savegame);
        string dataPath = Path.Combine(Application.persistentDataPath, "savegame_" + savegame.timestamp + ".txt");

        using (StreamWriter streamWriter = File.CreateText(dataPath))
        {
            await streamWriter.WriteAsync(jsonString);
            streamWriter.Close();
        }
    }


    /// <summary>
    /// Sets the striked pairs count to the label in the UI.
    /// </summary>
    /// <param name="_pairCount"></param>
    public void SetStrikedPairsCount(int _pairCount)
    {
        if (pairValueText != null)
        {
            pairValueText.text = _pairCount.ToString();
        }
    }


    /// <summary>
    /// Sets the undo count to the label in the UI.
    /// </summary>
    /// <param name="_pairCount"></param>
    public void SetUndoCount(int _undoCount)
    {
        if (undoCountText != null)
        {
            undoCountText.text = _undoCount.ToString();
        }
    }


    /// <summary>
    /// Starts the timer and disblays it in the UI.
    /// </summary>
    private void UpdateTimer()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(GameplayController.Instance.passedTime);

        timerLabel.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}

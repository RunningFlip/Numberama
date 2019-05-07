using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
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
        menuButton.onClick.AddListener(                 delegate { StartCoroutine(BackToGameMenu());                    });
        moreFieldsButton.onClick.AddListener(           delegate { GameplayController.Instance.TrySpawnMoreNumbers();   });
        undoStrikeButton.onClick.AddListener(           delegate { GameplayController.Instance.UndoLastAction();        });
        hintButton.onClick.AddListener(                 delegate { GameplayController.Instance.ShowNextHint();          });

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
    private IEnumerator BackToGameMenu()
    {
        //Remove listener from button
        menuButton.onClick.RemoveListener(delegate { StartCoroutine(BackToGameMenu()); });

        //Save game
        if (NumberField.Instance.IsDirty())
        {
            yield return new WaitForEndOfFrame();
            DataHelper.SaveProgress();
        }

        //Load game menu
        SceneManager.LoadScene("SavegameMenuScene");   
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

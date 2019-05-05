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
    public Button moreFieldsButton;
    public Button undoStrikeButton;
    public Button menuButton;

    [Header("Labels")]
    public Text timerLabel;
    public Text pairValueText;
    public Text undoCountText;


    private void Start()
    {
        //Layout
        gridLayoutGroup.constraintCount = GameplayController.Instance.GameParameter.maxLineLength;

        //Listeners
        moreFieldsButton.onClick.AddListener(           delegate { GameplayController.Instance.TrySpawnMoreNumbers();   });
        undoStrikeButton.onClick.AddListener(           delegate { GameplayController.Instance.UndoLastAction();        });
        menuButton.onClick.AddListener(                 delegate { StartCoroutine(BackToGameMenu());                    });
    }


    private void Update()
    {
        UpdateTimer();
    }


    /// <summary>
    /// Loads the mainMenu.
    /// </summary>
    private IEnumerator BackToGameMenu()
    {
        //Remove listener from button
        menuButton.onClick.RemoveListener(delegate { StartCoroutine(BackToGameMenu()); });

        //Save game
        if (GameplayController.Instance.IsDirty())
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

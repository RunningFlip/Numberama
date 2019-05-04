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
    public Button redoStrikeButton;
    public Button deleteStrikedLinesButton;
    public Button menuButton;

    [Header("Labels")]
    public Text pairValueText;
    public Text undoCountText;


    private void Start()
    {
        //Layout
        gridLayoutGroup.constraintCount = GameplayController.Instance.GameParameter.maxLineLength;

        //Listeners
        moreFieldsButton.onClick.AddListener(           delegate { GameplayController.Instance.TrySpawnMoreNumbers();   });
        undoStrikeButton.onClick.AddListener(           delegate { GameplayController.Instance.UndoLastAction();        });
        //redoStrikeButton.onClick.AddListener(           delegate { GameplayController.Instance.RedoLastAction();        });
        menuButton.onClick.AddListener(                 delegate { StartCoroutine(BackToGameMenu());                    });

        //Optional UI
        if (PlayerPrefs.GetInt("autoLineDeletingEnabled") == 0)
        {
            ActivateManuallyLineDeleting();
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
        int index = PlayerPrefs.GetInt("SavegameIndex");
        if (GameplayController.Instance.IsDirty() && index > -1)
        {
            yield return new WaitForEndOfFrame();
            DataHelper.SaveProgress(index);
        }

        //Load game menu
        SceneManager.LoadScene("SavegameMenuScene");   
    }


    /// <summary>
    /// Activates a button to delete striked lines manually.
    /// </summary>
    private void ActivateManuallyLineDeleting()
    {
        deleteStrikedLinesButton.gameObject.SetActive(true);

        deleteStrikedLinesButton.onClick.AddListener(delegate
        {
            GameplayController.Instance.DeleteAllStrikedLinesManually();
        });
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
}

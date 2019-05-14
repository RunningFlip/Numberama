using UnityEngine;
using UnityEngine.UI;


public class TutorialController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject mainMenuPanel;
    public GameObject tutorialPanel;

    [Header("Button")]
    public Button backToMainMenuButton;


    private void Start()
    {
        //Button
        backToMainMenuButton.onClick.AddListener(delegate { GoBackToMainMenu(); });
    }

    /// <summary>
    /// Toggles back to the main menu.
    /// </summary>
    private void GoBackToMainMenu()
    {
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}

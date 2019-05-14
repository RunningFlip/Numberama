using UnityEngine;
using UnityEngine.UI;


public class AchievmentsController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject mainMenuPanel;
    public GameObject achievmentsMenuPanel;

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
        achievmentsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}

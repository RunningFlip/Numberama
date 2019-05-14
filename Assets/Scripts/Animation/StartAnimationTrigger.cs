using UnityEngine;


public class StartAnimationTrigger : MonoBehaviour
{
    [Header("Panels")]
    public GameObject animationPanel;
    public GameObject mainMenuPanel;
    

    /// <summary>
    /// Ends the animation and shows the original main menu.
    /// </summary>
    public void SwitchToMainMenu()
    {
        animationPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}

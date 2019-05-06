using UnityEngine;

public class StartAnimationTrigger : MonoBehaviour
{
    public MainMenuController menuController;

    
    public void StartAnimation()
    {
        menuController.MenuSetup();
    }
}

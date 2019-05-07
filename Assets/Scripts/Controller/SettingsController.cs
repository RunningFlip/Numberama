using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingsController : MonoBehaviour
{
    [Header("Button")]
    public Button loadMainMenuButton;

    [Header("Toggle")]
    public Toggle soundEffectsToggle;
    public Toggle hintsToggle;
    public Toggle backtrackToggle;
    [Space]
    public Toggle darkModeToggle;


    private void Start()
    {
        //Button
        loadMainMenuButton.onClick.AddListener(delegate { SceneManager.LoadScene("MainMenuScene"); });

        //Toggle
        soundEffectsToggle.onValueChanged.AddListener(      delegate { ToggleSoundEffects();        });
        hintsToggle.onValueChanged.AddListener(             delegate { ToggleHints();               });
        backtrackToggle.onValueChanged.AddListener(         delegate { Togglebacktracking();        });
        darkModeToggle.onValueChanged.AddListener(          delegate { ToggleDarkMode();            });

        //Settings setup
        SettingsSetup();
    }


    public static void PlayerPrefSetup()
    {
        int init = PlayerPrefs.GetInt("playerPrefsInit");

        if (init == 0)
        {
            PlayerPrefs.SetInt("playerPrefsInit", 1);

            PlayerPrefs.SetInt("soundEffectsEnabled", 1);
            PlayerPrefs.SetInt("hintsEnabled", 1);
            PlayerPrefs.SetInt("backtrackingEnabled", 1);
            PlayerPrefs.SetInt("darkmodeEnabled", 1);

            PlayerPrefs.Save();
        }
    }


    private void SettingsSetup()
    {
        //Toggle UI
        soundEffectsToggle.isOn         = PlayerPrefs.GetInt("soundEffectsEnabled") == 1;
        hintsToggle.isOn                = PlayerPrefs.GetInt("hintsEnabled") == 1;
        backtrackToggle.isOn            = PlayerPrefs.GetInt("backtrackingEnabled") == 1;
        darkModeToggle.isOn             = PlayerPrefs.GetInt("darkmodeEnabled") == 1;
    }


    /// <summary>
    /// Enables or disables the soundeffects.
    /// </summary>
    private void ToggleSoundEffects()
    {
        PlayerPrefs.SetInt("soundEffectsEnabled", soundEffectsToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// Enables or disables the hints in game.
    /// </summary>
    private void ToggleHints()
    {
        PlayerPrefs.SetInt("hintsEnabled", hintsToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// Enables or disables backtracking in game.
    /// </summary>
    private void Togglebacktracking()
    {
        PlayerPrefs.SetInt("backtrackingEnabled", backtrackToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// Enables or disables the darkmode theme.
    /// </summary>
    private void ToggleDarkMode()
    {
        PlayerPrefs.SetInt("darkmodeEnabled", darkModeToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}

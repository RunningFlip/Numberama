using UnityEngine;


public class GameplayController : MonoBehaviour
{
    //Singelton
    public static GameplayController Instance;


    [Header("Game Parameter")]
    public GameParameter GameParameter;

    [Header("UI Controller")]
    public UIController uiController;

    [Header("Field Spawn")]
    public Transform spawnParent;


    //Numberfield
    private NumberField numberField;

    //Time
    public float passedTime;


    private void Awake()
    {
        //Singelton init
        if (!Instance)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        //PlayerPrefs
        int gameMode = PlayerPrefs.GetInt("SavegameMode");
        int savegameTimestamp = PlayerPrefs.GetInt("SavegameTimestamp");

        //PatternConfig
        NumberPatternConfig patternConfig = gameMode == 0
            ? GameParameter.defaultNumberPatternConfig
            : GameParameter.hardNumberPatternConfig;

        //Number Field
        if (savegameTimestamp != -1)
        {   
            //Creates a numberfield with a given savegame
            Savegame savegame = DataHelper.LoadProgress(savegameTimestamp);
            numberField = new NumberField(spawnParent, patternConfig, savegame);
        }
        else
        {
            //Creates a numberfield with non savegame
            numberField = new NumberField(spawnParent, patternConfig, null);
        }
    }


    private void LateUpdate()
    {
        passedTime += Time.deltaTime;
    }


    /// <summary>
    /// Spawns the left fields as an extension.
    /// </summary>
    public void TrySpawnMoreNumbers()
    {
        numberField.TrySpawnMoreNumbers();
    }


    /// <summary>
    /// Undos the last action in the backlog.
    /// </summary>
    public void UndoLastAction()
    {
        numberField.UndoLastAction();
    }


    /// <summary>
    /// Shows the next possible combination and highlights both fields.
    /// </summary>
    public void ShowNextHint()
    {
        numberField.ShowNextHint();
    }
}

using UnityEngine;


public class GameplayController : MonoBehaviour
{
    //Singelton
    public static GameplayController Instance;


    [Header("UI Controller")]
    public UIController uiController;

    [Header("Field Spawn")]
    public Transform spawnParent;


    //Numberfield
    private NumberField numberField;

    //Time
    public float passedTime;

    //Audio
    [HideInInspector] public AudioClipObject selectionAudioClipObject;
    [HideInInspector] public AudioClipObject deselectionAudioClipObject;
    [HideInInspector] public AudioClipObject strikeFieldAudioClipObject;
    [HideInInspector] public AudioClipObject strikeLineAudioClipObject;
    [HideInInspector] public AudioClipObject addNumbersAudioClipObject;
    [HideInInspector] public AudioClipObject hintAudioClipObject;
    [HideInInspector] public AudioClipObject undoAudioClipObject;


    private void Awake()
    {
        //Singelton init
        if (Instance == null)
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
            ? ParameterManager.Instance.GameParameter.defaultNumberPatternConfig
            : ParameterManager.Instance.GameParameter.hardNumberPatternConfig;

        //Number Field
        if (savegameTimestamp != -1)
        {   
            //Creates a numberfield with a given savegame
            Savegame savegame = DataHelper.LoadProgress(savegameTimestamp);
            passedTime = savegame.passedTime;
            numberField = new NumberField(spawnParent, patternConfig, savegame);
        }
        else
        {
            //Creates a numberfield with non savegame
            numberField = new NumberField(spawnParent, patternConfig, null);
        }

        //Audio
        MusicController.Instance.SetMusicType(MusicType.InGame);
        AudioSetup();
    }


    private void LateUpdate()
    {
        passedTime += Time.deltaTime;
    }


    /// <summary>
    /// Setups the audio that can be played.
    /// </summary>
    private void AudioSetup()
    {
        if (PlayerPrefs.GetInt("soundEffectsEnabled") == 1)
        {
            selectionAudioClipObject    = new AudioClipObject(ParameterManager.Instance.AudioParameter.selectionClip);
            deselectionAudioClipObject  = new AudioClipObject(ParameterManager.Instance.AudioParameter.deselectionClip);
            strikeFieldAudioClipObject  = new AudioClipObject(ParameterManager.Instance.AudioParameter.strikeFieldClip);
            strikeLineAudioClipObject   = new AudioClipObject(ParameterManager.Instance.AudioParameter.strikeLineClip);
            addNumbersAudioClipObject   = new AudioClipObject(ParameterManager.Instance.AudioParameter.addNumbersClip);
            hintAudioClipObject         = new AudioClipObject(ParameterManager.Instance.AudioParameter.hintClip);
            undoAudioClipObject         = new AudioClipObject(ParameterManager.Instance.AudioParameter.undoClip);
        }
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

using UnityEngine;


public class ParameterManager : MonoBehaviour
{
    //Singelton
    public static ParameterManager Instance;

    //Parameter Configs
    public GameParameter GameParameter;
    public AudioParameter AudioParameter;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}

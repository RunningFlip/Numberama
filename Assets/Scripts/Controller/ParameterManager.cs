using UnityEngine;


public class ParameterManager : MonoBehaviour
{
    //Singelton
    public static ParameterManager Instance;


    public GameParameter GameParameter;
    public AudioParameter AudioParameter;


    private void Awake()
    {
        //Singelton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }
}

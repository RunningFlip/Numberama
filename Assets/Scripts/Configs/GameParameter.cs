using UnityEngine;


[CreateAssetMenu(fileName = "GameParameter", menuName = "Parameter")]
public class GameParameter : ScriptableObject
{
    [Header("General")]
    public int maxLineLength = 9;
    public int maxBackTracking = 10;

    [Header("Number Pattern Configs")]
    public NumberPatternConfig defaultNumberPatternConfig;
    public NumberPatternConfig hardNumberPatternConfig;
}

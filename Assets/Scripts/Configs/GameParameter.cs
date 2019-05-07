using UnityEngine;


[CreateAssetMenu(fileName = "GameParameter", menuName = "Parameter/Game Parameter")]
public class GameParameter : ScriptableObject
{
    [Header("General")]
    public int maxLineLength = 9;
    public int maxBackTracking = 10;

    [Header("Number Pattern Configs")]
    public NumberPatternConfig defaultNumberPatternConfig;
    public NumberPatternConfig hardNumberPatternConfig;

    [Header("Number Field Prefabs")]
    public GameObject numberFieldPrefab_Left;
    public GameObject numberFieldPrefab_Right;
}

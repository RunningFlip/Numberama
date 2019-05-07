using UnityEngine;
using System;


[CreateAssetMenu(fileName = "AudioParameter", menuName = "Parameter/Audio Parameter")]
public class AudioParameter : ScriptableObject
{
    [Header("General")]
    public float easeSpeed;
    public float introEaseTime;

    [Header("Audio UI")]
    public AudioObject defaultButtonClip;
    public AudioObject defaultToggleClip;
    public AudioObject defaultInputFieldClip;

    [Header("Audio Music")]
    public AudioObject menuMusic;
    public AudioObject inGameMusic;

    [Header("Game Sounds")]
    public AudioObject selectionClip;
    public AudioObject deselectionClip;
    public AudioObject strikeClip;
    public AudioObject hintClip;
    public AudioObject undoClip;
}

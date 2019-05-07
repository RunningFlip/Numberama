using UnityEngine;
using System;


[CreateAssetMenu(fileName = "AudioParameter", menuName = "Parameter/Audio Parameter")]
public class AudioParameter : ScriptableObject
{
    [Header("General")]
    public float easeSpeed;
    public float introEaseTime;

    [Header("Audio UI")]
    public AudioClipInformation defaultButtonClip;
    public AudioClipInformation defaultToggleClip;
    public AudioClipInformation defaultInputFieldClip;

    [Header("Audio Music")]
    public AudioClipInformation menuMusic;
    public AudioClipInformation inGameMusic;

    [Header("Game Sounds")]
    public AudioClipInformation selectionClip;
    public AudioClipInformation deselectionClip;
    public AudioClipInformation strikeFieldClip;
    public AudioClipInformation strikeLineClip;
    public AudioClipInformation addNumbersClip;
    public AudioClipInformation hintClip;
    public AudioClipInformation undoClip;
}

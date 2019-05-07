using System.Collections;
using UnityEngine;


public class MusicController : MonoBehaviour
{
    //Singelton
    public static MusicController Instance;


    //Type
    private MusicType musicType = MusicType.None;

    //Audio
    private bool secondSourceWasInUse;

    //Values
    private float easeSpeed;
    private float musicVolume;
    private float introEaseTime;

    //Sources
    private int index;
    private AudioSource[] audioSources = new AudioSource[0];


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


    private void Start()
    {
        //Init
        introEaseTime = ParameterManager.Instance.AudioParameter.introEaseTime;
    }


    /// <summary>
    /// Changes the music to a given type.
    /// </summary>
    /// <param name="_musicType"></param>
    public void SetMusicType(MusicType _musicType)
    {
        if (audioSources.Length == 0) Setup();

        if (_musicType != musicType)
        {
            musicType = _musicType;
            AudioClip audioClip = null;

            switch (musicType)
            {
                case MusicType.Menu:
                    audioClip   = ParameterManager.Instance.AudioParameter.menuMusic.audioClip;
                    musicVolume = ParameterManager.Instance.AudioParameter.menuMusic.volume;
                    break;

                case MusicType.InGame:
                    audioClip   = ParameterManager.Instance.AudioParameter.inGameMusic.audioClip;
                    musicVolume = ParameterManager.Instance.AudioParameter.inGameMusic.volume;
                    break;
            }

            HandleEase(audioClip);
        }
    }


    /// <summary>
    /// Setups the musiccontroller.
    /// </summary>
    private void Setup()
    {
        index = 0;
        audioSources = GetComponents<AudioSource>();
        easeSpeed = ParameterManager.Instance.AudioParameter.easeSpeed * 0.5f;
    }


    /// <summary>
    /// Prepares the easing.
    /// </summary>
    /// <param name="_audioClip"></param>
    private void HandleEase(AudioClip _audioClip)
    {
        StopAllCoroutines(); //Safty first

        if (!secondSourceWasInUse && index == 1) secondSourceWasInUse = true;
        if (secondSourceWasInUse) StartCoroutine(EaseAudioClip(index, false));

        index++;
        if (index == 2) index = 0;

        StartCoroutine(EaseAudioClip(index, true, _audioClip));
    }


    /// <summary>
    /// Eases an audioclip in or out.
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_easeIn"></param>
    /// <param name="_audioClip"></param>
    /// <returns></returns>
    private IEnumerator EaseAudioClip(int _index, bool _easeIn, AudioClip _audioClip = null)
    {
        AudioSource audioSource = audioSources[_index];

        float multiplier = (_easeIn) ? 1 : -1;
        bool destinationReached = false;

        if (_audioClip != null)
        {
            audioSource.clip = _audioClip;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.Play();
        }

        while (!destinationReached)
        {
            audioSource.volume += musicVolume * easeSpeed * multiplier;

            if (_easeIn && audioSource.volume >= musicVolume)
            {
                destinationReached = true;
                audioSource.volume = musicVolume;

            }
            else if (!_easeIn && audioSource.volume <= 0) {
                destinationReached = true;
                audioSource.volume = 0;
                audioSource.Stop();
            }
            yield return null;
        }
    }


    /// <summary>
    /// Starts easing the music while starting the menu.
    /// </summary>
    /// <param name="_status"></param>
    public void IntroEasing(bool _status)
    {
        if (_status)
        {
            StartCoroutine(IntroEase(audioSources[1]));
        }
        else
        {
            StopCoroutine(IntroEase());
        }
    }


    /// <summary>
    /// Routine for the intro easing.
    /// </summary>
    /// <param name="_source"></param>
    /// <returns></returns>
    private IEnumerator IntroEase(AudioSource _source = null)
    {
        if (_source != null) {
            bool easing = true;

            float step = _source.volume / (introEaseTime / Time.deltaTime);

            while (easing)
            {
                _source.volume -= step;
                if (_source.volume <= 0) easing = false;

                yield return null;
            }
        }
    }
}

using UnityEngine;


public struct AudioClipObject
{
    private AudioClipInformation audioClipInformation;
    private Vector3 zeroVec;


    public AudioClipObject(AudioClipInformation _audioClipInformation)
    {
        audioClipInformation = _audioClipInformation;
        zeroVec = Vector3.zero;
    }


    public void PlaySound()
    {
        if (audioClipInformation.audioClip != null)
        {
            AudioSource.PlayClipAtPoint(audioClipInformation.audioClip, zeroVec, audioClipInformation.volume);
        }
    }
}

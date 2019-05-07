using UnityEngine;


public class AudioClipObject
{
    private AudioClipInformation audioClipInformation;
    private Vector3 zeroVec = Vector3.zero;


    public AudioClipObject(AudioClipInformation _audioClipInformation)
    {
        audioClipInformation = _audioClipInformation;
    }


    public void PlaySound()
    {
        if (audioClipInformation.audioClip != null)
        {
            AudioSource.PlayClipAtPoint(audioClipInformation.audioClip, zeroVec, audioClipInformation.volume);
        }
    }
}

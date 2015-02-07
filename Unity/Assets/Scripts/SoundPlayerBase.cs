using UnityEngine;

public abstract class SoundPlayerBase : MonoBehaviour
{
    protected AudioSource _audioPlayer;

    protected virtual void Start()
    {
        _audioPlayer = this.GetComponent<AudioSource>();
        if (_audioPlayer == null)
        {
            Debug.LogError(this.gameObject.name + " is missing its AudioSource component");
        }
    }

    protected virtual void PlayRandomSound(AudioClip[] audioClips, bool playOneShot = false)
    {
        if (_audioPlayer.isPlaying)
        {
            var newAudioPlayer = this.gameObject.AddComponent<AudioSource>();
            PlayRandomSound(newAudioPlayer, audioClips, playOneShot);
        }
        else
        {
            PlayRandomSound(_audioPlayer, audioClips, playOneShot);
        }
    }

    protected virtual void PlayRandomSound(AudioSource player, AudioClip[] audioClips, bool playOneShot=false)
    {
        if (player.isPlaying)
        {
            return;
        }

        int random = Random.Range(0, audioClips.Length);
        var audioClip = audioClips[random];
        if (audioClip == null)
        {
            Debug.LogError(this.gameObject.name + " could not find the audioclip specified in: " + audioClips);
            return;
        }
        
        if (!playOneShot)
        {
            player.clip = audioClip;
            player.Play();
        }
        else
        {
            AudioSource.PlayClipAtPoint(audioClip, this.transform.position);
        }
    }
}
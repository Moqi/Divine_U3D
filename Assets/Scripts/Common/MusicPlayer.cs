using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	
	public string		musicKey;
	public AudioSource	audioSource;
	
	public bool Load(string musicKey) {
        Debug.Log("Music Load:: " + musicKey);

        this.musicKey = musicKey;
		audioSource.clip = (AudioClip)Resources.Load(musicKey, typeof(AudioClip));
		if(null == audioSource.clip)
		{
			Debug.LogError("Error " + audioSource.clip + ": " + musicKey);
			return false;
		}
		return true;
	}
	
    public bool Play() {
		if (audioSource.clip == null) {
			if (!Load(musicKey)) {
				Debug.LogError("Error " + audioSource.clip + ": " + musicKey);
				return false;
			}
		}
        audioSource.Play();
        return IsPlaying();
    }
	
	public void SetLoop(bool isLoop) {
        audioSource.loop = isLoop;
    }

    public void Pause()              { audioSource.Pause(); }
    public void Stop()               { audioSource.Stop(); }
	public bool IsPlaying()          { return audioSource.isPlaying; }
	public float GetPlayTime()       { return audioSource.time; }
}

using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	
	public string		musicKey;
	
	public AudioSource	audioSource;
	
	public bool Load(string music_key)
	{
		musicKey = music_key;
		Debug.Log("Load:: " + musicKey);
		audioSource.clip = (AudioClip)Resources.Load(musicKey, typeof(AudioClip));
		if(null == audioSource.clip)
		{
			Debug.LogError("Error " + audioSource.clip + ": " + musicKey);
			return false;
		}
		return true;
	}
	
    public void Play()
    {
		if (audioSource.clip == null)
		{
			if (!Load (musicKey))
			{
				Debug.LogError("Error " + audioSource.clip + ": " + musicKey);
				return;
			}
		}
        audioSource.Play();
		Debug.Log(audioSource.isPlaying);
    }
	
	public void SetLoop(bool isLoop)
	{
		audioSource.loop = isLoop;
	}
	
	public bool IsPlaying()
	{
		return audioSource.isPlaying;
	}
	
	public void Stop()
	{
		Debug.Log(musicKey);
		audioSource.Stop();
	}
	
	public void Pause()
	{
		audioSource.Pause();
	}
	
	public float GetPlayTime()
	{
		return audioSource.time;
	}
}

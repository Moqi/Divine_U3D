using UnityEngine;
using System.Collections;

public class StartMain : MonoBehaviour {
	
	public MusicPlayer musicPlayer;
	
	// Use this for initialization
	void Start () {
		musicPlayer = Instantiate(musicPlayer) as MusicPlayer;
		musicPlayer.audioSource.loop = true;
		musicPlayer.Load("Music/start");
		musicPlayer.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("Quit");
			Application.Quit();
		}
		else if (Input.GetKeyDown(KeyCode.Home))
		{
			
		}
	}
	
	void OnGUI () {
		//GUILayout.Label("Start");
	}
}

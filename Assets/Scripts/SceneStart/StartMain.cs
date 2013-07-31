using UnityEngine;
using System.Collections;

public class StartMain : MonoBehaviour {

    private const string musicPlayerKey = "Prefab/Common/MusicPlayer";
	private const string startButtonKey = "Prefab/SceneStart/StartButton";
    private const string startMusicKey = "Music/Start";

    private MusicPlayer musicPlayer;
	private Animation2D startButton;

	void Start()
	{
		MusicPlayer musicPlayerPrefab = (MusicPlayer)Resources.Load(musicPlayerKey, typeof(MusicPlayer));
		musicPlayer = Instantiate(musicPlayerPrefab) as MusicPlayer;
		musicPlayer.Load(startMusicKey);
		musicPlayer.SetLoop(true);
		musicPlayer.Play();

		Animation2D startButtonPrefab = (Animation2D)Resources.Load(startButtonKey, typeof(Animation2D));
		startButton = Instantiate(startButtonPrefab) as Animation2D;
		startButton.drawRect = new Rect(
			Screen.width * (1 - 0.618f) * 0.5f,
			Screen.height * 0.618f,
			Screen.width * 0.618f,
			Screen.width * 0.618f * 0.15f);
		startButton.SetDrawColorFunc(StartButton_DrawAlpahFunc);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (TouchControl.IsOnTouch(new Rect(0, 0, Screen.width, Screen.height)))
		{
			Application.LoadLevel("List");
		}
	}

	void OnGUI()
	{
		startButton.Draw();
	}

	public void StartButton_DrawAlpahFunc(float curTime, out Color drawColor)
	{
		drawColor = GUI.color;
		drawColor.a = Mathf.Sin(curTime * 4) + 0.3f;
	}
}

using UnityEngine;
using System.Collections;

public class LevelPreview : MonoBehaviour {

	public Animation2D diskImage;
	public MusicPlayer musicPlayer;

	public Animation2D maxScoreLabel;
	public NumberLabel maxScoreNumber;

	void Start() {
	
	}
	
	void Update() {
	
	}

	void OnGUI() {
		diskImage.Draw();
		maxScoreLabel.Draw();
	}
}

using UnityEngine;
using System.Collections;

public class WelcomeText : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	int CurrentFadeDir=-1;
	float preAnimateTime=0;
	// Update is called once per frame
	void Update () {
		if (IsTouch()) {
			Application.LoadLevel("MusicList");
		}
		
		if (Time.time - preAnimateTime < (1f/25)) return;
		preAnimateTime = Time.time;
		
		Color tmp = this.guiTexture.color;
		tmp.a += ((float)CurrentFadeDir)*0.04f;
		CurrentFadeDir = (tmp.a < 1e-3f) ? 1 : (tmp.a > 0.98) ? -1 : CurrentFadeDir;
		this.guiTexture.color = tmp;
		
		
	}
	
	Vector3 leftDown, upRight;
	string tchLocation;
	
	public bool IsTouch() {

		
		//leftDown = (this.transform.position - this.transform.localScale/2);
		leftDown = new Vector3(0, 0, 0);
		leftDown.x = Screen.width * leftDown.x;
		leftDown.y = Screen.height * leftDown.y;
		//upRight = this.transform.position + this.transform.localScale/2;
		upRight = new Vector3(1, 1, 1);
		upRight.x = Screen.width * upRight.x;
		upRight.y = Screen.height * upRight.y;
		
		tchLocation="";
		foreach (UnityEngine.Touch tch in Input.touches) {
			tchLocation += tch.position+"\n";
			if (tch.position.x > leftDown.x && tch.position.x < upRight.x &&
				tch.position.y > leftDown.y && tch.position.y < upRight.y &&
				tch.phase == TouchPhase.Ended)
				return true;
			
		}
		
		return false;
	}
	
	void OnGUI() {
		GUILayout.Label(tchLocation);
	}
	
	
	
}

using UnityEngine;
using System.Collections;

public class StoryBackGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Global.Skip) {
			Vector3 size = this.guiTexture.transform.localScale;
			size.x=0;
			guiTexture.transform.localScale = size;
		}
			
	}
}

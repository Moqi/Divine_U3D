using UnityEngine;
using System.Collections;

public class MusicMain : MonoBehaviour {
	
	//private DiskImage diskImgObj;
	
	// Use this for initialization
	void Start () {
		//GUITexture diskImg = GameObject.Find("DiskImage").guiTexture;
		//diskImgObj = (DiskImage)diskImg.GetComponentInChildren(typeof(DiskImage));
		
		//diskImgObj.SwitchPreviewMusic(Global.GetCurrentSongIndex());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("Escape: Back");
			Application.LoadLevel("Start");
		}
	}
}

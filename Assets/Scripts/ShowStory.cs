using UnityEngine;
using System.Collections;

public class ShowStory : MonoBehaviour {
	float initHeigh = 0.2f;
	float startTime;
	string story;
	float lineHeigh, totalHeigh;
	public float LinePerSecond = 0.5f;
	// Use this for initialization
	void Start () {
		
		this.guiText.material.color = Color.black;
		TextAsset textAssert = (TextAsset)Resources.Load ("Story", typeof(TextAsset));
		story = textAssert.text;
		
		CharacterInfo info;
		guiText.font.GetCharacterInfo(story[0], out info);
		Debug.Log(info.vert);
		//lineHeigh = -info.vert.height;
		float fontSize = guiText.fontSize < 1e-2 ? 12f : guiText.fontSize;
		Debug.Log(Screen.dpi);
		lineHeigh = (fontSize/72f) * Screen.dpi;
		//int charWidth = (int)lineHeigh;
		int charWidth = (int)info.vert.width;
		int charsPerLine = (int)(Screen.width/charWidth);
		story = SplitLine(story, charsPerLine);
		Debug.Log(story);
		
		totalHeigh = lineHeigh * LineCount(story);
		guiText.text = story;
	
		
		
	}
	
	int LineCount(string content) {
		int count=0;
		foreach (char c  in content) {
			if (c == '\n')
				count++;
		}
		return count;
	}
	
	string SplitLine(string content, int charsPerLine) {
		string newContent="";
		int count=0;
		for (int i=0; i<content.Length; i++) {
			if (content[i] == '\n') {
				count=0;
			}
			if (count == charsPerLine) {
				newContent += "\n";
				count = 0;
			}
			newContent += content[i];
			count++;
		}
		return newContent;
	}
	
	int number;
	// Update is called once per frame
	void Update () {
		if (Global.Skip) {
			guiText.text = null;
			return;
		}
		Vector3 tmp = guiText.transform.position;
		float newY = initHeigh + Time.timeSinceLevelLoad * LinePerSecond / Screen.height;
		//if (newY > totalHeigh/Screen.height) {
		//	Global.Skip = true;
		//	return;
		//}
		tmp.y = newY;
		guiText.transform.position = tmp;
		
	}
	
	void OnGUI() {
		//Debug.Log("OnGUI Called");
		
	//	GUILayout.Label (""+(Screen.width/info.vert.width));
		
	}
}

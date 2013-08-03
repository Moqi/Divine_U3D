using UnityEngine;
using System.Collections;

public class JudgeTextControler : MonoBehaviour {
	
	public Texture2D missTex;
	public Texture2D perfectTex;
	public Texture2D goodTex;
	public Texture2D badTex;
	
	private TapResult tapResult;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	
	public void SetTapResult(TapResult tr)
	{
		tapResult = tr;
		if (tapResult == TapResult.TAP_RESULT_MISS) gameObject.guiTexture.texture = missTex;
		if (tapResult == TapResult.TAP_RESULT_PERFECT) gameObject.guiTexture.texture = perfectTex;
		if (tapResult == TapResult.TAP_RESULT_GOOD) gameObject.guiTexture.texture = goodTex;
		if (tapResult == TapResult.TAP_RESULT_BAD) gameObject.guiTexture.texture = badTex;
	}
	public void SetPos(Vector2 pos)
	{
		Vector3 pos3 = new Vector3(pos.x,pos.y,transform.position.z);
		transform.position = pos3;
	}
}

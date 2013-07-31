using UnityEngine;
using System.Collections;

public class ReadyEffectPlayer : MonoBehaviour {
	private const int EFFECT_INTERVAL = 10;
	public Texture2D[] readyEff;
	private Effecter readyEffecter;
	private int frameCount;
	private const int TotalFrame = 60;
	// Use this for initialization
	void Start () {
		readyEffecter = new Effecter(guiTexture,readyEff,0,10000,false,1.0f);
		frameCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		bool playing = readyEffecter.Update(GetAlpha(frameCount));
		if (playing == false) Destroy(gameObject);
		frameCount++;
		if (frameCount>TotalFrame) Destroy(gameObject);
	}
	
	float GetAlpha(float fc)
	{
		if (fc<(TotalFrame/2)) return fc/(TotalFrame/2);
		if (fc>=(TotalFrame/2)) return 2.0f - fc/(TotalFrame/2);
		return 0.0f;
	}
}

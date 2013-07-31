using UnityEngine;
using System.Collections;

public class Animation2D : MonoBehaviour {

	public Texture2D[] textureList;
    public float interval;
	public bool loopPlay;
	public Color drawColor;
	public Rect drawRect;

    private int curTextureID	= -1;
    private bool isPlaying		= false;
    private float startTime;

	public delegate void DrawRectFunc(float curTime, out Rect curDrawRect);
	public delegate void DrawColorFunc(float curTIme, out Color curDrawColor);

	private DrawRectFunc drawRectFunc = null;
	private DrawColorFunc drawColorFunc = null;

	void Awake() {
		drawColor = GUI.color;
		drawRect = new Rect(0f, 0f, 100f, 100f);
	}

	void Start() {
		Play();
	}

	void Update() {
		if (false == isPlaying)
			return;

		float playTime = Time.time - startTime;
		if (interval != 0)
		{
			float loopTime = textureList.Length * interval;
			if (loopPlay) playTime %= loopTime;
			if (loopTime < playTime)
			{
				Stop();
				return;
			}

			curTextureID = (int)(playTime / interval);
		}
		else curTextureID = 0;

		if (drawRectFunc != null) drawRectFunc(playTime, out drawRect);
		if (drawColorFunc != null) drawColorFunc(playTime, out drawColor);
	}

	public void Draw() {
		if (false == isPlaying) return;
		if (-1 == curTextureID) return;

		Color tmpColor = GUI.color;
		GUI.color = drawColor;
		GUI.DrawTexture(drawRect, textureList[curTextureID]);
		GUI.color = tmpColor;
	}

	public void SetTextures(Texture2D[] textureList)
	{
		this.textureList = textureList;
	}

	public void SetInterval(float interval)
	{
		this.interval = interval;
	}

	public void SetDrawColorFunc(DrawColorFunc drawColorFunc)
	{
		this.drawColorFunc = drawColorFunc;
	}

	public void SetDrawRectFunc(DrawRectFunc drawRectFunc)
	{
		this.drawRectFunc = drawRectFunc;
	}

	public void Play() {
		isPlaying = true;
		if (-1 == curTextureID)
		{
			curTextureID = 0;
			startTime = Time.time;
		} else {
			startTime = Time.time - startTime;
		}
	}
	
	public void Pause() {
		isPlaying = false;
		startTime = Time.time - startTime;
	}

	public void Stop() {
		isPlaying = false;
		curTextureID = -1;
	}
}
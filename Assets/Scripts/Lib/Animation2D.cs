using UnityEngine;
using System.Collections;

public class Animation2D : MonoBehaviour {
	
	public GUITexture target;
	public Texture2D[] fragments;
	public bool loopPlay;
	public int interval;
	public int beginFragID;
	
	private int totFragNum;
	private int curFragNum;
	private bool isPlaying;
	
	void Start()
	{
		totFragNum	= (fragments.Length-1)*(interval+1)+1;
		isPlaying	= false;
	}
	
	void Update()
	{
		if (false == isPlaying)
			return;
		if (null == target)
			return;
		
		if (fragments.Length<=0||curFragNum>=totFragNum)
		{
			if (this.loopPlay)
			{
				curFragNum = beginFragID * (interval + 1);
				if (curFragNum>=totFragNum)
				{
					return;
				}
			}else {
				return;
			}
		}
		int curFragID = curFragNum/(interval+1);
		target.texture = fragments[curFragID];
		curFragNum++;
	}
	
	public void Play() {
		isPlaying = true;
	}
	
	public void Pause() {
		isPlaying = false;
	}
}
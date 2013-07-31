using UnityEngine;
using System.Collections;


public class Effecter
{
	private Texture2D[] fragments;
	private GUITexture target;
	private int beginFragID;
	private int interval;
	private int curFragNum;
	private bool loopPlay;
	Rect oldPixelInset;
	float scale;
	
	public Effecter(GUITexture target,Texture2D[] frags,int beginFragID,int interval, bool isLoop,float scale)
	{
		this.target = target;
		this.fragments = frags;
		this.beginFragID = beginFragID;
		this.interval = interval;
		this.loopPlay = isLoop;
		curFragNum = beginFragID*(interval+1);
		this.scale = scale;
		oldPixelInset = target.pixelInset;
	}
	public bool Update(float alpha)
	{
		int totFragNum = (fragments.Length-1)*(interval+1)+1;
		Rect rect = new Rect(oldPixelInset.x*scale,oldPixelInset.y*scale,oldPixelInset.width*scale,oldPixelInset.height*scale);
		target.pixelInset = rect;
		if (fragments.Length<=0||curFragNum>=totFragNum)
		{
			if (this.loopPlay)
			{
				curFragNum = beginFragID * (interval + 1);
				if (curFragNum>=totFragNum)
				{
					return false;
				}
			}else {
				target.pixelInset = oldPixelInset;
				return false;
			}
		}
		int curFragID = curFragNum/(interval+1);
		target.texture = fragments[curFragID];
		Color color = target.color;
		color.a = alpha;
		target.color = color;
		curFragNum++;
		return true;
	}
	
	public bool IsEffectFinished()
	{
		int totFragNum = (fragments.Length-1)*(interval+1)+1;
		if (loopPlay==false&&curFragNum>=totFragNum)
			return true;
		else return false;
	}
}





public class TapPoint : MonoBehaviour {
	
	public int radius;
	public float perfectTimeLimit;
	public float goodTimeLimit;
	public JudgeTextControler judgeTextControler;
	public SlideLineControler slideLineControler;
	
	public Texture2D[] beforeTapEff;
	public Texture2D[] succeedTapEff;
	public Texture2D[] failedTapEff;
	
	private Effecter beforeEffecter;
	private Effecter succeedEffecter;
	private Effecter failedEffecter;
	
	private float fullScoreTime;
	
	private const int EFFECT_INTERVAL = 5;
	private Effecter curEffecter;
	private TapPointData tapPointData;
	private MusicPlayer musicPlayer;
	private bool hasLine;
	
	public TapPoint()
	{
		hasLine = false;
	}

	// Use this for initialization
	void Start () {
		
		beforeEffecter = new Effecter(guiTexture,beforeTapEff,0,1,false,1.0f);
		succeedEffecter = new Effecter(guiTexture,succeedTapEff,0,1,false,2.0f);
		failedEffecter = new Effecter(guiTexture,failedTapEff,0,0,false,1.0f);
		curEffecter = beforeEffecter;
	}
	void FixedUpdate()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		TapResult tr = GetTapResult();
		if (tr!=TapResult.TAP_RESULT_UNKNOW)
		{
			if (tapPointData.result == TapResult.TAP_RESULT_UNKNOW)
			{
				tapPointData.result = tr;
				ScoreCalculator.PushTapResult(tapPointData);
				judgeTextControler = Instantiate(judgeTextControler) as JudgeTextControler;
				judgeTextControler.SetTapResult(tapPointData.result);
				judgeTextControler.SetPos(new Vector2(gameObject.transform.position.x,gameObject.transform.position.y));
				if (hasLine) Destroy(slideLineControler.gameObject);
				//Debug.Log("hasLine:"+hasLine.ToString());
				if (tapPointData.result == TapResult.TAP_RESULT_PERFECT || tapPointData.result == TapResult.TAP_RESULT_GOOD)
				{
					
					curEffecter = succeedEffecter;
				}
				if (tapPointData.result == TapResult.TAP_RESULT_BAD || tapPointData.result == TapResult.TAP_RESULT_MISS)
					curEffecter = failedEffecter;
			}
		}
		curEffecter.Update(1.0f);
		if (IsEffectFinished()) 
		{
			Destroy(judgeTextControler.gameObject);
			Destroy(gameObject);
		}
	}
	
	bool IsEffectFinished()
	{
		if (succeedEffecter.IsEffectFinished()||failedEffecter.IsEffectFinished()) return true;
		return false;
	}
	
	
	TapResult GetTapResult()
	{
		float curTime = musicPlayer.GetPlayTime();
		float timeDelta = Mathf.Abs(fullScoreTime - curTime );
		if (DetectTap())
		{
			if (timeDelta <= perfectTimeLimit) return TapResult.TAP_RESULT_PERFECT;
			else if (timeDelta <= goodTimeLimit) return TapResult.TAP_RESULT_GOOD;
			else return TapResult.TAP_RESULT_BAD;
		}
		else 
		{
			if (curTime >= (fullScoreTime + goodTimeLimit)) 
			{
				return TapResult.TAP_RESULT_MISS;
			}
			else
			{
				return TapResult.TAP_RESULT_UNKNOW;
			}
			
		}
	}
	
	/**
	 * return value = false: miss
	 * true: hit
	 **/
	
	bool DetectTap()
	{
		int len = Input.touches.Length;
		if (len>0)
		{
			string debugLog = len.ToString();
			for (int i=0;i<len;i++)
			{
				Vector2 hitPnt = Input.touches[i].position;
				TouchPhase phase = Input.touches[i].phase;
				debugLog+=" "+hitPnt.ToString() +" "+phase.ToString()+" "+tapPointData.tapPhase.ToString() ;
				if (phase != tapPointData.tapPhase) continue;
				Vector2 pos = new Vector2(transform.position.x*Screen.width,transform.position.y*Screen.height);
				float dis = Vector2.Distance(hitPnt,pos);
				if (dis<=radius) return true;
			}
		}
		return false;
	}
	
	public void SetFullScoreTime(float tm)
	{
		fullScoreTime = tm;
	}
	
	public void SetPos(Vector2 pos)
	{
		//Debug.Log("pos:"+pos.ToString());
		float x = pos.x/Screen.width;
		float y = pos.y/Screen.height;
		transform.position = new Vector2(x,y);
	}
	
	public void SetTapPointData(TapPointData tpo)
	{
		tapPointData = tpo;
	}
	
	public void SetMusicPlayer(MusicPlayer mp)
	{
		musicPlayer = mp;
	}
	
	public void SetSize(float size)
	{
		Rect rt = new Rect();
		rt.x = -size/2.0f;
		rt.y = -size/2.0f;
		rt.width = size;
		rt.height = size;
		gameObject.guiTexture.pixelInset = rt;
	}
	
	public void SetNextTapPointPos(Vector2 nextPntPos)
	{
		hasLine = true;
		slideLineControler = Instantiate(slideLineControler) as SlideLineControler;
		Vector2 currPos = new Vector2(transform.position.x*Screen.width,transform.position.y*Screen.height);
		slideLineControler.SetEndPnts(currPos,nextPntPos);
	}
}

using UnityEngine;
using System.Collections;

enum PlaneEffectTextureType:int 
{
	TEXTURE_TYPE_ARRAY = 0,
	TEXTURE_TYPE_TILE = 1
}

abstract class PlaneEffectTexture
{
	public abstract void SetTextureWithIndex(Renderer renderer ,int idx);
	public abstract int Length
	{
		get;
	}
	PlaneEffectTextureType type;
	public PlaneEffectTextureType Type
	{
		get {return type;}
		set {this.type = value; }
	}
}

class PlaneEffectTextureTile : PlaneEffectTexture
{
	private int numTex;
	private Texture2D tex;
	private int texUnitWidth;
	private int texUnitHeight;
	public PlaneEffectTextureTile(Texture2D tileTex,int numTex,int texUnitWidth,int texUnitHeight)
	{
		tex = tileTex;
		Type = PlaneEffectTextureType.TEXTURE_TYPE_TILE;
		this.numTex = numTex;
		this.texUnitWidth = texUnitWidth;
		this.texUnitHeight = texUnitHeight;
	}
	public override void SetTextureWithIndex(Renderer renderer,int idx)
	{
		
	}
	public override int Length
	{
		get
		{
			return numTex;
		}
	}
}

class PlaneEffectTextureArray : PlaneEffectTexture
{
	private Texture2D[] texs;
	public PlaneEffectTextureArray(Texture2D[] texs)
	{
		this.texs = texs;
		Type = PlaneEffectTextureType.TEXTURE_TYPE_ARRAY;
	}
	public override void SetTextureWithIndex(Renderer renderer,int idx)
	{
		renderer.material.SetTexture("_MainTex",texs[idx]);
	}
	public override int Length
	{
		get
		{
			return texs.Length;
		}
	}
}

abstract class PlaneEffecter
{
	protected PlaneEffectTexture fragments;
	protected GameObject target;
	protected int beginFragID;
	protected int interval;
	protected int curFragNum;
	protected int totFragNum;
	protected Vector3 targetScale;
	
	public PlaneEffecter(GameObject target,PlaneEffectTexture frags,int beginFragID,int interval)
	{
		this.target = target;
		this.fragments = frags;
		this.beginFragID = beginFragID;
		this.interval = interval;
		curFragNum = beginFragID*(interval+1);
		totFragNum = (fragments.Length-1)*(interval+1)+1;
		targetScale = new Vector3(target.transform.localScale.x,target.transform.localScale.y,target.transform.localScale.z);
	}
	
	public bool Update()
	{
		int totFragNum = (fragments.Length-1)*(interval+1)+1;
		
		if (fragments.Length<=0||curFragNum>=totFragNum) 
			return false;
		int curFragID = curFragNum/(interval+1);
		fragments.SetTextureWithIndex(target.renderer,curFragID);
		Color color = target.renderer.material.GetColor("_Color");
		color.a = GetAlphaWithIndex(curFragNum);
		target.renderer.material.SetColor("_Color",color);
		target.transform.Translate(GetTranslateWithIndex(curFragNum));
		target.transform.Rotate(GetRotateWithIndex(curFragNum));
		Vector3 scaleFactor = GetScaleWithIndex(curFragNum);
		target.transform.localScale = new Vector3(scaleFactor.x*targetScale.x,scaleFactor.y*targetScale.y,scaleFactor.z*targetScale.z);
		curFragNum++;
		return true;
	}
	
	public bool IsEffectFinished()
	{
		if (curFragNum>=totFragNum) return true;
		else return false;
	}
	protected virtual float GetAlphaWithIndex(int idx)
	{
		return 1.0f;
	}
	
	protected virtual Vector3 GetTranslateWithIndex(int idx)
	{
		return new Vector3(0,0,0);
	}
	
	protected virtual Vector3 GetRotateWithIndex(int idx)
	{
		return new Vector3(0,0,0);
	}
	
	protected virtual Vector3 GetScaleWithIndex(int idx)
	{
		return new Vector3(1,1,1);
	}
}

class BeforeTapPlaneEffecter : PlaneEffecter
{
	private float rotationAngle;
	public BeforeTapPlaneEffecter(GameObject target,PlaneEffectTexture frags,int beginFragID,int interval)
		:base(target,frags,beginFragID,interval)
	{
		float delta = Random.value*13.0f;
		rotationAngle = 30.0f + delta;
	}
	
	protected override float GetAlphaWithIndex(int idx)
	{
		return (float)idx/totFragNum;
	}
	
	protected override Vector3 GetRotateWithIndex(int idx)
	{
		return new Vector3(0,rotationAngle,0);
	}
	
	protected override Vector3 GetScaleWithIndex(int idx)
	{
		float ratio = (float)idx/totFragNum;
		return new Vector3(ratio,1.0f,ratio);
	}
}


class SucceedTapPlaneEffecter : PlaneEffecter
{
	public SucceedTapPlaneEffecter(GameObject target,PlaneEffectTexture frags,int beginFragID,int interval)
		:base(target,frags,beginFragID,interval)
	{
	}
	protected override Vector3 GetScaleWithIndex(int idx)
	{
		float ratio = 2.0f;
		return new Vector3(ratio,1.0f,ratio);
	}
}



class FailedTapPlaneEffecter : PlaneEffecter
{
	public FailedTapPlaneEffecter(GameObject target,PlaneEffectTexture frags,int beginFragID,int interval)
		:base(target,frags,beginFragID,interval)
	{
	}
	
	protected override float GetAlphaWithIndex(int idx)
	{
		return (float)idx/totFragNum;
	}
	
	protected override Vector3 GetRotateWithIndex(int idx)
	{
		return new Vector3(0,-30,0);
	}
	
	protected override Vector3 GetScaleWithIndex(int idx)
	{
		float ratio = 1.0f-(float)idx/totFragNum;
		return new Vector3(ratio,1.0f,ratio);
	}
}



/*
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




 */
public class TapPointPlane : MonoBehaviour {
	
	public int radius;
	public float perfectTimeLimit;
	public float goodTimeLimit;
	public JudgeTextControler judgeTextControler;
	public SlideLineControler slideLineControler;
	
	public Texture2D[] beforeTapEff;
	public Texture2D[] succeedTapEff;
	public Texture2D[] failedTapEff;
	
	private BeforeTapPlaneEffecter beforeEffecter;
	private SucceedTapPlaneEffecter succeedEffecter;
	private FailedTapPlaneEffecter failedEffecter;
	
	private float fullScoreTime;
	
	private const int EFFECT_INTERVAL = 5;
	private PlaneEffecter curEffecter;
	private TapPointData tapPointData;
	private MusicPlayer musicPlayer;
	private bool hasLine;
	
	public TapPointPlane()
	{
		hasLine = false;
	}
	
	void LoadTextures()
	{
		succeedTapEff = new Texture2D[61];
		for (int idx = 0; idx <=60;idx++)
		{
			string strIdx = idx.ToString("D3");
			succeedTapEff[idx] = 
				(Texture2D)Resources.Load("Image/Effects/NewSucceedTap/SuccessTap_"+strIdx,typeof(Texture2D));
		}
	}

	// Use this for initialization
	void Start () {
		LoadTextures();
		beforeEffecter = new BeforeTapPlaneEffecter(gameObject,
			new PlaneEffectTextureArray(beforeTapEff),0,20);
		succeedEffecter = new SucceedTapPlaneEffecter(gameObject,
			new PlaneEffectTextureArray(succeedTapEff),0,0);
		failedEffecter = new FailedTapPlaneEffecter(gameObject,
			new PlaneEffectTextureArray(failedTapEff),0,20);
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
				Vector2 srPos = Global.Coord_World2Screen(transform.position);
				judgeTextControler.SetPos(new Vector2(srPos.x/Screen.width,srPos.y/Screen.height));
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
		curEffecter.Update();
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
				//debugLog+=" "+hitPnt.ToString() +" "+phase.ToString()+" "+tapPointData.tapPhase.ToString() ;
				if (phase != tapPointData.tapPhase) continue;
				//Vector2 pos = new Vector2(transform.position.x*Screen.width,transform.position.y*Screen.height);
				Vector2 pos = Global.Coord_World2Screen(transform.position);
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
		//pos.x=0;
		//pos.y=0;
		//Debug.Log("screen pos:"+pos.ToString());
		//Debug.Log("world pos:"+Global.Coord_Screen2World(pos,0));
		//float x = pos.x/Screen.width;
		//float y = pos.y/Screen.height;
		transform.position = Global.Coord_Screen2World(pos,transform.position.z);
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
		/*Rect rt = new Rect();
		rt.x = -size/2.0f;
		rt.y = -size/2.0f;
		rt.width = size;
		rt.height = size;*/
		transform.localScale = new Vector3(size,1.0f,size);
		//gameObject.guiTexture.pixelInset = rt;
	}
	
	public void SetNextTapPointPos(Vector2 nextPntPos)
	{
		hasLine = true;
		slideLineControler = Instantiate(slideLineControler) as SlideLineControler;
		//Vector2 currPos = new Vector2(transform.position.x*Screen.width,transform.position.y*Screen.height);
		Vector2 currPos = Global.Coord_World2Screen(transform.position);
		slideLineControler.SetEndPnts(currPos,nextPntPos);
	}
}

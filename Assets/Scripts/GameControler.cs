using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TapType : int 
{
	TAP_TYPE_BIG = 0,
	TAP_TYPE_SMALL = 1
}

public class TapPointData
{
	public int idx;
	public TapType type;
	public float fullScoreTime;
	public int x;
	public int nextID;
	public TouchPhase tapPhase;
	public TapResult result;
	public TapPointData() 
	{
		result = TapResult.TAP_RESULT_UNKNOW;
	}
	public TapPointData(int idx,TapType tp,float ft,int x,int nextID,TouchPhase tapType)
	{
		this.idx = idx;
		this.type = tp;
		this.fullScoreTime = ft;
		this.x = x;
		this.nextID = nextID;
		this.tapPhase = tapType;
		result = TapResult.TAP_RESULT_UNKNOW;
	}
	public float GetAppearTime()
	{
		//return fullScoreTime - Global.TimeFrmAppear2Perfect;
		return 0;
	}
	
	public TapResult GetTapResult()
	{
		return result;
	}
}

public class GameControler : MonoBehaviour {
	
	public float readyDuration;
	public TapPointPlane tapPoint;
	private float startTime;
	private SortedList<int, TapPointData> tapPntQue;

	public ScanLineCtrl	scanLine;
	public MusicPlayer musicPlayer;
	public ScoreLabel scoreLabel;
	
	private bool		isInGame;
	
	private float		gameStartTime;
	
	void Start () {
		
		LevelInfo levelInfo = Global.GetLevelInfo(Global.GetCurrentLevelIndex());
		
		musicPlayer	= Instantiate(musicPlayer) as MusicPlayer;
		scanLine	= Instantiate(scanLine) as ScanLineCtrl;
		scoreLabel	= Instantiate(scoreLabel) as ScoreLabel;
		tapPntQue	= LoadLevel("Data/" + levelInfo.beatListFile);
		
		startTime = Time.realtimeSinceStartup;
		
		musicPlayer.Load("Music/" + levelInfo.songFile);
		scanLine.SetVisible(false);
		scoreLabel.SetVisible(true);
		
		for (int idx = 0; idx <=60;idx++)
		{
			string strIdx = idx.ToString("D3");
			Resources.Load("Image/Effects/NewSucceedTap/SuccessTap_"+strIdx, typeof(Texture2D));
		}

	}
	
	Queue<int> waitingForRemove = new Queue<int>();
	
	// Update is called once per frame
	void Update () {

		if (isInGame && Input.GetKeyDown(KeyCode.Escape))
		{
			LeaveGame("List");
		}
		
		//float curTime = Time.realtimeSinceStartup;
		//if (curTime > startTime + readyDuration)
		//{
		//    if ((!isInGame) && !musicPlayer.IsPlaying())
		//    {
		//        isInGame = true;
		//        gameStartTime = Time.time;
		//        musicPlayer.Play();
		//    }
		//    scanLine.SetVisible(true);

		//    scanLine.SetPosByTime(Time.time - gameStartTime);
		//    float deltaTime = curTime - startTime - readyDuration;
		//    foreach (TapPointData nextPnt in tapPntQue.Values)
		//    {
		//        //TapPointData nextPnt = tapPntQue.Peek();
		//        if (deltaTime >= nextPnt.GetAppearTime())
		//        {
		//            waitingForRemove.Enqueue(nextPnt.idx);
		//            //tapPntQue.Remove(nextPnt.idx);
		//            //tapPntQue.Dequeue();
		//            TapPointPlane tp;
		//            tp = Instantiate(tapPoint) as TapPointPlane;
		//            tp.SetPos(new Vector2(nextPnt.x,GetYFromTime(nextPnt.fullScoreTime)));
		//            tp.SetFullScoreTime(nextPnt.fullScoreTime);
		//            tp.SetTapPointData(nextPnt);
		//            tp.SetMusicPlayer(musicPlayer);
		//            if (nextPnt.type == TapType.TAP_TYPE_BIG) tp.SetSize(Global.TapPointSizeBig);
		//            if (nextPnt.type == TapType.TAP_TYPE_SMALL) tp.SetSize(Global.TapPointSizeSmall);
		//            if (tapPntQue.ContainsKey(nextPnt.nextID))
		//            {
		//                TapPointData nxt;
		//                tapPntQue.TryGetValue(nextPnt.nextID,out nxt);
		//                tp.SetNextTapPointPos(new Vector2(nxt.x,GetYFromTime(nxt.fullScoreTime)));
		//            }
		//            //livingTapPnts.Add(tp);
		//        }
		//        else break;
		//    }
		//    foreach (int idx in waitingForRemove)
		//        tapPntQue.Remove(idx);
		//}
		
		// Game Is Over
		if (isInGame)
		{
			if (!musicPlayer.IsPlaying())
			{ // 音乐结束则，游戏结束
				LeaveGame("Result");
			}
		}

	}
	
	void LeaveGame(string toScene)
	{
		isInGame = false;
		scoreLabel.SetVisible(false);
		scanLine.SetVisible(false);
		ScoreCalculator.ResetScore();
		Application.LoadLevel(toScene);
	}
	
	SortedList<int,TapPointData> GetMusicTapList(string filePath)
	{
		SortedList<int,TapPointData> que = new SortedList<int,TapPointData>();
		
		TapPointData tpo = new TapPointData();
		tpo.fullScoreTime = 1.0f;
		tpo.x = 100;
		tpo.idx = 0;
		tpo.tapPhase = TouchPhase.Began;
		que.Add(tpo.idx,tpo);
		
		TapPointData tpo1 = new TapPointData();
		tpo1.fullScoreTime = 2.0f;
		tpo1.x = 200;
		tpo1.idx = 1;
		tpo1.tapPhase = TouchPhase.Moved;
		que.Add(tpo1.idx,tpo1);
		
		TapPointData tpo2 = new TapPointData();
		tpo2.fullScoreTime = 3.0f;
		tpo2.x = 300;
		tpo2.idx = 2;
		tpo2.tapPhase = TouchPhase.Began;
		que.Add(tpo2.idx,tpo2);
		
		
		TapPointData tpo3 = new TapPointData();
		tpo3.fullScoreTime = 3.0f;
		tpo3.x = 400;
		tpo3.idx = 3;
		tpo3.tapPhase = TouchPhase.Began;
		que.Add(tpo3.idx,tpo3);
		return que;
	}
	
	int GetYFromTime(float time)
	{
		//float sweepNum = (time/Global.sweepOnceTime)/2.0f;
		//sweepNum = sweepNum - (int)sweepNum;
		//int pixelNum = (int)(sweepNum * Global.GamingZoneHeight*2);
		//if (pixelNum> Global.GamingZoneHeight) 
		//    pixelNum = 2*Global.GamingZoneHeight - pixelNum; 
		//return pixelNum + Global.GamingZoneBottom;
		return 0;
	}
	
	public SortedList<int, TapPointData> LoadLevel(string levelDataFile)
	{
		SortedList<int, TapPointData> res_list = new SortedList<int, TapPointData> ();
		
		DataLoader levelLoader = new DataLoader();
		
		levelLoader.LoadData(levelDataFile);
		
		int lines = levelLoader.getLineCnt();
		for (int i=1; i<lines; ++i)
		{
			TapPointData data = new TapPointData();
			data.idx = levelLoader.GetGridInt(i, 0, 0);
			if (data.idx == 0)
				continue;
			
			data.fullScoreTime = levelLoader.GetGridFloat(i, 1, 0.0f);
			data.type			= (TapType)levelLoader.GetGridInt(i, 2, 0);
			switch (levelLoader.GetGridInt(i, 6, 0))
			{
			case 0:
				data.tapPhase = TouchPhase.Began;
				break;
			case 1:
				data.tapPhase = TouchPhase.Moved;
				break;
			default:
				break;
			}
			data.x				= levelLoader.GetGridInt(i, 4, 0);
			data.nextID			= levelLoader.GetGridInt(i, 5, 0);
			
			res_list.Add(data.idx, data);
		}
		return res_list;
	}
}

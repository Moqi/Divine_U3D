using UnityEngine;
using System.Collections;
using System.Xml;

public struct LevelInfo {
	public string	songName;
	public string	songFile;
	public string	imageFile;
	public string	beatListFile;
	public string	songPreview;
	public float	sweepOnceTime;
};
	
public enum TapResult : int 
{
	TAP_RESULT_UNKNOW = 0,
	TAP_RESULT_MISS = 1,
	TAP_RESULT_BAD = 2,
	TAP_RESULT_GOOD = 3,
	TAP_RESULT_PERFECT = 4
}

public class Global : MonoBehaviour {
	//for skip
	public static bool Skip = false;
	
	static LevelInfo[] levelInfos;
	static int levelCount;
	static int curLevelIndex = 0;
	public static float sweepOnceTime = 1.3333333f;
	
	public static float TimeFrmAppear2Perfect = 1.0f;
	public static string SelectedMusicTapListFilePath = "";
	public static float TapPointSizeBig = 120.0f;
	public static float TapPointSizeSmall = 80.0f;
	public static float TapPointVisiableRadius = 5.0f;
	public static int GamingZoneHeight = Screen.height - 260;
	public static int GamingZoneBottom = 100;
	//from bottom to top
	public static int GamingZoneTop = Screen.height - 160;
	
	//for final result
	public static int curScore;
	public static int curMaxCombo;
	
	public static int perfectCnt;
	public static int goodCnt;
	public static int badCnt;
	public static int missCnt;
	
	public static void CleanUp()
	{
		Global.curScore		= 0;
		Global.curMaxCombo	= 0;
		
		perfectCnt	= 0;
		goodCnt		= 0;
		badCnt		= 0;
		missCnt		= 0;
	}
	
	public static bool IsNewBest()
	{
		int maxScore = PlayerPrefs.GetInt("MaxScore:Level" + curLevelIndex);
		return curScore > maxScore;
	}
	
	public static void SaveMaxScore()
	{
		if (IsNewBest())
		{
			PlayerPrefs.SetInt("MaxScore:Level" + curLevelIndex, curScore);
			PlayerPrefs.Save();
		}
	}
	
	public static void GetLevelInfos()
	{
		TextAsset xmlText = (TextAsset)Resources.Load ("LevelConfig", typeof(TextAsset));

		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(xmlText.text);
		
		XmlNodeList levelNodes	= xmlDoc.SelectSingleNode("Levels").ChildNodes;
		Global.levelInfos		= new LevelInfo[levelNodes.Count];
		Global.levelCount		= levelNodes.Count;
		
		int index = 0;
		foreach (XmlElement xmlNode in levelNodes) {
			levelInfos[index].songName		= xmlNode.GetAttribute("Name");
			levelInfos[index].songFile		= xmlNode.GetAttribute("SongFile");
			levelInfos[index].songPreview	= xmlNode.GetAttribute("SongPreview");
			levelInfos[index].imageFile		= xmlNode.GetAttribute("ImageFile");
			levelInfos[index].beatListFile	= xmlNode.GetAttribute("ConfigFile");
			float.TryParse(xmlNode.GetAttribute("SweepOnceTime"), out levelInfos[index].sweepOnceTime);
			index++;
		}
	}
	
	public static LevelInfo GetLevelInfo(int levelIndex)
	{
		if (levelInfos == null)
			GetLevelInfos();
		
		if (levelIndex < 0 || levelIndex >= levelCount)
			return levelInfos[0];
		
		return levelInfos[levelIndex];
	}
	
	public static int GetLevelCount()
	{
		return levelCount;
	}
	
	public static void SetCurrentLevelIndex(int levelIndex)
	{
		curLevelIndex = levelIndex;
		sweepOnceTime = levelInfos[levelIndex].sweepOnceTime;
	}
	
	public static int GetCurrentLevelIndex()
	{
		return curLevelIndex;
	}
	
	public static Vector3 Coord_Screen2World(Vector2 sc,float z)
	{
		Vector3 pos = new Vector3((sc.x-(Screen.width/2.0f))*10.0f,
			(sc.y-(Screen.height/2.0f))*10.0f,
			z);
		return pos;
	}
	
	public static Vector2 Coord_World2Screen(Vector3 wc)
	{
		Vector2 pos = new Vector2(wc.x/10.0f+(Screen.width/2.0f),wc.y/10.0f+(Screen.height/2.0f));
		return pos;
	}
}

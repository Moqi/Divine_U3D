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

public class LevelManager
{
	static LevelInfo[] levelInfos;
	static int levelCount;
	static int curLevelIndex = 0;

	public static void GetLevelInfos()
	{
		TextAsset xmlText = (TextAsset)Resources.Load("LevelConfig", typeof(TextAsset));

		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(xmlText.text);

		XmlNodeList levelNodes = xmlDoc.SelectSingleNode("Levels").ChildNodes;
		levelInfos = new LevelInfo[levelNodes.Count];
		levelCount = levelNodes.Count;

		int index = 0;
		foreach (XmlElement xmlNode in levelNodes)
		{
			levelInfos[index].songName = xmlNode.GetAttribute("Name");
			levelInfos[index].songFile = xmlNode.GetAttribute("SongFile");
			levelInfos[index].songPreview = xmlNode.GetAttribute("SongPreview");
			levelInfos[index].imageFile = xmlNode.GetAttribute("ImageFile");
			levelInfos[index].beatListFile = xmlNode.GetAttribute("ConfigFile");
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

	public static void SetCurrentLevelIndex(int levelIndex)
	{
		curLevelIndex = levelIndex;
	}

	public static int GetLevelCount() { return levelCount; }
	public static int GetCurrentLevelIndex() { return curLevelIndex; }
}

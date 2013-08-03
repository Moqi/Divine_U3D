using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader {
	
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

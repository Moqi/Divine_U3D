using UnityEngine;
using System;
using System.Collections;

public struct ScoreInfo {
	public int levelIndex;
	public int levelMaxScore;

	public int curScore;
	public int curCombo;
	public int curMaxCombo;

	public int perfectCnt;
	public int goodCnt;
	public int badCnt;
	public int missCnt;

	public void CleanUp()
	{
		levelIndex		= 0;
		levelMaxScore	= 0;

		curScore		= 0;
		curCombo		= 0;
		curMaxCombo		= 0;

		perfectCnt		= 0;
		goodCnt			= 0;
		badCnt			= 0;
		missCnt			= 0;
	}
}

public class ScoreManager
{
	const int perfectScore	= 100;
	const int goodScore		= 70;
	const int badScore		= 30;
	const int missScore		= 0;

	static ScoreInfo scoreInfo;

	static string GetLevelKey(int levelIndex)
	{
		return "::lv" + levelIndex;
	}

	public static void InitLevelScoreInfo (int index)
	{
		scoreInfo.CleanUp();
		scoreInfo.levelIndex = index;
		scoreInfo.levelMaxScore = PlayerPrefs.GetInt(GetLevelKey(scoreInfo.levelIndex));
	}

	public static bool IsNewBest()
	{
		return scoreInfo.curScore > scoreInfo.levelMaxScore;
	}

	public static void SaveMaxScore()
	{
		if (IsNewBest())
		{
			PlayerPrefs.SetInt(GetLevelKey(scoreInfo.levelIndex), scoreInfo.curScore);
			PlayerPrefs.Save();
		}
	}

	public static void UpdateResult(TapPointData tapPointData)
	{
		int baseScore = 0;
		switch (tapPointData.GetTapResult())
		{
			case TapResult.TAP_RESULT_PERFECT:
				baseScore = perfectScore;
				++scoreInfo.curCombo;
				++scoreInfo.perfectCnt;
				break;
			case TapResult.TAP_RESULT_GOOD:
				baseScore = goodScore;
				++scoreInfo.curCombo;
				++scoreInfo.goodCnt;
				break;
			case TapResult.TAP_RESULT_BAD:
				baseScore = badScore;
				scoreInfo.curCombo = 0;
				++scoreInfo.badCnt;
				break;
			case TapResult.TAP_RESULT_MISS:
				baseScore = missScore;
				scoreInfo.curCombo = 0;
				++scoreInfo.missCnt;
				break;
			default:
				break;
		}

		int comboScore = 0;
		if (scoreInfo.curCombo != 0)
		{
			int k_in_doc = scoreInfo.curCombo;
			int n_in_doc = (int)Math.Ceiling(scoreInfo.curCombo / 50.00f) - 1;
			comboScore = (k_in_doc - n_in_doc * 50) * (k_in_doc - n_in_doc * 50) * (n_in_doc + 1) * 10;

			if (scoreInfo.curCombo % 50 != 0)
			{
				k_in_doc = scoreInfo.curCombo - 1;
				n_in_doc = (int)Math.Ceiling(scoreInfo.curCombo / 50.00f) - 1;
				comboScore -= (k_in_doc - n_in_doc * 50) * (k_in_doc - n_in_doc * 50) * (n_in_doc + 1) * 10;
			}
		}

		scoreInfo.curScore += baseScore + comboScore;
		scoreInfo.curMaxCombo = Math.Max(scoreInfo.curMaxCombo, scoreInfo.curCombo);
	}
}

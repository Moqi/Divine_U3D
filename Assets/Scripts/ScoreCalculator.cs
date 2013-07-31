using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScoreCalculator : MonoBehaviour {
	
	public static int perfectScore	= 100;
	public static int goodScore		= 70;
	public static int badScore		= 30;
	public static int missScore		= 0;
	
	private static int score = 0;
	private static int currCombo = 0;
	
	public static int GetScore()
	{
		return score;
	}
	
	public static int GetCurrCombo()
	{
		return currCombo;
	}
	
	public static void ResetScore()
	{
		score = 0;
		currCombo = 0;
	}
	
	public static void PushTapResult(TapPointData tapPointData)
	{
		int base_score = 0;
		switch (tapPointData.GetTapResult())
		{
		case TapResult.TAP_RESULT_PERFECT:
			base_score = perfectScore;
			++ currCombo;
			++ Global.perfectCnt;
			break;
		case TapResult.TAP_RESULT_GOOD:
			base_score = goodScore;
			++ currCombo;
			++ Global.goodCnt;
			break;
		case TapResult.TAP_RESULT_BAD:
			base_score = badScore;
			currCombo = 0;
			++ Global.badCnt;
			break;
		case TapResult.TAP_RESULT_MISS:
			base_score = missScore;
			currCombo = 0;
			++ Global.missCnt;
			break;
		default:
			break;
		}
		
		int combo_score = 0;
		if(currCombo != 0)
		{
			int k_in_doc = currCombo;
			int n_in_doc = (int)Math.Ceiling(currCombo / 50.00f) - 1;
			combo_score = (k_in_doc - n_in_doc * 50) * (k_in_doc - n_in_doc * 50) * (n_in_doc + 1) * 10;
			
			if (currCombo % 50 != 0)
			{
				k_in_doc = currCombo - 1;
				n_in_doc = (int)Math.Ceiling(currCombo / 50.00f) - 1;
				combo_score -= (k_in_doc - n_in_doc * 50) * (k_in_doc - n_in_doc * 50) * (n_in_doc + 1) * 10;
			}
		}
		
		score += base_score + combo_score;
		
		Global.curScore = score;
		Global.curMaxCombo = Math.Max(Global.curMaxCombo, currCombo);
	}
}

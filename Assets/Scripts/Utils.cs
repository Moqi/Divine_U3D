using UnityEngine;
using System.Collections;

public class Utils {
	
	public static bool IsOnTouch_ByPercent (Vector2 leftDown, Vector2 RegionSize, 
		UnityEngine.Touch[] tchPoints ) {
		Vector2 leftDownPixel = new Vector2(leftDown.x*Screen.width, leftDown.y*Screen.height);
		Vector2 regionSizePixel = new Vector2(RegionSize.x*Screen.width, RegionSize.y*Screen.height);
		
		return IsOnTouch_ByPixel(leftDownPixel, regionSizePixel, tchPoints);
	}
	
	public static bool IsOnTouch_ByRect(Rect regionRect, UnityEngine.Touch[] tchPoints) {
		if (tchPoints == null) return false;
		
		Rect realRect = regionRect;
		
		if (realRect.width < 0)
		{
			realRect.width = -realRect.width;
			realRect.x -= realRect.width;
		}
		
		if (realRect.height < 0)
		{
			realRect.height = -realRect.height;
			realRect.y -= realRect.height;
		}
		
		Vector2 leftDown;
		Vector2 regionSize;
		
		leftDown.x = realRect.x;
		leftDown.y = Screen.height - realRect.y - realRect.height;
		
		regionSize.x = realRect.width;
		regionSize.y = realRect.height;
		
		//Debug.Log("P??? " + leftDown + "S??? " + regionSize);
		//Debug.Log("???? " + tchPoints[0].position);
		
		return IsOnTouch_ByPixel(leftDown, regionSize, tchPoints);
	}
	
	static UnityEngine.Touch beginTouch;
	
	public static bool IsOnTouch_ByPixel(Vector2 leftDown, Vector2 RegionSize, 
		UnityEngine.Touch[] tchPoints) {
		if (tchPoints == null) return false;
		if (RegionSize.x * RegionSize.y <= 0) return false;
		foreach (UnityEngine.Touch tch in tchPoints) {
			if (tch.position.x >= leftDown.x && tch.position.x <= (leftDown.x + RegionSize.x) 
				&& tch.position.y >= leftDown.y && tch.position.y <= (leftDown.y + RegionSize.y) &&
				tch.phase == TouchPhase.Ended && 
				beginTouch.position.x-tch.position.x > -32 && beginTouch.position.x-tch.position.x < 32 &&
				beginTouch.position.y-tch.position.y > -32 && beginTouch.position.y-tch.position.y < 32) {
				return true;
			} else if (tch.phase == TouchPhase.Began) 
				beginTouch = tch;
			
		}
		return false;
	}
	
	public static int GetDigitNumber(int number) {
		int count=1;
		while (number > 0) {
			number /= 10;
			if (number > 0) count++;
		}
		return count;
	}
}

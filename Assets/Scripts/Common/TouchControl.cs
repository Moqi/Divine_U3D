using UnityEngine;
using System.Collections;

public class TouchControl {

	public static bool IsOnTouch(Rect regionRect) {
		
		if (Input.GetMouseButton(0) && regionRect.Contains(Input.mousePosition))
			return true;

		if (Input.touches == null) return false;

		foreach (UnityEngine.Touch touch in Input.touches)
		{
			if(regionRect.Contains(touch.position))
				return true;
		}

		return false;
	}

	Vector2 GetStdPosition(Vector2 position)
	{
		Vector2 stdPosition = position;
		stdPosition.y = Screen.height - position.y;
		return stdPosition;
	}
}

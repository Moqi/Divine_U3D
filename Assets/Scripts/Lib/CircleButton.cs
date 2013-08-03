using UnityEngine;
using System.Collections;

public class CircleButton : MonoBehaviour {
	
	public float triggerRadius;
	
	Vector2 selfCenter;
	Vector2 selfWidthHeight;
	
	void Start () {
		
		GUITexture self = GetComponent(typeof(GUITexture)).guiTexture;
		selfWidthHeight[0] = self.pixelInset.width;
		selfWidthHeight[1] = self.pixelInset.height;
		
		selfCenter[0] = transform.position[0] * Screen.width;
		selfCenter[1] = transform.position[1] * Screen.height;
		
		Debug.Log("x:" + selfWidthHeight[0] + " y:" + selfWidthHeight[1]);
		Debug.Log("x:" + selfCenter[0] + " y:" + selfCenter[1]);
	}
	
	public bool IsOnTouch () {
		bool is_on_touch = false;
		for (int i=0; i<Input.touchCount; ++i)
		{
			Vector2 touch_pos = Input.GetTouch(i).position;
			float touch_dis = 0.0f;
			touch_dis = (touch_pos[0] - selfCenter[0]) * (touch_pos[0] - selfCenter[0]) + 
				(touch_pos[1] - selfCenter[1]) * (touch_pos[1] - selfCenter[1]);
			Debug.Log(touch_pos[0] + "," + touch_pos[1] + " ; " + selfCenter[0] + "," + selfCenter[1]);
			if (touch_dis < triggerRadius * triggerRadius)
			{
				is_on_touch = true;
				break;
			}
		}
		return is_on_touch;
	}
}

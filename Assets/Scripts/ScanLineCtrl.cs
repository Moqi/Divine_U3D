using UnityEngine;
using System.Collections;

public class ScanLineCtrl : MonoBehaviour {
	
	public Texture2D 	scanLineImg;
	public int			scanLineImgHeight;
	public float		scanTurnTime;
	
	public int			scanRangeTop;
	public int			scanRangeBottom;
	
	private Rect		drawRect;
	private bool		isVisible;
	
	void Start () {
		Vector3 cur_pos = transform.position;
		transform.Translate(0,-cur_pos.y,0,Space.World);
		scanTurnTime = Global.sweepOnceTime;
		scanRangeTop = Screen.height - Global.GamingZoneTop; // 800
		//scanRangeTop = 0.1f;
		scanRangeBottom = Screen.height - Global.GamingZoneBottom; // 100
		//scanRangeBottom = 0.35f;
		
		drawRect = new Rect(0, 0, Screen.width, scanLineImgHeight);
	}
	
	public void SetPosByTime(float cur_time)
	{
		float turn_time = cur_time % (2.0f * scanTurnTime);
		
		int pos_y;
		if (turn_time < scanTurnTime)
		{ // 从下向上
			pos_y = (int)((scanTurnTime - turn_time) * (scanRangeBottom - scanRangeTop) / scanTurnTime);
			pos_y += scanRangeTop - scanLineImgHeight / 2;
		}
		else
		{ // 从上向下
			pos_y = (int)((turn_time - scanTurnTime) * (scanRangeBottom - scanRangeTop) / scanTurnTime);
			pos_y += scanRangeTop - scanLineImgHeight / 2;
		}
		
		drawRect.y = pos_y;
		//transform.Translate(0.0f, new_pos_y - cur_pos_y, 0.0f, Space.World);
	}
	
	public void SetVisible (bool visible) {
		this.isVisible = visible;
		//if (visible==false) guiTexture.texture = null;
		//else guiTexture.texture = texture;
	}
	
	void OnGUI() {
		if(isVisible)
			GUI.DrawTexture(drawRect, scanLineImg);
	}
}

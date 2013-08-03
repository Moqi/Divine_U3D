using UnityEngine;
using System.Collections;

public class Skip : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 center = this.transform.position;
		Vector3 leftDown3 = (center - this.transform.localScale/2);
		Vector2 leftDown2 = new Vector2(leftDown3.x, leftDown3.y);
		
		Vector3 regionSize3 = this.transform.localScale;
		Vector2 regionSize2 = new Vector2(regionSize3.x, regionSize3.y);
		
		if (Utils.IsOnTouch_ByPercent(leftDown2, regionSize2, Input.touches)) {
			Global.Skip = true;
			Vector3 size = this.guiTexture.transform.localScale;
			size.x = 0;
			this.guiTexture.transform.localScale = size;
		}
	}
	
	
}

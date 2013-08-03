using UnityEngine;
using System.Collections;

public class SlideLineControler : MonoBehaviour {
	public Texture2D lineTex;
	private float angle;
	Rect rect;
	
	// Use this for initialization
	void Start () {
		transform.Rotate(new Vector3(0.0f,angle,0.0f));
	}
	
	// Update is called once per frame
	void Update () {
	}

	
	public void SetEndPnts(Vector2 p1,Vector2 p2)
	{
		p1 = p1 + (p2-p1).normalized*Global.TapPointVisiableRadius;
		p2 = p2 + (p1-p2).normalized*Global.TapPointVisiableRadius;
		if (p1.y<p2.y) 
		{
			Vector2 tmp = p1;
			p1 = p2;
			p2 = tmp;
		}
		Vector2 centre = (p1+p2)/2.0f;
		Vector3 newPos = new Vector3((centre.x-(Screen.width/2.0f))*10.0f,
			(centre.y-(Screen.height/2.0f))*10.0f,
			transform.position.z);
		float len = Vector2.Distance(p1,p2)*1.0f;
		//Debug.Log("len:"+len.ToString());
		transform.localScale = new Vector3(len,transform.localScale.y,transform.localScale.z);
		transform.position = newPos;
		Vector2 currDir = new Vector2(0.0f,1.0f);
		Vector2 tarDir = (p1-p2).normalized;
		//Debug.Log(tarDir.ToString());
		angle = Vector2.Angle(currDir,tarDir);
	}
}

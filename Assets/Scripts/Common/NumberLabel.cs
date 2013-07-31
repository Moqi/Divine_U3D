using UnityEngine;
using System.Collections;

public class NumberLabel : MonoBehaviour {
	
	public int			number = 0;
	public Vector2		numberPos;
	
	public bool			leadingZeros = false;
	public int			numberWide = 0;
	
	public int			digitalWidth = 10;
	public int			digitalHeight = 10;
	
	private Texture2D[]	digitalTexture = null;
	private bool		isVisible = true;
	private char[]		cNumber;
	
	void Start()
	{
		if (null == digitalTexture)
		{
			digitalTexture = new Texture2D[10];
			for (int i=0; i<10; ++i)
			{
				digitalTexture[i] = (Texture2D)Resources.Load("Image/Number/" + i, typeof(Texture2D));
			}
		}
	}
	
	public void SetNumber(int number)
	{
		if (cNumber == null || this.number != number)
		{
			cNumber = number.ToString().ToCharArray();
		}
		this.number = number;
	}
	
	public void SetPos(int pos_x, int pos_y)
	{
		numberPos.x = pos_x;
		numberPos.y = pos_y;
	}
	
	public void SetVisible(bool isVisible)
	{
		this.isVisible = isVisible;
	}
	
	void OnGUI () {
		
		if (isVisible == false)
			return;
		
		if (cNumber == null)
			return;
		
		int pos_x = (int)numberPos.x;
		int pos_y = (int)numberPos.y;
		
		if (leadingZeros)
		{
			for (int i=cNumber.Length; i<numberWide; ++i)
			{
				Rect draw_rect = new Rect(pos_x, pos_y, digitalWidth, digitalHeight);
				GUI.DrawTexture(draw_rect, digitalTexture[0]);
				pos_x += digitalWidth;
			}
		}
		else
		{
			for (int i=cNumber.Length; i<numberWide; ++i)
			{
				pos_x += digitalWidth;
			}
		}
		
		foreach (char c_digital in cNumber)
		{
			int digital = (int)c_digital - (int)'0';
			Rect draw_rect = new Rect(pos_x, pos_y, digitalWidth, digitalHeight);
			GUI.DrawTexture(draw_rect, digitalTexture[digital]);
			pos_x += digitalWidth;
		}
	}
}

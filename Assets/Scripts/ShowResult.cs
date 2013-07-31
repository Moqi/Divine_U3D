using UnityEngine;
using System.Collections;

public class ShowResult : MonoBehaviour {
	
	public Texture2D	perfectImg;
	public Texture2D	goodImg;
	public Texture2D	badImg;
	public Texture2D	missImg;
	public NumberLabel	perfectLabel;
	public NumberLabel	goodLabel;
	public NumberLabel	badLabel;
	public NumberLabel	missLabel;
	
	public float	tapCntYScale;
	public int		tapCntImgWidth;
	public int		tapCntImgHeight;
	public float	tapCntImgXScale;
	public float	tapCntLabelXScale;
	public int		tapCntDigitalWidth;
	public int		tapCntDigitalHeight;
	public int		tapCntGap;
	
	public Texture2D	scoreImg;
	public int			scoreImgWidth;
	public int			scoreImgHeight;
	public NumberLabel	scoreLabel;
	public int			scoreDigitalWidth;
	public int			scoreDigitalHeight;
	public float		scoreLabelYScale;
	
	public Texture2D	comboImg;
	public int			comboImgWidth;
	public int			comboImgHeight;
	public NumberLabel	comboLabel;
	public int			comboDigitalWidth;
	public int			comboDigitalHeight;
	public float		comboLabelYScale;
	
	public Texture2D	backButton;
	public int			backButtonWidth;
	public int			backButtonHeight;
	public float		backButtonYScale;
	
	private Rect		backButtonRect;
	
	void Start () {

		perfectLabel	= Instantiate(perfectLabel) as NumberLabel;
		goodLabel		= Instantiate(goodLabel) as NumberLabel;
		badLabel		= Instantiate(badLabel) as NumberLabel;
		missLabel		= Instantiate(missLabel) as NumberLabel;
		
		// Tap Count Label
		int labelPosX = (int)(tapCntLabelXScale * Screen.width);
		int labelPosY = (int)(tapCntYScale * Screen.height);
		labelPosY += (tapCntImgHeight - tapCntDigitalHeight)/2;
		InitTapCntLabel(perfectLabel, Global.perfectCnt, labelPosX, labelPosY);
		labelPosY += tapCntDigitalHeight + tapCntGap;
		labelPosY += (tapCntImgHeight - tapCntDigitalHeight);
		InitTapCntLabel(goodLabel, Global.goodCnt, labelPosX, labelPosY);
		labelPosY += tapCntDigitalHeight + tapCntGap;
		labelPosY += (tapCntImgHeight - tapCntDigitalHeight);
		InitTapCntLabel(badLabel, Global.badCnt, labelPosX, labelPosY);
		labelPosY += tapCntDigitalHeight + tapCntGap;
		labelPosY += (tapCntImgHeight - tapCntDigitalHeight);
		InitTapCntLabel(missLabel, Global.missCnt, labelPosX, labelPosY);
		
		
		scoreLabel = Instantiate(scoreLabel) as NumberLabel;
		comboLabel = Instantiate(comboLabel) as NumberLabel;
		
		// Score Label
		scoreLabel.leadingZeros = false;
		scoreLabel.numberWide = 0;
		scoreLabel.digitalWidth = scoreDigitalWidth;
		scoreLabel.digitalHeight = scoreDigitalHeight;
		scoreLabel.SetNumber(Global.curScore);
		int scoreDigitalCnt = 10;// TouchControl.GetDigitNumber(Global.curScore);
		scoreLabel.SetPos((Screen.width - scoreDigitalCnt * scoreDigitalWidth)/2,
							(int)(Screen.height * scoreLabelYScale + scoreImgHeight));
		
		// Max Combo Label
		comboLabel.leadingZeros = false;
		comboLabel.numberWide = 0;
		comboLabel.SetNumber(Global.curMaxCombo);
		comboLabel.digitalWidth = comboDigitalWidth;
		comboLabel.digitalHeight = comboDigitalHeight;
		comboLabel.SetNumber(Global.curMaxCombo);
		int comboDigitalCnt = 10;// TouchControl.GetDigitNumber(Global.curMaxCombo);
		comboLabel.SetPos((Screen.width - comboDigitalCnt * comboDigitalWidth)/2,
							(int)(Screen.height * comboLabelYScale + comboImgHeight));
		
		
		backButtonRect = new Rect((Screen.width - backButtonWidth)/2, Screen.height * backButtonYScale,
									backButtonWidth, backButtonHeight);
		
		Global.SaveMaxScore();
		Global.CleanUp();
	}
	
	// Update is called once per frame
	void Update () {
		if (TouchControl.IsOnTouch(backButtonRect))
		{
			Application.LoadLevel("List");
		}
	}
	
	void OnGUI() {
		Rect drawRect = new Rect(0f, 0f, 0f, 0f);
		drawRect.x = tapCntImgXScale * Screen.width;
		drawRect.y = tapCntYScale * Screen.height;
		
		drawRect.width = tapCntImgWidth;
		drawRect.height = tapCntImgHeight;
		
		GUI.DrawTexture(drawRect, perfectImg);
		drawRect.y += tapCntImgHeight + tapCntGap;
		GUI.DrawTexture(drawRect, goodImg);
		drawRect.y += tapCntImgHeight + tapCntGap;
		GUI.DrawTexture(drawRect, badImg);
		drawRect.y += tapCntImgHeight + tapCntGap;
		GUI.DrawTexture(drawRect, missImg);
		
		drawRect.x = (Screen.width - scoreImgWidth) / 2;
		drawRect.y = scoreLabelYScale * Screen.height;
		drawRect.width = scoreImgWidth;
		drawRect.height = scoreImgHeight;
		GUI.DrawTexture(drawRect, scoreImg);
		
		drawRect.x = (Screen.width - comboImgWidth) / 2;
		drawRect.y = comboLabelYScale * Screen.height;
		drawRect.width = comboImgWidth;
		drawRect.height = comboImgHeight;
		GUI.DrawTexture(drawRect, comboImg);
		
		GUI.DrawTexture(backButtonRect, backButton);
	}
	
	void InitTapCntLabel(NumberLabel numberLabel, int cntNumber, int posX, int posY)
	{
		numberLabel.numberWide = 3;
		numberLabel.leadingZeros = false;
		numberLabel.digitalWidth = tapCntDigitalWidth;
		numberLabel.digitalHeight = tapCntDigitalHeight;
		
		numberLabel.SetNumber(cntNumber);
		numberLabel.SetPos(posX, posY);
		
		Debug.Log(posX + " : " + posY);
	}
}

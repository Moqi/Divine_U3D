using UnityEngine;
using System.Collections;

public class ScoreLabel : MonoBehaviour {
	
	public NumberLabel	scoreLabel;
	public NumberLabel	comboLabel;
	
	public int			scoreLabelWide	= 6;
	public int			scoreWidth		= 50;
	public int			scoreHeight		= 50;
	
	public int			comboLabelWide	= 0;
	public int			comboWidth		= 40;
	public int			comboHeight		= 40;
	
	public Texture2D	comboImgTex;
	public Rect			comboImgRect = new Rect(0.0f, 0.0f, 150.0f, 40.0f);
	
	public Texture2D	wingImgTex;
	public int			wingImgWidth = 100;
	public int			wingImgHeight = 100;
	
	private Rect		wingImgRectLeft;
	private Rect		wingImgRectRight;
	
	private int			curScore;
	private int			curCombo;
	private bool		isVisible;
	
	void Start () {
		scoreLabel = Instantiate(scoreLabel) as NumberLabel;
		comboLabel = Instantiate(comboLabel) as NumberLabel;
		
		int screen_width = Screen.width;
		
		// scoreLabel
		int score_width = scoreWidth * scoreLabelWide;
		scoreLabel.numberWide = scoreLabelWide;
		scoreLabel.digitalWidth = scoreWidth;
		scoreLabel.digitalHeight = scoreHeight;
		
		scoreLabel.SetNumber(0);
		scoreLabel.SetPos((screen_width - score_width)/2, 0);
		
		// comboLabel
		int combo_width = comboWidth * comboLabelWide + (int)comboImgRect.width;
		
		comboLabel.leadingZeros = false;
		comboLabel.numberWide = comboLabelWide;
		comboLabel.digitalWidth = comboWidth;
		comboLabel.digitalHeight = comboHeight;
		
		comboLabel.SetVisible(false);
		comboLabel.SetNumber(0);
		comboLabel.SetPos((screen_width - combo_width)/2 + (int)comboImgRect.width, scoreLabel.digitalHeight);
		//scoreLabel.SetVisible(false);
		
		comboImgRect.x = (screen_width - combo_width)/2;
		comboImgRect.y = scoreLabel.digitalHeight;
		
		// wingImage
		wingImgRectLeft.x = wingImgWidth;
		wingImgRectLeft.y = 0;
		wingImgRectLeft.width = -wingImgWidth;
		wingImgRectLeft.height = wingImgHeight;
		
		wingImgRectRight.x = screen_width - wingImgWidth;
		wingImgRectRight.y = 0;
		wingImgRectRight.width = wingImgWidth;
		wingImgRectRight.height = wingImgHeight;
	}
	
	void Update () {
		if (!isVisible)
		{
			scoreLabel.SetVisible(false);
			comboLabel.SetVisible(false);
			return;
		}
		
		curScore = ScoreCalculator.GetScore();
		scoreLabel.SetNumber(curScore);
		
		curCombo = ScoreCalculator.GetCurrCombo();
		if (curCombo == 1 || curCombo == 0)
			comboLabel.SetVisible(false);
		else
			comboLabel.SetVisible(true);
		comboLabel.SetNumber(curCombo);
	}
	
	void OnGUI () {
		if (!isVisible)
			return;
		
		if (!(curCombo == 1 || curCombo == 0))
			GUI.DrawTexture(comboImgRect, comboImgTex);
		
		GUI.DrawTexture(wingImgRectLeft, wingImgTex);
		GUI.DrawTexture(wingImgRectRight, wingImgTex);
	}
	
	public void SetVisible(bool isVisible)
	{
		this.isVisible = isVisible;
	}
}

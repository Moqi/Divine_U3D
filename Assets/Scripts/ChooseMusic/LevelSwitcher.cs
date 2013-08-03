using UnityEngine;
using System.Collections;

public class LevelSwitcher : MonoBehaviour {
	
	public Texture2D	diskImg;
	public int			diskImgWidth = 220;
	public int			diskImgHeight = 220;
	public float		diskImgYScale = 0.3f;
	private Rect		diskImgRect;
	
	public MusicPlayer	musicPlayer;
	
	public Texture2D	leftArrow;
	public Texture2D	rightArrow;
	public int			arrowImgWidth = 100;
	public int			arrowImgHeight = 100;
	public int			arrowImgX = 20;
	public float		arrowImgYScale = 0.8f;
	private Rect		leftArrowImgRect;
	private Rect		rightArrowImgRect;
	
	public Texture2D	maxScoreImg;
	public int			maxScoreImgWidth;
	public int			maxScoreImgHeight;
	public float		maxScoreImgYScale;
	public NumberLabel	maxScoreLabel;
	public int			labelDigiWidth = 40;
	public int			labelDigiHeight = 50;
	private Rect		maxScoreImgRect;
	
	public Texture2D	loadingImg;
	public int			loadingImgWidth;
	public int			loadingImgHeight;
	
	UnityEngine.Touch	beginPos;
	UnityEngine.Touch	endPos;
	
	int	levelCount = 0;
	int curLevelIndex = 0;
	LevelInfo curLevelInfo;
	
	bool inTouchMove = false;
	bool isInLoading = false;
	
	// Use this for initialization
	void Start () {
		diskImgRect			= new Rect((Screen.width - diskImgWidth)/2, diskImgYScale * Screen.height,
								diskImgWidth, diskImgHeight);
		leftArrowImgRect	= new Rect(arrowImgX, arrowImgYScale * Screen.height,
								arrowImgWidth, arrowImgHeight);
		rightArrowImgRect	= new Rect(Screen.width - arrowImgX - arrowImgWidth, arrowImgYScale * Screen.height,
								arrowImgWidth, arrowImgHeight);
		maxScoreImgRect		= new Rect((Screen.width - maxScoreImgWidth)/2, maxScoreImgYScale * Screen.height,
								maxScoreImgWidth, maxScoreImgHeight);
		
		Debug.Log(leftArrowImgRect);
		Debug.Log(rightArrowImgRect);
		
		Global.GetLevelInfos();
		levelCount = Global.GetLevelCount();
		musicPlayer = Instantiate(musicPlayer) as MusicPlayer;
		musicPlayer.SetLoop(true);
		
		maxScoreLabel = Instantiate(maxScoreLabel) as NumberLabel;
		maxScoreLabel.leadingZeros = false;
		maxScoreLabel.numberWide = 0;
		maxScoreLabel.digitalWidth = labelDigiWidth;
		maxScoreLabel.digitalHeight = labelDigiHeight;
		
		SwitchLevelPreview();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (isInLoading)
			return;
		//Debug.Log("Music Loop:: " + musicPlayer.audioSource.loop);
		
		// 滑动切换
		foreach (UnityEngine.Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				beginPos = touch;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				endPos = touch;
				if (inTouchMove && (endPos.position.x - beginPos.position.x) < (-0.3f*Screen.width))
				{
					SwitchPreviousSong();
				}
				else if (inTouchMove && (endPos.position.x - beginPos.position.x) > (0.3f*Screen.width))
				{
					SwitchNextSong();
				}
				inTouchMove = false;
				break;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				inTouchMove = true;
			}
		}
		
		// 按钮切换
		if (!inTouchMove)
		{
			if (Input.touches.Length > 0)
			{
				if(Utils.IsOnTouch_ByRect(diskImgRect, Input.touches))
				{
					EnterGame();
				}
				
				if(Utils.IsOnTouch_ByRect(leftArrowImgRect, Input.touches))
				{
					SwitchPreviousSong();
				}
				
				if(Utils.IsOnTouch_ByRect(rightArrowImgRect, Input.touches))
				{
					SwitchNextSong();
				}
			}
		}
	}
	
	void OnGUI()
	{
		if (isInLoading)
		{
			Rect loadingImgRect = new Rect((Screen.width - loadingImgWidth)/2, (Screen.height - loadingImgHeight)/2,
											loadingImgWidth, loadingImgHeight);
			GUI.DrawTexture(loadingImgRect, loadingImg);
			return;
		}
		
		GUI.DrawTexture(diskImgRect, diskImg);
		GUI.DrawTexture(leftArrowImgRect, leftArrow);
		GUI.DrawTexture(rightArrowImgRect, rightArrow);
		GUI.DrawTexture(maxScoreImgRect, maxScoreImg);
	}
	
	void SwitchPreviousSong()
	{
		curLevelIndex--;
		if (curLevelIndex < 0)
			curLevelIndex = levelCount - 1;
		Debug.Log("next" + curLevelIndex);
		SwitchLevelPreview();
	}
	
	void SwitchNextSong()
	{
		Debug.Log(levelCount);
		curLevelIndex++;
		if (curLevelIndex >= levelCount)
			curLevelIndex = 0;
		Debug.Log("next" + curLevelIndex);
		SwitchLevelPreview();
	}
	
	void SwitchLevelPreview()
	{
		curLevelInfo = Global.GetLevelInfo(curLevelIndex);
		diskImg = (Texture2D)Resources.Load("Image/ListScene/" + curLevelInfo.imageFile);
		
		musicPlayer.Stop();
		musicPlayer.Load("Music/" + curLevelInfo.songPreview);
		musicPlayer.Play();
		
		int maxScore = 0;
		if (PlayerPrefs.HasKey("MaxScore:Level" + curLevelIndex))
			maxScore = PlayerPrefs.GetInt("MaxScore:Level" + curLevelIndex);
		maxScoreLabel.SetNumber(maxScore);
		int labelDigitalCnt = Utils.GetDigitNumber(maxScore);
		maxScoreLabel.SetPos((Screen.width - (labelDigitalCnt * labelDigiWidth))/2,
							(int)(Screen.height * (1.0f - maxScoreImgYScale)) + maxScoreImgHeight);
	}
	
	void EnterGame()
	{
		isInLoading = true;
		Global.SetCurrentLevelIndex(curLevelIndex);
		Global.CleanUp();
		maxScoreLabel.SetVisible(false);
		musicPlayer.Stop();
		Application.LoadLevel("Game");
	}
}

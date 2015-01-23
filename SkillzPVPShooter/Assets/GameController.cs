using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

	// Use this for initialization
	public float maxGameTime;
	private float gameTime;
	private bool isGame;
	private int kills;

	public Text timerText;
	public Text killsText;
	public float scaleSpeed;

	private bool showStartAnimation = true;
	private bool showEndAnimation;
	private bool showLoadingMessage = false;

	private bool timeIsOver;
	private float startAnimationTimer;
	private float endAnimationTimer;
	private float showLoadingMessageTimer;
	public string tipsStatus;

	public static string SHOW_READY_MESSAGE = "showReadyMessage";
	public static string SHOW_STEADY_MESSAGE = "showSteadyMessage";
	public static string SHOW_GO_MESSAGE = "showGoMessage";
	public static string NO_MESSAGE = "noMessage";
	private bool ShowReadyFlag = false;
	private bool ShowSteadyFlag = false;
	private bool ShowGoFlag = false;

	public static string SHOW_TIME_IS_OVER_MESSAGE = "timeIsOver";

	public GameObject killsTextBlock;
	public GameObject timerTextBlock;
	public GameObject timeIsOverTextBlock;

	public GameObject loadingBG;
	public GameObject loadingText;

	// pause screen elements:
	public GameObject pauseBG;
	public GameObject pauseText;
	public GameObject stopButton;
	public GameObject continueButton;
	public GameObject pauseButton;
	private bool isPause = false;
	public float oldTimeScale;

	void OnApplicationPause(bool pause) {
		if(pause)
		{
			StartPause();
		}
	}
	
	void Awake(){
		HidePauseMenu ();
		ResetLevel ();	
	}

	void Start () {

	}	

	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Debug.Log("RESTART");
			ResetLevel();
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			StartPause();
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			StopPause();
		}

		if (showStartAnimation) {
			startAnimationTimer += Time.deltaTime;
			if (startAnimationTimer > 3){
				isGame = true;
				killsTextBlock.SetActive (true);
				timerTextBlock.SetActive (true);
				pauseButton.SetActive(true);
				showStartAnimation = false;
			}
		} 

		if (isGame) {
			gameTime -= Time.deltaTime;
			UpdateTimerText();	
			if (gameTime<=0){
				timerTextBlock.SetActive(false);
				isGame = false;
				showEndAnimation = true;
				timeIsOver = true;
			}
		}

		if (showEndAnimation) {
			if (endAnimationTimer == 0) {
				tipsStatus = SHOW_TIME_IS_OVER_MESSAGE;
			}
			if (tipsStatus == SHOW_TIME_IS_OVER_MESSAGE){
				timeIsOverTextBlock.SetActive (true);
			}
			endAnimationTimer+= Time.deltaTime;
			if (endAnimationTimer>1){
				showEndAnimation = false;
				if (Skillz.tournamentIsInProgress()) {
					Skillz.displayTournamentResultsWithScore(kills);
				} 
				showLoadingMessage = true;

			}
		}

		if (showLoadingMessage) {
			showLoadingMessageTimer += Time.deltaTime;
			if (showLoadingMessageTimer > 4f){
				showLoadingMessage = false;
				loadingBG.SetActive(true);
				loadingText.SetActive(true);
			}
		}
	}

	void UpdateTimerText(){
		timerText.text = "Time:" + (int)gameTime;
	}

	public void ChangeKillsCounter(int delta){
		// animation for text
		kills += delta;
		killsText.text = "Kills:" + kills;

		if (Skillz.tournamentIsInProgress()) {
			// update the current score
			Skillz.updatePlayersCurrentScore(kills);
		}
	}

	public bool IsGame(){
		return isGame;
	}

	public void SetShowStartAnimation(bool _state){
		showStartAnimation = _state;
	}

	public void ResetLevel(){
		Debug.Log("ResetLevel()");
		killsTextBlock.transform.localScale = new Vector3 (1f, 1f, 1f);
		timerTextBlock.transform.localScale = new Vector3 (1f, 1f, 1f);
		timeIsOverTextBlock.transform.localScale = new Vector3 (1f, 1f, 1f);
		loadingBG.SetActive (false);
		loadingText.SetActive (false);
		killsTextBlock.SetActive(false);
		timerTextBlock.SetActive(false);
		pauseButton.SetActive (false);
		timeIsOverTextBlock.SetActive (false);
		showStartAnimation = true;
		showEndAnimation = false;
		isGame = false;
		startAnimationTimer = 0;
		endAnimationTimer = 0;
		kills = 0;
		killsText.text = "Kills:" + kills;
		gameTime = maxGameTime;
		UpdateTimerText();
	}

	public void StartPause(){
		isPause = true;
		oldTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		ShowPauseMenu ();
	}

	public void StopPause(){
		isPause = false;
		Time.timeScale = 1f;
		HidePauseMenu ();
	}

	public void ShowPauseMenu(){
		pauseBG.SetActive (true);
		pauseText.SetActive (true);
		stopButton.SetActive (true);
		continueButton.SetActive (true);
		pauseButton.SetActive (false);
	}

	public void HidePauseMenu(){
		pauseBG.SetActive (false);
		pauseText.SetActive (false);
		stopButton.SetActive (false);
		continueButton.SetActive (false);
		pauseButton.SetActive (true);
	}

	public void StopGame(){
		// abort current game
		if (Skillz.tournamentIsInProgress()) {
			Skillz.notifyPlayerAbortWithCompletion();
		}
	}

	public bool IsPause(){
		return isPause;
	}
}

/*
 * Timing:
 * 14 Jan. 15-00 - 15-50 work with gamecontoller. GUI (Timer, score), additional prestart scene, switching to game mode.
 * 		   15-50 - 16-20 project customization and few steps for integration
 * 		   16-20 - 17-00 Ready - steady - go messages, adding fonts, animations
 * 		   17-00 - 17-55 - frags counting, stop game session condition, backward time counting
 * 
 * 15 Jan. 13-00 - 13-15 making iOS build and Xcode project, adding Skillz framework to Xcode
 * 		   13-15 - 13-47 studying "iPhone 6 and 6+ native resolution compatibility" article, optimization for Retina HD 5.5 and Retina HD 4.7
 * 	       13-47 - 14-44 adding reauired linker flags
 * 						 enabling modules and automatic framework linking
 * 						 configuring game orientation
 * 						 adding the skillz run script
 * 		   14-44 - 15-10     studying core implementation article
 * 
 * 		   15-10 - 15-25 adding background, help button and tournament button on menu stage
 *         15-25 - 15-55 adding skillz, building and testing application
 * 		   15-55 - 16-55 there is some problem with landscape orientation (width). Fixing. Working with github skillz landscape-oriented demo project.
 * 		   16-55 - 17-30 making changes on loading gamelevel, testing end of game session and result window, changing fontsize and game session time, new testing.
 * 		   17-30 - 18-00 there is delay before "play" button click and loading gameScene. Making preloading. Switching off start menu.
 * 							switching off default SniperKit GUI tips
 * 
 * 16 Jan  14-06 - 15-23 programming of replay opportunity (reset timer, kills counter etc.), upgrading GUI for different screen sizes, testing
 * 		   15-23 - 16-06 testing PVP game one versus one on 2 devices
 * 		   16-06 - 17-00 app shows "loading" image during skillz loaded after app start.
 * 		   17-00 - 17-40 testing app on different devices. There is trouble with Skillz GUI width on iPhone 4 (iOS 7) and iPod. 5 (iOS 7).		 
 * 		   17-40 - 18-47 memory optimazation.
 * 			
 * 19 Jan  12-40 - 13-50 pause screen: making buttons (pause, stop, continue), positioning, pause on/off scripts
 * 		   14-45 - 17-00 adding pause functionality on mobile GUI (touchable interface), stoppping all game processes (units movement, shooting etc.), working with Sniper Kit code.
 * 		   17-00 - 17-45 Aborting game, restorng game, making pause button enebled just during game session (not ready-steady-go part)
 * 		   17-45 - 18-05 Standart SniperKIT shows always. Switching it off when game displays pause screen.	
 * 
 * 20 Jan
 * 14.40 - 15.33 - now app shows pause screen when it swtching to background modes
 * 15-33 - 16-15 - app stay at pause mode when player returns. Switching GUI off when game ends.
 * 16-15 - 18-00 - memory optimization
 * 
 * 21 Jan
 * 14-20 - 16-00 - GUI animator
 * 16-00 - 18-10 - clean up the code
 * 
 * 22 Jan 
 * 14-45 - 00-00 Adding icon
 * 
 * 
 * 			Logo needed			
 * 			gui must be swiched off after game end
 * 			Sounds: gameEnd Sfx, ost.
 * 
 * 
 * 
 *  * Timing:
 * 14 Jan. 
 * 1h - work with gamecontoller. GUI (Timer, score), additional prestart scene, switching to game mode.
 * 		   0.5h - project customization and few steps of integration
 * 		   0.5h - "ready - steady - go" messages, adding fonts, animations
 * 		   1h - frags counting, stop game session condition, backward time counting
 * 
 * 15 Jan. 
 * 0.5h - making iOS build and Xcode project, adding Skillz framework to Xcode, adding background, help button and tournament button on menu stage
 * 		   0.5h - studying "iPhone 6 and 6+ native resolution compatibility" article, optimization for Retina HD 5.5 and Retina HD 4.7
 * 	       1h - adding required linker flags
 * 						 enabling modules and automatic framework linking
 * 						 configuring game orientation
 * 						 adding the skillz run script
 * 		   0.5h - studying core implementation article 		   
 *         0.5h - adding skillz, building and testing application
 * 		   1h - there is some problem with landscape orientation (width). Trying to fix. Working with github skillz landscape-oriented demo project.
 * 		   0.5h making changes on loading gamelevel, testing end of game session window and result window, changing fontsize and game session time, new testing.
 * 		   1h - there is delay after "play" button click and before gameScene loading. Making preloading. Switching off start menu.
 * 							switching off default SniperKit GUI tips
 * 
 * 16 Jan  
 * 1.5h - programming of replay opportunity (reset timer, kills counter etc.), upgrading GUI for different screen sizes, testing
 * 		   0.5h - testing PVP game one versus one on 2 devices
 * 		   1h - app shows "loading" image during skillz loaded after app start. Fixing.
 * 		   0.5h - testing app on different devices. There is trouble with Skillz GUI width on iPhone 4 (iOS 7) and iPod. 5 (iOS 7).		 
 * 		   1.5h - memory optimazation.
 * 			
 * 19 Jan  
 * 1h - pause screen: making buttons (pause, stop, continue), positioning, pause on/off scripts
 * 		   2.5h - adding pause functionality on mobile GUI (touchable interface), stopping all game processes (units movement, shooting etc.), working with Sniper Kit code.
 * 		   1h - Aborting game, restoring game, making pause button enabled just during game session (not ready-steady-go part)
 * 		   0.5h - Standart SniperKIT shows always. Switching it off when game displays pause screen.
 * 20 Jan
 * 14.40 - 15.33 - now app shows pause screen when it swtching to background modes
 * 
 * 
 * 
 * /
*/

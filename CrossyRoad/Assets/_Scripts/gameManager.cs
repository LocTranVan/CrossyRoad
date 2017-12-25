using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Facebook.Unity;
public class gameManager : MonoBehaviour {

	public static gameManager intance;
	private int highScore;
	public GameObject EndPanel, btnPause;
	public GameObject btnSaveMe, btnExit;
	// PanelUI
	public GameObject ChoosePlayerPanel, PanelResetGame,
		imageCrossyRoad, PanelInGame, PanelPause;
	public GameObject PanelStartGame, PanelSetting;

	private GameObject player, camera;
	public GameObject canvas;
	private GameObject currentCharater;
	public GameObject defautCharacter;
	public GameObject pet;
	public GameObject PanelTopScore;
	public Material materialSnowFlower, materialNormal;

	public GameObject panelAchivements;
	private int _score, _coins = 30;
	public GameObject[] settings;
	public GameObject txtNamePost, txtLoading;
	private bool reloadGame;
	private enum Maps
	{
		winter, summer
	}
	private Maps map = Maps.summer;
	private void Awake()
	{
		if (intance == null)
			intance = this;
		else if (intance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
	void Start () {
		currentCharater = defautCharacter;
	//	PlayerPrefs.DeleteAll();
		highScore = PlayerPrefs.GetInt("highscore");
		Debug.Log(highScore);
	    _coins = PlayerPrefs.GetInt("Coin");
		PanelInGame.GetComponent<ScoreUI>().setNumberCoinAndChangeSprite(_coins);
	}

	public void IntancePet()
	{
		player = GameObject.Find("Player");
		Instantiate(pet, player.transform.position, Quaternion.identity);
	}
	// Update is called once per frame
	void Update () {
	}
	public void checkeAllCharacter()
	{

	}
	public void Setting()
	{
		PanelSetting.SetActive(true);
	}
	public void EndGame()
	{
		if (!reloadGame)
		{
			EndPanel.SetActive(true);
			btnPause.SetActive(false);
			if (FB.IsLoggedIn &&
				GameStateManager.HighScore < _score)
			{
				FBShare.PostScore(_score);
				GameStateManager.HighScore = _score;
				RedrawPanelTopScore();
			}else
			{
				if(_score > PlayerPrefs.GetInt("highscore"))
				{
					PlayerPrefs.SetInt("highscore", highScore);
					RedrawPanelTopScore();
				}

			}
		
			reloadGame = true;

			//player = GameObject.Find("Player");
			//Debug.Log(((RectTransform)player.transform).anchoredPosition);
		}
	}
	public void SaveMe()
	{
		GameObject StupidDog = GameObject.FindGameObjectWithTag("Lion");
		if(StupidDog != null)
		{
			Enemy ni = StupidDog.GetComponent<Enemy>();
			ni.ResetLife();
		}
		btnSaveMe.SetActive(false);
	}
	public void setActiveTextSuccess()
	{
		txtLoading.SetActive(false);
		txtNamePost.SetActive(true);
		DOTweenAnimation dot = txtNamePost.GetComponentInChildren<DOTweenAnimation>();
		dot.DORestartById("text");
	}
	public void setActiveTextLoading()
	{
		txtLoading.SetActive(true);
	//	DOTweenAnimation dot = txtNamePost.GetComponentInChildren<DOTweenAnimation>();
		//dot.DORestartById("text");

	}
	
	public void RedrawPanelTopScore()
	{
		PanelTopScore.SetActive(false);
		PanelTopScore.SetActive(true);
	}
	public void startUIPlayGame()
	{
		PanelResetGame.SetActive(false);
		PanelStartGame.SetActive(true);
	}
	public void setPlayer(GameObject character)
	{
		ChoosePlayerPanel.SetActive(false);
		currentCharater = character;
		//init();
		player = GameObject.Find("Player");
		if (player != null)
		{
			//Debug.Log(currentCharater.gameObject.name);
			if (currentCharater != null)
			{
				player.GetComponentInChildren<MeshFilter>().mesh = currentCharater.GetComponentInChildren<MeshFilter>().mesh;

				player.GetComponentInChildren<Renderer>().material = currentCharater.GetComponentInChildren<Renderer>().material;
				Market mark = currentCharater.GetComponent<Market>();
				player.GetComponent<Character>().setAudioDead(mark.getAudio());
				player.GetComponent<Character>().setNormalAudio(mark.getAudioNormal());
			}
		}

	}
	public void continueGame()
	{
		player = GameObject.Find("Player");
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		player.GetComponent<Character>().isPause = false;
		camera.GetComponent<CameraMovement>().setPause(false);
	}
	public void init()
	{
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		if (canvas != null)
		{
			Debug.Log("set Camera");
			canvas.GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();
		}
	
		foreach(GameObject key in settings)
		{
			UIManager keys = key.GetComponent<UIManager>();
			if (!keys.getON())
			{
				keys.setON("on");
				switch (keys.name)
				{
					case "btMute":
						keys.setMute();
						break;
					case "btNoShadows":
						keys.turnOffShadow();
						break;
					case "btRotationLock":
						keys.LockRotation();
						break;
					case "BtMore":
						keys.setON("");
						keys.setMore();
						Debug.Log("set More");
						
					//	keys.ChangeSpriteWhenPress();
						break;
					default:
						break;

				}
			}
		}
	}
	

	public bool buyCharacter(int price)
	{
		if (_coins >= price)
		{
			_coins -= price;
			PanelInGame.GetComponent<ScoreUI>().setNumberCoinAndChangeSprite(_coins);
			ScoreUI scoreUI = ChoosePlayerPanel.GetComponent<ScoreUI>();
			scoreUI.setNumberCoinAndChangeSprite(_coins);
			return true;
		}
		return false;
	}
	public void setPause()
	{
		PanelPause.SetActive(true);
		Time.timeScale = 0;
		
	}
	public void setContinueGame()
	{
		PanelPause.SetActive(false);
		Time.timeScale = 1;
	}
	public Material GetMaterial()
	{
		if(map == Maps.winter)
			return materialSnowFlower;
		return null;
	}
	public void setMaps()
	{
		if (map == Maps.winter)
			map = Maps.summer;
		else	
			map = Maps.winter;
		Play();
	}
	public GameObject getCharacter()
	{
		return currentCharater;
	}
	public void ChooseCharacter()
	{
		ChoosePlayerPanel.SetActive(true);
		ScoreUI scoreUI = ChoosePlayerPanel.GetComponent<ScoreUI>();
		scoreUI.setNumberCoinAndChangeSprite(_coins);
		//Debug.Log("coin" + _coins);
	}
	public void ActiveBt()
	{
		btnSaveMe.SetActive(true);
	}
	public void Play()
	{
		EndPanel.SetActive(false);
		PanelResetGame.SetActive(true);
		btnPause.SetActive(true);
		PanelResetGame.GetComponent<DOTweenAnimation>().DORestartById("fade");
		imageCrossyRoad.GetComponent<DOTweenAnimation>().DORestart();
	}
	public void LoadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		_score = 0;
		PanelInGame.GetComponent<ScoreUI>().setScoreAndChangeSprite(_score);

		Physics.IgnoreLayerCollision(9, 10, false);
		Physics.IgnoreLayerCollision(8, 10, false);
		Physics.IgnoreLayerCollision(9, 12, false);
		Physics.IgnoreLayerCollision(8, 12, false);
	
		reloadGame = false;
		btnExit.SetActive(true);
		Invoke("init", 1f);
	}
	public void setScore()
	{
		_score ++;
		if (FBLogin.HavePublishActions)
		{
			setAchivements(_score);
		}
		if (_score > highScore)
			PlayerPrefs.SetInt("highscore", _score);
		PanelInGame.GetComponent<ScoreUI>().setScoreAndChangeSprite(_score);
	}
	public void setAchivements(int score)
	{
		DOTweenAnimation tl = panelAchivements.GetComponent<DOTweenAnimation>();
		//tl.DORestartById("Move");
		switch (score)
		{
			case 10:
				if (GameStateManager.HighScore <= 10)
				{
					UIManager uIManager = panelAchivements.GetComponent<UIManager>();
					uIManager.setAchivements("Achivement 1", "+ 10 Coins");
					tl.DORestartById("Move");
					_coins += 9;
					setCoin();
					GetComponent<SoundManager>().soudAchivements();
					FBAchievements.GiveAchievement("https://loctranvan-89.herokuapp.com/index.htm");
				}
				break;
			case 20:
				if (GameStateManager.HighScore <= 20)
				{
					UIManager uIManager = panelAchivements.GetComponent<UIManager>();
					uIManager.setAchivements("Achivement 2", "+ 20 Coins");
					_coins += 19;
					setCoin();
					tl.DORestartById("Move");
					GetComponent<SoundManager>().soudAchivements();
					FBAchievements.GiveAchievement("https://achivements2.herokuapp.com/index.htm");
				}
				break;
		}
	}
	public void setCoin()
	{
		Debug.Log("Collect Coin"+ _coins);
		_coins ++;
		PlayerPrefs.SetInt("Coin", _coins);
		PanelInGame.GetComponent<ScoreUI>().setNumberCoinAndChangeSprite(_coins);
	}
	public int getScore()
	{
		return _score;
	}
	public int getCoin()
	{
		return _coins;
	}
}

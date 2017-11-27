using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class gameManager : MonoBehaviour {

	public static gameManager intance;
	public GameObject EndPanel;
	public GameObject btnSaveMe;
	public GameObject ChoosePlayerPanel, PanelResetGame, imageCrossyRoad, PanelInGame;
	public GameObject PanelStartGame;
	private GameObject player, camera;
	public GameObject canvas;
	private GameObject currentCharater;
	public GameObject defautCharacter;

	public Material materialSnowFlower, materialNormal;

	private GameObject pet, YardPet;
	private int _score = 10, _coins;
	

	private bool reloadGame;

	private enum Maps
	{
		winter, summer
	}
	private Maps map = Maps.summer;
	// Use this for initialization
	void Start () {
		if (intance == null)
			intance = this;
		else if(intance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	
		currentCharater = defautCharacter;
	
	}

	public void IntancePet()
	{
		pet = GameObject.FindGameObjectWithTag("Lion");
		YardPet = GameObject.FindGameObjectWithTag("YardPet");
		Debug.Log("pet");
		if(pet != null && YardPet != null)
		{
			pet.SetActive(true);
			YardPet.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void checkeAllCharacter()
	{

	}
	
	public void EndGame()
	{
		if (!reloadGame)
		{
			EndPanel.SetActive(true);
			reloadGame = true;
		}
	}
	public void SaveMe()
	{
		GameObject StupidDog = GameObject.Find("Lion");
		if(StupidDog != null)
		{
			StupidDog.GetComponent<Enemy>().ResetLife();
		}
		btnSaveMe.SetActive(false);
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
	}
	public void ActiveBt()
	{
		btnSaveMe.SetActive(true);
	}
	public void Play()
	{
		

		EndPanel.SetActive(false);
		PanelResetGame.SetActive(true);
		//PanelResetGame.GetComponent<DOTweenAnimation>().DORestart();
		PanelResetGame.GetComponent<DOTweenAnimation>().DORestartById("fade");
		imageCrossyRoad.GetComponent<DOTweenAnimation>().DORestart();



	
	//	LoadScene();
		//Invoke("LoadScene",1f);
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
		Invoke("init", 1f);
	}
	public void setScore()
	{
		_score ++;
		PanelInGame.GetComponent<ScoreUI>().setScoreAndChangeSprite(_score);
	}
	public void setCoin()
	{
		_coins ++;
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

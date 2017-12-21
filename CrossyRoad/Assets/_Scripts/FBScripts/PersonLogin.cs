using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PersonLogin : MonoBehaviour {
	public GameObject Image;
	public GameObject TxtName;
	public GameObject TxtScore;
	public GameObject TxtRank;

	private Image imageAvatar;
	private Text txtName;
	private Text txtScore;
	private Text txtRank;
	// Use this for initialization
	void Start () {
		
	}
	private void OnEnable()
	{
	//	imageAvatar = Image.GetComponent<Image>();
		//txtName = TxtName.GetComponent<Text>();
	//	txtScore = TxtScore.GetComponent<Text>();
		//txtRank = TxtRank.GetComponent<Text>();

	
	//	txtScore.text = GameStateManager.HighScore.ToString();

	}
	public void setImage(Texture texture)
	{
		RawImage image = Image.GetComponent<RawImage>();
		image.texture = texture;
	}
	public void setTxtName(string name)
	{
		txtName = TxtName.GetComponent<Text>();
		txtName.text = name;
	}
	public void setTxtScore(string score)
	{
		Text txtScore = TxtScore.GetComponent<Text>();
		txtScore.text = "High Score " + score;
	}
	public void setStandings(string standings, string count)
	{
		txtRank = TxtRank.GetComponent<Text>();
		txtRank.text = "Standings " + standings + "/" + count;
	}
	// Update is called once per frame
	void Update () {
		
	}
}

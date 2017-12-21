using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Achivements : MonoBehaviour {
	public Text txtTitle;
	public Text txtDescription;
	public Text txtCoin;
	public Image imageAvatar;

	public Sprite spriteDone, spriteNotDone;
	public string ID;
	// Use this for initialization
	
	void Start () {
		
	}
	private void OnEnable()
	{
			
	}
	public void setImageDone()
	{
		imageAvatar.sprite = spriteDone;
	}

	public void setImageNotDone()
	{
		imageAvatar.sprite = spriteNotDone;
	}
	public void setInfoAchivements(Texture2D imageA, string title, string Description, string Coin)
	{
		txtTitle.text = title;
		txtDescription.text = Description;
		txtCoin.text = Coin;
	}

}

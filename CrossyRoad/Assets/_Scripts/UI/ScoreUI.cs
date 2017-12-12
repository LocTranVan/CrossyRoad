using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ScoreUI : MonoBehaviour {
	public GameObject[] scores, numberCoins;
	public GameObject panelCoins;
	public List<Sprite> spriteNumberSocre, spriteNumberCoins;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void setScoreAndChangeSprite(int numberSCore)
	{
		//Debug.Log(numberSCore.ToString());
		string number = numberSCore.ToString();
		char[] numbers = number.ToCharArray();
		activeImage(scores, numbers.Length);

		for(int i = 0; i < number.Length; i++)
		{
			Image image = scores[i].GetComponent<Image>();
			image.sprite = getSpriteScore(number[i]);
			
		}
	}
	private void activeImage(GameObject[] arrayImage, int number)
	{
		if (number == 1)
		{
			arrayImage[0].SetActive(true);
			arrayImage[1].SetActive(false);
			arrayImage[2].SetActive(false);
		} else if (number == 2)
		{
			arrayImage[0].SetActive(true);
			arrayImage[1].SetActive(true);
			arrayImage[2].SetActive(false);
		}
		else if (number == 3)
		{
			arrayImage[0].SetActive(true);
			arrayImage[1].SetActive(true);
			arrayImage[2].SetActive(true);
		}
	}
	public void setNumberCoinAndChangeSprite(int numberSCoin)
	{
		string number = numberSCoin.ToString();
		char[] numbers = number.ToCharArray();
		//Debug.Log(number);
		DOTweenAnimation animate = panelCoins.GetComponent<DOTweenAnimation>();
		animate.DORestartById("ZoomOut");

		activeImage(numberCoins, numbers.Length);
		/*
		for (int i = 0; i < numberCoins.Length; i++)
		{
			Image image = numberCoins[i].GetComponent<Image>();
			image.sprite = getSpriteCoin(number[i]);

		} */
		int k = 0;
		for (int i = numbers.Length - 1; i >= 0; i--)
		{
			Image image = numberCoins[i].GetComponent<Image>();
			image.sprite = getSpriteCoin(number[k]);
			k++;

		}
	}
	private Sprite getSpriteScore(char number)
	{
		switch (number)
		{
			case '0':
				return spriteNumberSocre[0];
			case '1':
				return spriteNumberSocre[1];
			case '2':
				return spriteNumberSocre[2];
			case '3':
				return spriteNumberSocre[3];
			case '4':
				return spriteNumberSocre[4];
			case '5':
				return spriteNumberSocre[5];
			case '6':
				return spriteNumberSocre[6];
			case '7':
				return spriteNumberSocre[7];
			case '8':
				return spriteNumberSocre[8];
			case '9':
				return spriteNumberSocre[9];
			default:
				return null;

		}
	}
	private Sprite getSpriteCoin(char number)
	{
		switch (number)
		{
			case '0':
				return spriteNumberCoins[0];
			case '1':
				return spriteNumberCoins[1];
			case '2':
				return spriteNumberCoins[2];
			case '3':
				return spriteNumberCoins[3];
			case '4':
				return spriteNumberCoins[4];
			case '5':
				return spriteNumberCoins[5];
			case '6':
				return spriteNumberCoins[6];
			case '7':
				return spriteNumberCoins[7];
			case '8':
				return spriteNumberCoins[8];
			case '9':
				return spriteNumberCoins[9];
			default:
				return null;

		}
	}
}

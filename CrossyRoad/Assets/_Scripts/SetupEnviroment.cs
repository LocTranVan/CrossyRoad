﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupEnviroment : MonoBehaviour {

	public GameObject[] strips;
	public Transform camera, Player;
	public GameObject Eagle;
	public GameObject Introl, IntrolPC;
	//private GameObject[] allStrips;
	private List<GameObject> allStrips;
	public float offset;
	public float depth;
	private const int GRASS = 0, GRASSDARK = 1, ONEWAYSTREET = 2,
		TWOWAYSTREET = 3, TRACKTRAIN = 4, OCEAN = 5;
	

	private int previousStrip = GRASSDARK;
	private Vector3 positionPreviousStrip;
	public float maxDistance = 89f;
	private bool sPawdedEagle;

	private string[] introlString = {"111000111", "111000111", "111000111",
		"111101111", "111101111", "111101111", "111101111",
	"110001111", "110001111", "110111111", "110001111", "110001111", "111101111"};
	// Use this for initialization
	private void Awake()
	{
		//PlayerPrefs.DeleteAll();
	}
	void Start () {
		allStrips = new List<GameObject>();
		//PlayerPrefs.SetInt("highscore", 0);
		if (PlayerPrefs.GetInt("highscore") <= 0)
			Introduction();
		else
		{
#if UNITY_ANDROID
			Introl.SetActive(false);
#else
			IntrolPC.SetActive(false);
#endif
			int k = 0;
			do
			{
				GameObject gras = (k % 2 == 0) ? strips[GRASSDARK] : strips[GRASS];
				positionPreviousStrip = transform.position + new Vector3(offset * k, 0, 0);

				GameObject grassOb = Instantiate(gras, positionPreviousStrip, Quaternion.identity);
				allStrips.Add(grassOb);
				if (k < 7 && k > 3)
					grassOb.GetComponent<Grass>().GrassMid = true;
				k++;
			} while (k < 7);

			int tam = 8;
			while (tam > 0)
			{
				spaw();
				tam--;
			}
		}
	}
	private void Introduction()
	{
#if UNITY_ANDROID
			Introl.SetActive(true);
#else
		IntrolPC.SetActive(true);
#endif
		int k = 0;
		do
		{
			GameObject gras = (k % 2 == 0) ? strips[GRASSDARK] : strips[GRASS];
			positionPreviousStrip = transform.position + new Vector3(offset * k, 0, 0);

			GameObject grassOb = Instantiate(gras, positionPreviousStrip, Quaternion.identity);

			allStrips.Add(grassOb);
			if (k > 3)
			{
				grassOb.GetComponent<Grass>().setIntroldution(introlString[k - 4]);
				//Debug.Log( "here"+k);
			}
			k++;
		} while (k < 17);
	}
	public void spawEagle()
	{
		if (!sPawdedEagle)
		{
			Instantiate(Eagle, Player.position + new Vector3(90, 13, 0), Quaternion.identity);
			sPawdedEagle = true;
			Player.gameObject.GetComponent<Character>().isPause = true;
		}
	}
	private void spaw()
	{
		previousStrip = getStrip();
		positionPreviousStrip += new Vector3(offset, 0, 0);
		allStrips.Add(Instantiate(strips[previousStrip], getPositionStrip(),
			Quaternion.identity));
	}
	private int getStrip()
	{
		// 0 1 = grass
		int scores = (int)(gameManager.intance.getScore() / 5);

		int startCurrentStrip = Random.Range(0, scores);
		if (startCurrentStrip <= 1)
			startCurrentStrip = GRASS;
		else
			startCurrentStrip = ONEWAYSTREET;

		//	int currentStrip = Random.Range(startCurrentStrip, OCEAN + 1);
		int currentStrip = Random.Range(2, 4);
		switch (currentStrip)
		{
			case GRASS:
				return (previousStrip == GRASS) ? GRASSDARK : GRASS;
			case GRASSDARK:
				return (previousStrip == GRASSDARK) ? GRASS : GRASSDARK;
			case TWOWAYSTREET:
				return ((previousStrip == ONEWAYSTREET) || 
					(previousStrip == TWOWAYSTREET)) ? TWOWAYSTREET : ONEWAYSTREET;
			case ONEWAYSTREET:
				return ((previousStrip == ONEWAYSTREET) ||
					(previousStrip == TWOWAYSTREET)) ? TWOWAYSTREET : ONEWAYSTREET;
			default:
				return currentStrip;
		}
	}
	private Vector3 getPositionStrip()
	{
		if(previousStrip == GRASS || previousStrip == GRASSDARK)
		{
			positionPreviousStrip.y = 0;
		}else if (previousStrip == ONEWAYSTREET || previousStrip == TWOWAYSTREET || previousStrip == TRACKTRAIN)
		{
			positionPreviousStrip.y = -depth;
		}else
		{
			positionPreviousStrip.y = -2*depth;
		}
		return positionPreviousStrip;
	}

	private void destroyObject()
	{

		Destroy(allStrips[0]);
		allStrips.Remove(allStrips[0]);
	}
	private void FixedUpdate()
	{
	//	if(allStrips[0] != null)
		if((allStrips[0].transform.position.x - camera.transform.position.x) <= maxDistance)
		{
			spaw();
			destroyObject();
		}
	}
	void Update () {
		
	}
}

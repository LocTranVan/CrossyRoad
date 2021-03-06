﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UITabStartGame : MonoBehaviour {
	public GameObject btTap, btArowUP;
	public GameObject ImageCrossy;
	public GameObject ButtonChoosePlayer, ButtonChossePet, ButtonMore;
	private Vector3 currentImage;
	
	// Use this for initialization
	void Start () {
	
	}	
	private void OnEnable()
	{
#if UNITY_ANDROID
		btTap.SetActive(true);
#else
		if(btArowUP != null)
		btArowUP.SetActive(true);
#endif

		Button bt = GetComponent<Button>();
		bt.enabled = true;
		ButtonChoosePlayer.SetActive(true);
		ButtonMore.SetActive(true);
		if (ImageCrossy.transform.position.x > 1000)
			ImageCrossy.transform.position -= new Vector3(1000, -300, 0);
		//ImageCrossy.transform.position = currentImage;
	}
	// Update is called once per frame
	void Update () {
	}
	public void StartGame()
	{
		Debug.Log("Start Game");
		btTap.SetActive(false);
		btArowUP.SetActive(false);
		ImageCrossy.GetComponent<DOTweenAnimation>().DORestart();
		ButtonChoosePlayer.SetActive(false);
		ButtonMore.SetActive(false);
		Button bt = GetComponent<Button>();
		bt.enabled = false;
		
	}
	public void enableTabStartGame()
	{
		gameObject.SetActive(false);
	}
	public void justTest()
	{
		Debug.Log("test");
	}
}

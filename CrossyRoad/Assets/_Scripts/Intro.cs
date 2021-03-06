﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Intro : MonoBehaviour {
	public Sprite pressImage, dontPressImage;
	private Image image;
	private bool ON;
	private GameObject Player;
	public LayerMask Tree;
	private GameObject InGame;
	private Animator animator;

	public Sprite[] allImages;
	// Use this for initialization
	private enum gameState
	{
		left, right, top
	}

	private gameState stateCurrent = gameState.top,
		statePrevious = gameState.top;
	void Start () {
		image = GetComponent<Image>();
		Player = GameObject.Find("Player");
		animator = GetComponent<Animator>();
		StartCoroutine(Introldution(1f));

		
		//moveLeft();
	}
	public void moveLeft()
	{
#if UNITY_ANDROID
		animator.SetFloat("Speed", 1);
		Debug.Log("move left");
		DOTweenAnimation dtAnimation = GetComponent<DOTweenAnimation>();
		dtAnimation.DORestartById("MoveLeft");
#else
		image.sprite = allImages[2];
#endif
	}
	public void moveRight()
	{
#if UNITY_ANDROID
		animator.SetFloat("Speed", 1);
		Debug.Log("move right");
		DOTweenAnimation dtAnimation = GetComponent<DOTweenAnimation>();
		dtAnimation.DORestartById("MoveRight");
#else
		image.sprite = allImages[1];
#endif
	}
	private IEnumerator Introldution(float waitTime)
	{
		while (true)
		{
			yield return new WaitForSeconds(0.3f);
			if(Player != null)
				checkSpre(Player.transform.position);
			else
			{
				Player = GameObject.Find("Player");
			}
			yield return new WaitForSeconds(waitTime);
		}
	}
	private void ChangeSprite()
	{
		ON = !ON;
		if (ON)
		{
			image.sprite = pressImage;
			return;
		}
		image.sprite = dontPressImage;
	}
	// Update is called once per frame
	void Update () {
	}

	private void checkSpre(Vector3 postion)
	{
		if (postion.x >= 150)
			gameObject.SetActive(false);

		postion += Vector3.up * 3;
		if (!Physics.Linecast(postion, postion + Vector3.right * 9, Tree)) {
#if UNITY_ANDROID
			animator.SetFloat("Speed", 0);
		
#else
			image.sprite = allImages[0];
			if(stateCurrent != gameState.top)
				statePrevious = stateCurrent;

			stateCurrent = gameState.top;
#endif
			return;
		}

		if (!Physics.Linecast(postion, postion + Vector3.forward * 9, Tree))
		{
			moveLeft();
			if (stateCurrent != gameState.left)
				statePrevious = stateCurrent;

			stateCurrent = gameState.left;

		}
		else
		{
			moveRight();
			if (stateCurrent != gameState.right)
				statePrevious = stateCurrent;

			stateCurrent = gameState.right;
		}
		
	}
}

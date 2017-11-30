﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class ControllScrollView : MonoBehaviour, IEndDragHandler, IBeginDragHandler{
	public RectTransform center;
	public RectTransform[] AllCharacter;
	public RectTransform MacCenter;
	private float distanceCharacter;
	private Vector2 endPosition;
	private bool move;


	//For Lerp
	private float startTime, journeyMove;
	//For Object choosed
	private GameObject currentChoose, previousChoose;

	private void Start()
	{
		distanceCharacter = Vector3.Distance(AllCharacter[0].anchoredPosition, AllCharacter[1].anchoredPosition);
		currentChoose = AllCharacter[(int)(AllCharacter.Length / 2) + 1].gameObject;
		previousChoose = currentChoose;
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		move = false;
	}

	public void OnEndDrag(PointerEventData eventData)

	{
		int numberCharac = Mathf.RoundToInt(center.anchoredPosition.x / distanceCharacter);

		Vector2 minPosition = center.anchoredPosition;
		float minDistance = 10000;


		foreach(RectTransform p in AllCharacter)
		{
			float distance = Vector2.Distance(p.anchoredPosition, center.anchoredPosition);
			if(distance < minDistance)
			{
				minDistance = distance;
				minPosition = p.anchoredPosition;
			}
		}
	

		endPosition = minPosition;
		//endPosition.x = numberCharac * distanceCharacter;

		startTime = Time.time;
		journeyMove = Vector3.Distance(center.anchoredPosition, endPosition);

		move = true;


		float min = 100;
		GameObject newChoose = currentChoose;
		foreach (RectTransform p in AllCharacter)
		{
			float distance = Mathf.Abs(p.position.x - MacCenter.position.x);
			if (distance < min)
			{
				min = distance;
				newChoose = p.gameObject;
			}

		}
		if (newChoose != currentChoose)
		{
			previousChoose = currentChoose;
			currentChoose = newChoose;

			Scale();
		}
	}
	private void Scale()
	{
		currentChoose.gameObject.GetComponent<DOTweenAnimation>().DORestartById("ZoomOut");
		SoundManager.intance.soundBTChoosePlayer();
		previousChoose.gameObject.GetComponent<DOTweenAnimation>().DORestartById("ZoomIn");
	}
	public void ChangePosition()
	{

		Debug.Log(center.position + " " + center.anchoredPosition + "c" + currentChoose.name);


	}

	public void changeCharacter()
	{
		gameManager.intance.setPlayer(currentChoose);
	}


	private void FixedUpdate()
	{
		
		if (move)
		{
			float timeJourney = Time.time - startTime;
			float distance = timeJourney * 10 / journeyMove;

			center.anchoredPosition = Vector3.Lerp(center.anchoredPosition, endPosition, distance);


			if (center.anchoredPosition == endPosition)
				move = false;
		}
		

	}

	
}

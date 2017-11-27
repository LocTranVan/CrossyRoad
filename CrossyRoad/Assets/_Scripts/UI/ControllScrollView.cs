using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class ControllScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler{
	public GameObject ScrollView;
	protected UnityEngine.UI.ScrollRect scrollRect;
	private ControllScrollView conScrollView;
	public GameObject[] AllCharacters;

	private Vector3 currentDrag = Vector3.zero;
	private int endDrag;
	private float DistanceStartToEnd;
	private Vector3 nearVe;
	private bool move;

	private float timeStart, JourneyDistan;

	private GameObject currentCharacter, newCharacter;
	// Use this for initialization

	private void Start()
	{
		currentDrag = ScrollView.transform.position;

		scrollRect = GetComponent<UnityEngine.UI.ScrollRect>();
	
		Debug.Log("position" + ScrollView.transform.position);
		currentCharacter = AllCharacters[4];
		newCharacter = AllCharacters[3];
		setScaleCurrent();
	}
	// Update is called once per frame
	void Update () {
		/*
		if (move)
		{
			float timeA = Time.time - timeStart;
			float dis = timeA  * Time.deltaTime * 3/ JourneyDistan;
			ScrollView.transform.position = Vector3.Lerp(ScrollView.transform.position, nearVe, dis);
			if (transform.position == nearVe)
				move = false;	
		} */
	}
	public void setVelocity()
	{

	}

	public void OnBeginDrag(PointerEventData eventData)
	{

		//	currentDrag = ScrollView.transform.position;
		

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		float minDistance = 1000;
		GameObject t = newCharacter;
		foreach (GameObject p in AllCharacters)
		{
			float currentDistance = Vector3.Distance(p.transform.position, currentDrag);
			if (currentDistance < minDistance)
			{
				minDistance = currentDistance;
				t = p;
			}

		}
		currentCharacter = newCharacter;
		newCharacter = t;
		if(newCharacter != currentCharacter)
			setScaleCurrent();
		
	
	}
	public void setPlayerCharacter()
	{
		gameManager.intance.setPlayer(newCharacter);
	}
	private void setScaleCurrent()
	{
		currentCharacter.GetComponent<DOTweenAnimation>().DORestartById("ZoomIn");
		newCharacter.GetComponent<DOTweenAnimation>().DORestartById("ZoomOut");
		

		//newCharacter.GetComponent<DOTweenAnimation>().DOPlayById("ZoomOut

	}

}

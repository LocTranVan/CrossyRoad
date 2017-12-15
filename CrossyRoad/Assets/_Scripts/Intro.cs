using System.Collections;
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
	// Use this for initialization
	
	void Start () {
		image = GetComponent<Image>();
		Player = GameObject.Find("Player");
		animator = GetComponent<Animator>();
		StartCoroutine(Introldution(1f));
		//moveLeft();
	}
	public void moveLeft()
	{
		animator.SetFloat("Speed", 1);
		Debug.Log("move left");
		DOTweenAnimation dtAnimation = GetComponent<DOTweenAnimation>();
		dtAnimation.DORestartById("MoveLeft");
	}
	public void moveRight()
	{
		animator.SetFloat("Speed", 1);
		Debug.Log("move right");
		DOTweenAnimation dtAnimation = GetComponent<DOTweenAnimation>();
		dtAnimation.DORestartById("MoveRight");
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
			animator.SetFloat("Speed", 0);
			return;
		}

		if (!Physics.Linecast(postion, postion + Vector3.forward * 9, Tree))
			moveLeft();
		else
		{
			moveRight();
		}
		
	}
}

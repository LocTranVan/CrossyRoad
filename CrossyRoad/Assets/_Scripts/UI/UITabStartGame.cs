using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UITabStartGame : MonoBehaviour {
	public GameObject btTap;
	public GameObject ImageCrossy;
	public GameObject ButtonChoosePlayer, ButtonChossePet;
	private Vector3 currentImage;
	// Use this for initialization
	void Start () {
	

		Debug.Log(currentImage);
	}	
	private void OnEnable()
	{
		btTap.SetActive(true);
		ButtonChoosePlayer.SetActive(true);
		ButtonChossePet.SetActive(true);
		if (ImageCrossy.transform.position.x > 1000)
			ImageCrossy.transform.position -= new Vector3(1000, 0, 0);
			Debug.Log(ImageCrossy.transform.position);
		//ImageCrossy.transform.position = currentImage;
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void StartGame()
	{
		Debug.Log("Start Game");
		btTap.SetActive(false);
		ImageCrossy.GetComponent<DOTweenAnimation>().DORestart();
		ButtonChoosePlayer.SetActive(false);
		ButtonChossePet.SetActive(false);
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

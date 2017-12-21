using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TakePictureAndShare : MonoBehaviour {
	public GameObject ImageAvatar;
	private Texture2D snapshot;
	private GameObject Player;
	// Use this for initialization
	private void Awake()
	{
		
	}
	void Start () {
	//	Invoke("TakePicture", 1f);
		//TakePicture();
		//StartCoroutine(TakeSnapshot(240, 290));
		

	}
	private void OnEnable()
	{
		RectTransform position = GetComponent<RectTransform>();
		position.localScale = new Vector3(0, 0, 0);
		
		StartCoroutine(startPlayImage());
	}
	private IEnumerator startPlayImage()
	{
		snapshot = null;

		yield return new WaitForEndOfFrame();
		Player = GameObject.Find("Player");
		Character charact = Player.GetComponent<Character>();
		snapshot = charact.getTexture();
		TakePicture();

		DOTweenAnimation animation = GetComponent<DOTweenAnimation>();
		animation.DORestart();

	}
	// Update is called once per frame
	void Update () {
		
	}
	public void TakePicture()
	{
		RawImage image = ImageAvatar.GetComponent<RawImage>();
		image.texture = null;
		image.texture = snapshot;
	}
	public IEnumerator TakeSnapshot(int width, int height)
	{

		yield return new WaitForEndOfFrame();
		Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, true);
		texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		texture.Apply();
		snapshot = texture;

		TakePicture();
		if (snapshot == null)
			Debug.Log("here");
	}

}

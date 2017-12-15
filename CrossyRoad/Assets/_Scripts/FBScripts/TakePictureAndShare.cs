using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		Player = GameObject.Find("Player");
		Character charact = Player.GetComponent<Character>();
		snapshot = charact.getTexture();
		TakePicture();
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void TakePicture()
	{
		RawImage image = ImageAvatar.GetComponent<RawImage>();
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

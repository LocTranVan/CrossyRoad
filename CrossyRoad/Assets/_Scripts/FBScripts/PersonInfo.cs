using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonInfo : MonoBehaviour {
	public GameObject Image;
	public GameObject TxtName;
	public GameObject TxtScore;

	public void setImage(Texture texture)
	{
		RawImage image = Image.GetComponent<RawImage>();
		image.texture = texture;
	}
	public void setTxtName(string name)
	{
		Text txtName = TxtName.GetComponent<Text>();
		txtName.text = name;
	}
	public void setTxtScore(string score)
	{
		Text txtScore = TxtScore.GetComponent<Text>();
		txtScore.text = score;
	}
	// Update is called once per frame
	
}

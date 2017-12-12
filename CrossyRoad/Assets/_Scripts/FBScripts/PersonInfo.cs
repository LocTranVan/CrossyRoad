using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonInfo : MonoBehaviour {
	public GameObject Image;
	public GameObject TxtName;
	public GameObject TxtScore;
	public GameObject TxtRank;
	public void setImage(Texture texture)
	{
		//RawImage image = Image.GetComponent<RawImage>();
		//image.texture = texture;
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
	public void SetupElement(int rank, object entryObj)
	{
		var entry = (Dictionary<string, object>)entryObj;
		var user = (Dictionary<string, object>)entry["user"];
		Text txtName = TxtName.GetComponent<Text>();
		Text txtScore = TxtScore.GetComponent<Text>();
		Text txtRank = TxtRank.GetComponent<Text>();

		RawImage image = Image.GetComponent<RawImage>();

		Texture picture;
		if (GameStateManager.FriendImages.TryGetValue((string)user["id"], out picture))
		{
			image.texture = picture;
		}

		txtScore.text = GraphUtil.GetScoreFromEntry(entry).ToString();
		txtName.text = ((string)user["name"]).Split(new char[] { ' ' })[0];
		txtRank.text = rank.ToString() + "st";

	}
	// Update is called once per frame
	
}

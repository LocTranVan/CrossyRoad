using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour {
	public Material blackMaterial;
	public Material nomarlMaterial;
	public string nameCharacter;
	public bool buy;
	public int Price;
	// Use this for initialization
	private void Awake()
	{
		
	}
	void Start () {

	//	Debug.Log(GameStateManager.HighScore);

		if(PlayerPrefs.GetString(nameCharacter) == "Yes")
		{
			GetComponentInChildren<Renderer>().material = nomarlMaterial;
		}else
		{
			GetComponentInChildren<Renderer>().material = blackMaterial;
			if (buy)
				setUnlock();
		}
		
	}
	public bool IsUnlock()
	{
		if (PlayerPrefs.GetString(nameCharacter) == "Yes")
			return true;
		return false;
	}
	public int getPrice()
	{
		return Price;
	}
	public void setUnlock()
	{
		PlayerPrefs.SetString(nameCharacter, "Yes");
		GetComponentInChildren<Renderer>().material = nomarlMaterial;
	}
	// Update is called once per frame
	void Update () {
		
	}
}

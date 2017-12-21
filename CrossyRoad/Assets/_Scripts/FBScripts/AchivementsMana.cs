using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementsMana : MonoBehaviour {
	public GameObject[] listAchivements;
	// Use this for initialization
	void Start () {
		
	}
	public void Redraw()
	{
		foreach(GameObject ob in listAchivements)
		{
			Achivements achive = ob.GetComponent<Achivements>();
			if(achive != null) 
			foreach(string tl in GameStateManager.Achivements)
			{
				if(tl == achive.ID)
				{
					achive.setImageDone();
				}
			}

		}
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}

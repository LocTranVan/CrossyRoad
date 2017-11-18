using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {

	public static gameManager intance;
	public GameObject EndPanel;
	// Use this for initialization
	void Start () {
		if (intance == null)
			intance = this;
		else if(intance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EndGame()
	{
		EndPanel.SetActive(true);
	}
	public void Play()
	{
		Physics.IgnoreLayerCollision(9, 10, false);
		Physics.IgnoreLayerCollision(8, 10, false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		EndPanel.SetActive(false);
	}
}

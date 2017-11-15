using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
	private GameObject objectFather;
	// Use this for initialization
	void Start () {
		
	}
	public void init(GameObject objectFather)
	{
		this.objectFather = objectFather;
	}
	// Update is called once per frame
	private void FixedUpdate()
	{
		if (objectFather == null)
			Destroy(gameObject);
	}
}

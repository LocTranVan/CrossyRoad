using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	// Use this for initialization
	public GameObject Player;
	public float speed, speedPlayer;
	private float maxDistance;
	void Start () {
		maxDistance = Player.transform.position.x - transform.position.x;
	}

	private void FixedUpdate()
	{
	
		if((Player.transform.position.x - transform.position.x) > (maxDistance + 9f))
		{
			transform.position += new Vector3(speedPlayer * Time.deltaTime, 0, 0);
		}else
		{
			transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
		}
	}
	// Update is called once per frame
	void Update () {
			
	}
}

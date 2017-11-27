using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour {

	// Use this for initialization
	public float speedEagle;
	private GameObject player;
	private bool hitPlayer;
	private Vector3 pLast;
	void Start () {
		pLast = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position -= new Vector3(speedEagle, 0, 0) * Time.deltaTime;
		if (hitPlayer)
		{
			Vector3 position = player.transform.position;

			position.x = transform.position.x;
			player.transform.position = position;

		}
		if(transform.position.x < pLast.x - 150)
		{
			player.GetComponentInParent<Character>().isDead = true;
			player.GetComponentInParent<Character>().isPause = false;
			Destroy(gameObject);
			
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Debug.Log("hit");
			hitPlayer = true;
			player = other.gameObject;
		}
	}
}

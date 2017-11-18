using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	// Use this for initialization
	public Transform Player;
	public float speed, speedPlayer;
	private float maxDistance, maxDistanceZ;

	private Vector3 velocity = Vector3.zero, distanceVector;
	private float distanceZ;
	void Start () {
		maxDistance = Player.position.x - transform.position.x;
		maxDistanceZ = Player.position.z;

		distanceVector = Player.transform.position - transform.position;
		distanceZ = distanceVector.z;
	//	velocity = new Vector3(speedPlayer, 0, 0);
	}

	private void FixedUpdate()
	{
		if (!Player.gameObject.GetComponentInParent<Character>().isDead)
		{
			transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

			Move();
		}

		//smoothCamera();

	}
	private void Move()
	{
		Vector3 distanceP = Player.position - transform.position;
		
		if ((distanceP.x) > (distanceVector.x + 9f))
		{
			transform.position += new Vector3(speedPlayer * Time.deltaTime, 0, 0);
		}
		if ((distanceP.z) > (distanceVector.z + 15f))
		{
			transform.position += new Vector3(0, 0, 8 * Time.deltaTime);
		}else if ((distanceP.z) < (distanceVector.z - 15f))
		{
			transform.position -= new Vector3(0, 0, 8 * Time.deltaTime);
		}

	}

	// Update is called once per frame
	void Update () {
			
	}
}

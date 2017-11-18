using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
	private Rigidbody rigidbody;
	private float speed;
	// Use this for initialization
	private float directionWay;
	private GameObject objectFather;
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
	}
	public void init(float speed, GameObject objectFather)
	{
		this.speed = speed;
		this.objectFather = objectFather;
		float direction = (speed < 0) ? 180 : 0;

		transform.localEulerAngles = new Vector3(0, direction, 0);
	}
	void OnCollisionExit(Collision other)
	{
	
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			//rigidbody.isKinematic = true;
			if(objectFather.GetComponent<SpawCar>() != null)
				objectFather.GetComponent<SpawCar>().effectBlood(collision.gameObject.transform.position);
			else
				objectFather.GetComponent<Train>().effectBlood(collision.gameObject.transform.position);
		}

			if (collision.gameObject.tag == "Ground")
		{
			rigidbody.useGravity = false;
			rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX;
		}
		 
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "BarieEnd")
		{
			Destroy(gameObject);
		}
	}
	void Update () {
		
	}
	void FixedUpdate()
	{
		rigidbody.velocity = new Vector3(0, 0, -speed);
		if(objectFather == null)
		{
			Destroy(gameObject);
		}
	}
}

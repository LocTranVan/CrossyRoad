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
	private void OnTriggerEnter(Collider other)
	{
		
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
		rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, -speed);
		if(objectFather == null)
		{
			Destroy(gameObject);
		}
	}
}

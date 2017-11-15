﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour {
	private Rigidbody rigidbody;
	private float speed;
	public float speedPosition;
	// Use this for initialization
	private float directionWay;
	private bool fastSpeed = true;
	private Transform pStartThac, pEndThac;
	private float y = 0;
	private bool rotate = true;
	private GameObject objectFather;
	private Vector3 startPosHit = new Vector3(1, 1, 1), endPosHit = new Vector3(1, 0.8f, 1);
	private float journeyHit;
	private float timeHit, timeStartHit;
	private bool Sank = false, changePos = false;
	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		journeyHit = Vector3.Distance(startPosHit, endPosHit);
	}
	public void init(float speed, GameObject objectFather)
	{
		this.speed = speed;
		this.objectFather = objectFather;

		//speed = 3f;
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			timeHit = Time.time;
			Sank = true;
			changePos = !changePos;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			timeHit = Time.time;
			Sank = true;
			changePos = !changePos;
		}
	}
	private void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.tag == "BarieEnd")
		{
			Destroy(gameObject);
		}
		

	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == ("Ocean"))
		{
			speed = (fastSpeed) ? (speed / 3) : (speed * 3);
			fastSpeed = !fastSpeed;
		}
	}
	private void AnimateHit()
	{

	}
	private void FixedUpdate()
	{
		if (speed != 0)
		{
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, -speed);
		//	transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
		}
		else
		{

			if(y <= 0)
			{
				rotate = true;
			}else if(y >= 90)
			{
				rotate = false;
			}
			y = (rotate) ? y + 0.5f : y - 0.5f;
			transform.eulerAngles = new Vector3(0, y, 0);
		}
		if (objectFather == null)
			Destroy(gameObject);
		
		if (changePos)
		{
			float timeScale = (Time.time - timeHit) * speedPosition;
			float journeyLeght = timeStartHit / journeyHit;
			if (Sank)
			{		
				transform.localScale = Vector3.Lerp(endPosHit, startPosHit, journeyLeght);
				if (transform.localScale == startPosHit)
				{
					Sank = false;
				}
			}
			else
			{
				transform.localScale =  Vector3.Lerp(startPosHit, endPosHit, journeyLeght);
				if (transform.localScale == endPosHit)
				{
					changePos = false;
				}
			}
			
		}
	
	}
	// Update is called once per frame
	void Update () {
		
	}
}
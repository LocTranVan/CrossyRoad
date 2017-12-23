using System.Collections;
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
	private Vector3 startPosHit = new Vector3(1, 1, 1), endPosHit = new Vector3(1, 0.6f, 1);
	private float journeyHit;
	private float timeHit, timeStartHit;
	private bool Sank = false, changePos = false;
	public bool beo;
	//AudioSource
	private AudioSource audioSource;
	public AudioClip WoodInAudio, WoodOutAudio, Beo;
	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		journeyHit = Vector3.Distance(startPosHit, endPosHit);
	}
	public void init(float speed, GameObject objectFather)
	{
		this.speed = speed;
		this.objectFather = objectFather;

	}
	public float getSpeed()
	{
		return speed;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "BarieEnd")
		{
			objectFather.GetComponent<Ocean>().setNumberWoodInOcean();
			Destroy(gameObject);
		}

		tranformWhenHit(other);

	}
	private void OnTriggerExit(Collider other)
	{
		tranformWhenHit(other);

		if (other.gameObject.name.Contains("WaterFall"))
		{
			speed = (fastSpeed) ? (speed / 3) : (speed * 3);
			fastSpeed = !fastSpeed;
		}
	}
	private void tranformWhenHit(Collider target)
	{
		if (target.gameObject.tag == "CoreCharacter")
		{
			Debug.Log("hit");
			timeHit = Time.time;
			Sank = true;
			changePos = !changePos;

			if (speed != 0)
				audioSource.PlayOneShot(WoodInAudio, 1f);
			else
				audioSource.PlayOneShot(Beo, 1f);
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
			float journeyLeght = timeScale / journeyHit;
			if (Sank)
			{		
				transform.localScale = Vector3.Lerp(startPosHit, endPosHit, journeyLeght);
				if (transform.localScale == endPosHit)
				{
					Sank = false;
					timeHit = Time.time;
				}
			}
			else
			{
				transform.localScale =  Vector3.Lerp(endPosHit, startPosHit, journeyLeght);
				if (transform.localScale == startPosHit)
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

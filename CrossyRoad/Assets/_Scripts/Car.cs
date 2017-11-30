using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
	private Rigidbody rigidbody;
	private float speed;
	// Use this for initialization
	private float directionWay;
	private GameObject objectFather;
	private AudioSource audioSource, audioCoiXe;
	public AudioClip carEnginer, coiXe;

	public bool train;
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		audioCoiXe = GetComponent<AudioSource>();
		if (!train)
		{
			audioSource.clip = carEnginer;
			audioSource.volume = Random.Range(0.05f, 0.2f);
			audioSource.loop = true;
			audioSource.Play();

			float waitTime = Random.Range(2f, 10f);

			StartCoroutine(coiXeEnum(waitTime));
		}
		else
		{
			audioSource.clip = carEnginer;
			audioSource.PlayOneShot(carEnginer, 0.8f);
			audioSource.PlayDelayed(0.5f);
			StartCoroutine(coiXeEnum(1f));

		}
		

		
	}
	
	private IEnumerator coiXeEnum(float time)
	{
		while (true)
		{
			yield return new WaitForSeconds(time);
			audioCoiXe.PlayOneShot(coiXe, 1.5f);	
		}
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
	public float getSpeed()
	{
		return speed;
	}
}

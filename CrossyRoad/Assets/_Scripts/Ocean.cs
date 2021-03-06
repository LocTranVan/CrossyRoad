﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour {
	// Use this for initialization
	public GameObject[] Stuff;
	public GameObject PracticleSystem;
	private GameObject vatTroi;
	[SerializeField]
	private GameObject[] positionSpaw;
	//private Transform[] positionThac;

	private IEnumerator coroutine;
	private int indexStartCar, indexEndCar;

	private int numberWood = 0;
	private float speed;
	private bool spaw = true;
	private float waitTime;

	private AudioSource audioSource, audioFallWater;
	public AudioClip riverAudio, flallRiverAudio;
	//public AudioClip riverAudio;
	void Start () {

		vatTroi = Stuff[Random.Range(0, 3)];
		audioSource = GetComponent<AudioSource>();
		audioFallWater = GetComponent<AudioSource>();

		audioSource.loop = true;
		audioSource.PlayOneShot(riverAudio, 0.1f);

		if (vatTroi == Stuff[2])
		{
			int number = Random.Range(1, 4);
			int k = 0;
			while(number > 0)
			{
				Vector3 current = transform.position;
				current.y = current.y + 2;
				current.z += k * 8 * Mathf.Pow(-1, number);
				GameObject beo = Instantiate(vatTroi, current, Quaternion.identity);
				beo.GetComponent<Wood>().init(0, gameObject);
				k = Random.Range(2, 4);
				number--;
			}
		}
		else
		{
			indexEndCar = Random.Range(0, 2);
			indexStartCar = (indexEndCar == 1) ? 0 : 1;

			positionSpaw[indexEndCar].SetActive(true);
			speed = Random.Range(60f, 80f);
			waitTime = Random.Range(1f, 2f);

			coroutine = SpawStuff(waitTime);
			StartCoroutine(coroutine);
		}
	}
	
	void Update () {
		
	}
	private void OnTriggerEnter(Collider other)
	{
		//GameObject objectFather = other.GetComponentInParent<Transform>().gameObject;
		
		if (other.name == "uniHorse")
		{
			//other.gameObject.GetComponent<Character>().isDead = true;
			other.gameObject.GetComponentInParent<Character>().TakeDamage();
			audioFallWater.volume = 1f;
			audioFallWater.PlayOneShot(flallRiverAudio, 1f);
			
			Vector3 position = PracticleSystem.transform.position;
			position.z = other.gameObject.transform.position.z;
			PracticleSystem.transform.position = position;
			PracticleSystem.SetActive(true);
			Debug.Log("hit Player");
		}
		if(other.name == "LionCharcacter")
		{
			Debug.Log("lion");
			Vector3 position = PracticleSystem.transform.position;
			position.z = other.gameObject.transform.position.z;
			PracticleSystem.transform.position = position;
			other.gameObject.GetComponentInParent<Enemy>().TakeDamage();
			PracticleSystem.SetActive(true);

		}
		
	}
	private IEnumerator SpawStuff(float waitTime)
	{
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			if (numberWood < 3)
			{ 
				GameObject Wood = Instantiate(vatTroi, positionSpaw[indexStartCar].transform.position, Quaternion.identity);
				float direction = (indexStartCar == 0) ? 1 : -1;
				Wood.GetComponent<Wood>().init(speed * direction, gameObject);
				numberWood++;
			}
		}

	}

	public void setNumberWoodInOcean()
	{
		numberWood--;
	}
}

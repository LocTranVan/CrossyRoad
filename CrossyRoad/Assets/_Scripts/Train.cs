using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour {
	public GameObject[] spawPositionCar;
	public GameObject trains;
	private int indexEndTrain, indexStartTrain;
	private float speed;
	private IEnumerator coroutine;
	// Use this for initialization
	void Start () {
		indexEndTrain = Random.Range(0, 2);
		indexStartTrain = (indexEndTrain == 1) ? 0 : 1;

		spawPositionCar[indexEndTrain].SetActive(true);

		speed = Random.Range(100f, 400f);
		float waitTime = Random.Range(2.0f, 4.0f);

		coroutine = spawTrain(waitTime);
		StartCoroutine(coroutine);
	}
	private IEnumerator spawTrain(float waitTime)
	{
		while (true)
		{
			GameObject train =	Instantiate(trains, spawPositionCar[indexStartTrain].transform.position, Quaternion.identity);

			float direction = (indexStartTrain == 0) ? 1 : -1;
			train.GetComponent<Car>().init(speed * direction, gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

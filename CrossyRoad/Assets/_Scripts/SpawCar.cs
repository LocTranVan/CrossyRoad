using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawCar : MonoBehaviour {
	public GameObject[] spawPositionCar;
	public GameObject[] Cars;
	private const int TAXI = 0, STRUCT = 1, CAR = 2;
	private IEnumerator coroutine;
	private int indexStartCar, indexEndCar;
	public GameObject PracticleSystem;

	private int numberCars = 0;
	private float speed;
	// Use this for initialization
	void Start () {

		//Instantiate(Cars[Random.Range(0, 3)], startCar.transform.position, Quaternion.identity);
		//endCar.SetActive(true);
		indexEndCar = Random.Range(0, 2);
		indexStartCar = (indexEndCar == 1) ? 0 : 1;

		spawPositionCar[indexEndCar].SetActive(true);

		float scores = gameManager.intance.getScore() / 5;


		speed = Random.Range(25f + scores, 50f + scores);
		float waitTime = Random.Range(2.0f, 4.0f);

		coroutine = SpawACar(waitTime);
		StartCoroutine(coroutine);
	}
	private IEnumerator SpawACar(float waitTime)
	{
		while (true)
		{

			if (numberCars <= 3)
			{
				GameObject Car = Instantiate(Cars[Random.Range(0, Cars.Length)],
					spawPositionCar[indexStartCar].transform.position, Quaternion.identity);

				float direction = (indexStartCar == 0) ? 1 : -1;
				Car.GetComponent<Car>().init(speed * direction, gameObject);
				numberCars++;
			}
			yield return new WaitForSeconds(waitTime);
			if (numberCars > 3)
			{
				yield return new WaitForSeconds(waitTime);
				numberCars = 0;
			}
		}
	}
	public void effectBlood(Vector3 pPlayer)
	{
		Vector3 position = PracticleSystem.transform.position;
		position.z = pPlayer.z;
	
		PracticleSystem.transform.position = position;

		PracticleSystem.SetActive(true);
	}
	// Update is called once per frame
	void Update () {
		
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawCoin : MonoBehaviour {
	public GameObject Coin;
	public float perCentSpawCoin;
	public int intdextLeght;
	public float offset;
	public Vector3 positionCenter;
	// Use this for initialization
	void Start () {

		if (Random.Range(0, 100) < perCentSpawCoin)
		{
			int numberP = Random.Range(-intdextLeght, intdextLeght + 1);
			GameObject coin = Instantiate(Coin, gameObject.transform.position + positionCenter +
				new Vector3(0, 0, numberP * offset), Quaternion.identity);
			coin.GetComponent<Coin>().init(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

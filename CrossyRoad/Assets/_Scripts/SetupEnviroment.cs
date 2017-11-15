using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupEnviroment : MonoBehaviour {

	public GameObject[] strips;
	public Transform camera;
	//private GameObject[] allStrips;
	private List<GameObject> allStrips;
	public float offset;
	public float depth;
	private const int GRASS = 0, GRASSDARK = 1, ONEWAYSTREET = 2,
		TWOWAYSTREET = 3, TRACKTRAIN = 4, OCEAN = 5;
	private int previousStrip = GRASSDARK;
	private Vector3 positionPreviousStrip;
	public float maxDistance = 89f;
	// Use this for initialization
	private void Awake()
	{
		allStrips = new List<GameObject>();
		int k = 0;
		do
		{
			GameObject gras = (k % 2 == 0) ? strips[GRASSDARK] : strips[GRASS];
			positionPreviousStrip = transform.position + new Vector3(offset * k, 0, 0);

			GameObject grassOb = Instantiate(gras, positionPreviousStrip, Quaternion.identity);
			allStrips.Add(grassOb);
			if (k < 7 && k > 3)
				grassOb.GetComponent<Grass>().GrassMid = true;
			k++;
		} while (k < 7);

		int tam = 8;
		while(tam > 0)
		{
			spaw();
			tam--;
		}
	}
	void Start () {
	}
	private int getStrip()
	{
		int currentStrip = Random.Range(GRASS, OCEAN + 1);
		switch (currentStrip)
		{
			case GRASS:
				return (previousStrip == GRASS) ? GRASSDARK : GRASS;
			case GRASSDARK:
				return (previousStrip == GRASSDARK) ? GRASS : GRASSDARK;
			case TWOWAYSTREET:
				return ((previousStrip == ONEWAYSTREET) || (previousStrip == TWOWAYSTREET)) ? TWOWAYSTREET : ONEWAYSTREET;
			case ONEWAYSTREET:
				return ((previousStrip == ONEWAYSTREET)) ? TWOWAYSTREET : ONEWAYSTREET;
			default:
				return currentStrip;
		}
	}
	private Vector3 getPositionStrip()
	{
		if(previousStrip == GRASS || previousStrip == GRASSDARK)
		{
			positionPreviousStrip.y = 0;
		}else if (previousStrip == ONEWAYSTREET || previousStrip == TWOWAYSTREET || previousStrip == TRACKTRAIN)
		{
			positionPreviousStrip.y = -depth;
		}else
		{
			positionPreviousStrip.y = -2*depth;
		}
		return positionPreviousStrip;
	}
	private void spaw()
	{
		previousStrip = getStrip();
		positionPreviousStrip += new Vector3(offset, 0, 0);
		allStrips.Add(Instantiate(strips[previousStrip], getPositionStrip(), Quaternion.identity));
	}
	private void destroyObject()
	{

		Destroy(allStrips[0]);
		allStrips.Remove(allStrips[0]);
	}
	private void FixedUpdate()
	{
	//	if(allStrips[0] != null)
		if((allStrips[0].transform.position.x - camera.transform.position.x) <= maxDistance)
		{
			spaw();
			destroyObject();
		}
	}
	void Update () {
		
	}
}

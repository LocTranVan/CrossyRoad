using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	// Use this for initialization
	public Transform Player;
	public float speed, speedPlayer;
	private float maxDistance, maxDistanceZ;

	private Vector3 velocity = Vector3.zero, distanceVector;
	private float distanceZ;
	private GameObject SpawEagle;
	private bool spawEagle, isPause = true;
	void Start () {
		maxDistance = Player.position.x - transform.position.x;
		maxDistanceZ = Player.position.z;

		distanceVector = Player.transform.position - transform.position;
		distanceZ = distanceVector.z;
		SpawEagle = GameObject.Find("Enviroment");
	//	velocity = new Vector3(speedPlayer, 0, 0);
	}

	private void FixedUpdate()
	{
		Vector3 distanceP = Player.position - transform.position;
		if (!Player.gameObject.GetComponentInParent<Character>().isDead && !isPause)
		{
			
			transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

			Move(distanceP);
			if(speed < 0 && (distanceP.x  > distanceVector.x - 18f)){
				speed = 0;

			}

		}

		//smoothCamera();

	}
	private void Move(Vector3 distanceP)
	{
		
		
		if ((distanceP.x) > (distanceVector.x + 9f))
		{
			transform.position += new Vector3(speedPlayer * Time.deltaTime, 0, 0);
		}else if(distanceP.x <= (distanceVector.x - 25f))
		{

			SpawEagle.GetComponent<SetupEnviroment>().spawEagle();
			//spawEagle = true;

			speed = -10;
		}
		if ((distanceP.z) > (distanceVector.z + 15f))
		{
			transform.position += new Vector3(0, 0, 8 * Time.deltaTime);
		}else if ((distanceP.z) < (distanceVector.z - 15f))
		{
			transform.position -= new Vector3(0, 0, 8 * Time.deltaTime);
		}

	}
	public void setPause(bool Pause)
	{
		this.isPause = Pause;
	}

	// Update is called once per frame
	void Update () {
			
	}
}

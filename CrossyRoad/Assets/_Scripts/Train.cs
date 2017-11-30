using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour {
	public GameObject[] spawPositionCar;
	public GameObject trains, PracticleSystem;
	public float timeWait;
	private int indexEndTrain, indexStartTrain;
	private float speed;
	private float waitTimeLight, waitTimeChange;
	private IEnumerator coroutine;

	public Renderer lightLeft, lightRight;
	public Material lightTurnOn, lightTurnOff;
	public Transform bangDen;
	public Light spotLight;


	private bool turnLight, side;
	private Material defaultMaterial;
	// AudioSourec
	private AudioSource audioSource;
	public AudioClip chuong;
	// Use this for initialization
	private void Awake()
	{
		//defaultMaterial = GetComponentInChildren<Renderer>().material;
	}
	void Start () {
		Material material = gameManager.intance.GetMaterial();
		audioSource = GetComponent<AudioSource>();
		if (material != null)
		{
			GetComponentInChildren<Renderer>().material = material;
		}

		indexEndTrain = Random.Range(0, 2);
		indexStartTrain = (indexEndTrain == 1) ? 0 : 1;

		spawPositionCar[indexEndTrain].SetActive(true);

		speed = Random.Range(300f, 600f);
		float waitTime = Random.Range(timeWait, 2*timeWait);


		coroutine = spawTrain(waitTime);
		StartCoroutine(coroutine);
	}
	private IEnumerator spawTrain(float waitTime)
	{
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			setUpForTrain();
			yield return new WaitForSeconds(3f);
			GameObject train =	Instantiate(trains, spawPositionCar[indexStartTrain].transform.position, Quaternion.identity);
			//audioSource.loop = false;
			//audioSource.Stop();
			float direction = (indexStartTrain == 0) ? 1 : -1;
			train.GetComponent<Car>().init(speed * direction, gameObject);
		}
	}
	public void effectBlood(Vector3 pPlayer)
	{
		Vector3 position = PracticleSystem.transform.position;
		position.z = pPlayer.z;

		PracticleSystem.transform.position = position;

		PracticleSystem.SetActive(true);
	}
	private void setUpForTrain()
	{
		AudioCoiXe();
		Invoke("AudioCoiXe", 1f);
		turnLight = true;
		waitTimeLight = Time.time;
		side = true;
		spotLight.intensity = 8;

		Vector3 position = bangDen.transform.position;
		position.y += 1;
		bangDen.position = position;
	}
	private void AudioCoiXe()
	{
		audioSource.PlayOneShot(chuong, 0.5f);
	}
	private void FixedUpdate()
	{
		if (turnLight)
		{
			if (Time.time > waitTimeLight + 3)
			{
				turnLight = false;
				spotLight.intensity = 1;
				lightLeft.material = lightTurnOff;
				lightRight.material = lightTurnOff;

				Vector3 position = bangDen.transform.position;
				position.y -= 1;
				bangDen.position = position;

			}
			else
			{
				if (Time.time > waitTimeChange + 0.5f)
				{
					if (side)
					{
						lightLeft.material = lightTurnOn;
						lightRight.material = lightTurnOff;
					}else
					{
						lightLeft.material = lightTurnOff;
						lightRight.material = lightTurnOn;
					}
					side = !side;
					waitTimeChange = Time.time;
				}

			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}

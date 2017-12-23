using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour {

	// Use this for initialization
	public float speedEagle;
	private GameObject player;
	private bool hitPlayer;
	private Vector3 pLast;
	public AudioClip hitAudio;
	private AudioSource audioSource;
	void Start () {
		pLast = transform.position;
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position -= new Vector3(speedEagle, 0, 0) * Time.deltaTime;
		if (hitPlayer)
		{
			Vector3 position = player.transform.position;

			position.x = transform.position.x;
			player.transform.position = position;

		}
		if(transform.position.x < pLast.x - 150)
		{
			player.GetComponentInParent<Character>().isDead = true;
			player.GetComponentInParent<Character>().isPause = false;
			Destroy(gameObject);
			
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "uniHorse")
		{
			Debug.Log("hit");
			audioSource.PlayOneShot(hitAudio);
			hitPlayer = true;
			player = other.gameObject;
			Character tf = player.GetComponentInParent<Character>();
			tf.TakeDamage();
		}
	}
}

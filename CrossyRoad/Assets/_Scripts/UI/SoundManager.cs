using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	private AudioSource audioSource;
	public AudioClip btTapIn, btTapOut, btTapWrong;
	public AudioClip btChoosePlayer;
	public static SoundManager intance;
	private AudioClip nextSounds;
	// Use this for initialization
	void Start () {
		if (intance == null)
			intance = this;
		else Destroy(gameObject);

		audioSource = GetComponent<AudioSource>();
	}

	public void soudBTap()
	{
		audioSource.PlayOneShot(btTapIn);
		nextSounds = btTapOut;
		Invoke("soudBTapOut", btTapIn.length /2);
	}
	public void soundBTChoosePlayer()
	{
		audioSource.Stop();
		audioSource.PlayOneShot(btChoosePlayer);
	}
	private void soudBTapOut()
	{
		audioSource.PlayOneShot(nextSounds);
	}
	// Update is called once per frame
	void Update () {
		
	}
}

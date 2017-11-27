using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Coin : MonoBehaviour {
	private GameObject objectFather;
	public LayerMask Tree;
	private bool move;
	private Vector3 posFather;

	// Use this for initialization
	void Start () {
		bool hit = Physics.CheckSphere(transform.position, 1, Tree);
		if (hit)
			Destroy(gameObject);
	}
	public void init(GameObject objectFather)
	{
		this.objectFather = objectFather;
		if (objectFather.tag == "Wood")
		{
			move = true;
			posFather = transform.position - objectFather.transform.position;
		}
	}
	// Update is called once per frame
	void Update () {
		if(objectFather == null)
			Destroy(gameObject);
	}
	public void prepareDisapear()
	{
		Destroy(gameObject);
	}
	public void destroyObject()
	{
		Destroy(gameObject);
	}
	private void FixedUpdate()
	{
		if (move && objectFather != null)
		{
			Vector3 position = transform.position;
			position.z = objectFather.transform.position.z + posFather.z;
			transform.position = position;
		}
	}
}

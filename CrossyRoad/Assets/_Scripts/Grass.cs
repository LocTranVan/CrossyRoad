using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour {

	public Transform MilestonesRight, MilestonesLeft, MilestonesMid;
	public GameObject[] Tree;
	public float offset;
	public bool GrassMid;
	private void Awake()
	{

	}
	void Start () {
		spawTree(Instantiate(Tree[Random.Range(2, 4)], MilestonesRight.position, Quaternion.identity));
		spawTree(Instantiate(Tree[Random.Range(2, 4)], MilestonesLeft.position, Quaternion.identity));

		int k = 5;

		while (k >= 1)
		{
			spawTree(Instantiate(Tree[Random.Range(0, 5)], MilestonesRight.position - new Vector3(0, 0, k * offset), Quaternion.identity));
			spawTree(Instantiate(Tree[Random.Range(0, 5)], MilestonesLeft.position + new Vector3(0, 0, k * offset), Quaternion.identity));

			if (!GrassMid)
				spawTree(Instantiate(Tree[Random.Range(0, 5)], MilestonesMid.position + new Vector3(0, 0, k * offset) * Mathf.Pow(-1, Random.Range(0, 2)), Quaternion.identity));

			k--;
		}
	}
	private void spawTree(GameObject objectFather)
	{
		objectFather.GetComponent<Tree>().init(gameObject);
	}
	void Update () {

	}
	private void OnBecameInvisible()
	{
		
	}
}

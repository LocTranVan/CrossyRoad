using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour {

	public Transform MilestonesRight, MilestonesLeft, MilestonesMid;
	public GameObject[] Tree;
	public float offset;
	public bool GrassMid;

	private Material defaulMaterial;
	private string introlDution = "";
	private bool startSpaw = false;
	private void Awake()
	{
		defaulMaterial = GetComponent<Renderer>().material;
	}
	void Start () {
		Material material = gameManager.intance.GetMaterial();
		if (material != null)
		{
			GetComponent<Renderer>().material = material;
		}
	//	else
			//GetComponent<Renderer>().material = defaulMaterial;

		spawTree(Instantiate(Tree[Random.Range(2, 4)], MilestonesRight.position, Quaternion.identity));
		spawTree(Instantiate(Tree[Random.Range(2, 4)], MilestonesLeft.position, Quaternion.identity));
			int k = 5;

			while (k >= 1)
			{
				spawTree(Instantiate(Tree[Random.Range(0, 5)], MilestonesRight.position - new Vector3(0, 0, k * offset), Quaternion.identity));
				spawTree(Instantiate(Tree[Random.Range(0, 5)], MilestonesLeft.position + new Vector3(0, 0, k * offset), Quaternion.identity));

				if (!GrassMid && introlDution == "")
					spawTree(Instantiate(Tree[Random.Range(0, 5)], MilestonesMid.position + new Vector3(0, 0, k * offset) * Mathf.Pow(-1, Random.Range(0, 2)), Quaternion.identity));
				else if(introlDution != "" && !startSpaw)
					setIntrolSpawTree();
				k--;
			}	
	}
	private void setIntrolSpawTree()
	{
		Debug.Log(introlDution);
		int k = introlDution.Length;
		char[] word = introlDution.ToCharArray();
		for(int i = 0; i < k; i++)
		{
			if(word[i] == '1')
				spawTree(Instantiate(Tree[Random.Range(0, 4)], MilestonesLeft.position - new Vector3(0, 0, (i + 1) * offset), Quaternion.identity));
			
		}
		startSpaw = true;
	}
	private void spawTree(GameObject objectFather)
	{
		objectFather.GetComponent<Tree>().init(gameObject);
	}
	public void setIntroldution(string introl)
	{
		introlDution = introl;
	}
	void Update () {

	}
	private void OnBecameInvisible()
	{
		
	}
}

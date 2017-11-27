using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public TerrainType[] walkableRegions;
	Dictionary<int,int> walkableRegionsDictionary = new Dictionary<int, int>();
	LayerMask walkableMask;
	public LayerMask cars, wood;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;
	private int numberAcces;

	private int roadCarMin;
	void Awake() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/(nodeDiameter + 1));
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);

		foreach (TerrainType region in walkableRegions) {
			walkableMask.value |= region.terrainMask.value;
			walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value,2),region.terrainPenalty);
		}

	}
	private void Start()
	{
		//StartCoroutine(UpdateGrid());

	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}


	public void CreateGrid(Vector3 startPos)
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * (nodeDiameter + 1) + nodeRadius + 0.5f) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
				Node Seeker = NodeFromWorldPoint(startPos);
				RaycastHit hit;

				if (walkable)
				{
				
					if (Physics.Raycast(ray, out hit, 100, cars))
					{
						//if (Mathf.Abs(worldPoint.x - startPos.x) <= 18)
						//{
							walkable = false;
						//}
					} 
				}else 
				if (Physics.Raycast(ray, out hit, 100, wood))
				{
					//if (Mathf.Abs(worldPoint.x - startPos.x) <= 18)
					//{
					walkable = true;
					//}
				}

				grid[x, y] = new Node(walkable, worldPoint, x, y, 0);
			}
		}
		test(startPos);

	
	}
	public void test(Vector3 startPos)
	{
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				if (!grid[x, y].walkable)
				{
					Ray ray = new Ray(grid[x, y].worldPosition + Vector3.up * 50, Vector3.down);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 100, cars))
					{
					//	grid[x, y].walkable = true;
						Node Seeker = NodeFromWorldPoint(startPos);
						//		if (Mathf.Abs(x - Seeker.gridX) <= 5)
						//	{
						//grid[x, y].walkable = true;
							float distanceY = Mathf.Abs(Seeker.worldPosition.x - grid[x, y].worldPosition.x) + 9;

							float speedCar = hit.collider.gameObject.GetComponent<Car>().getSpeed();
						//Vector3 pCar = hit.collider.gameObject.transform.position;
							Vector3 pCar = grid[x, y].worldPosition;
							//float distance = 18 * speedCar / 30 ;
						   float distance = distanceY * 8 * 30 / (9 * speedCar);
							//Debug.Log(distance);

							//grid[x, y].walkable = true;
							float percentSpeed = speedCar / 10;
							pCar = pCar - new Vector3(0, 0, distance);
							Node pCarNew = NodeFromWorldPoint(pCar);
							pCarNew.walkable = false;

						if (speedCar > 0)
							{
							
								//if(y > 1)
								//grid[x, y - 1].movementPenalty += 1;
									if(y < gridSizeY - 1)
									grid[x, y + 1].movementPenalty -= 15;
								if (y < gridSizeY - 2)
								grid[x, y + 2].movementPenalty -= 5;
							}
							else
							{
							//	pCar = pCar + new Vector3(0, 0, distance);
							//	Node pCarNew = NodeFromWorldPoint(pCar);
							//	pCarNew.walkable = false;
									if (y > 1)
										grid[x, y - 1].movementPenalty -= 15;
									if (y > 2)
										grid[x, y - 2].movementPenalty -= 5;
								//if (y < gridSizeY - 1)
								//	grid[x, y + 1].movementPenalty += 1;

							}
					//	}
					//	else
						//{
						//	grid[x, y].walkable = true;
						//}
					}
				}
			}
		}
		
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if ((x == 0 && y == 0) || ((Mathf.Abs(x )== 1 && Mathf.Abs(y) == 1)))
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = ((worldPosition.x - transform.position.x) + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z - transform.position.z + gridWorldSize.y/2) / gridWorldSize.y;
		

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
					
				//Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f) + new Vector3(1, 0, 0));
			}
		}
	}


	[System.Serializable]
	public class TerrainType {
		public LayerMask terrainMask;
		public int terrainPenalty;
	}


}
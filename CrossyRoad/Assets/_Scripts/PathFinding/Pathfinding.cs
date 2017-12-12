using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour {
	
	PathRequestManager requestManager;
	Grid grid;

	
	void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}
	
	
	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		grid.CreateGrid(startPos);
		StartCoroutine(FindPath(startPos,targetPos));
	//	StartCoroutine(FindPathBFS(startPos, targetPos));
	}
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
		
		Stopwatch sw = new Stopwatch();
		sw.Start();
		
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;
		
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);
		startNode.parent = startNode;
		Node tam = startNode;
		int maxCost = 0;
	//	if (startNode.walkable) {
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);
			
			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst();
				
			
				closedSet.Add(currentNode);
				
				if (currentNode == targetNode) {
					sw.Stop();
					//print ("Path found: " + sw.ElapsedMilliseconds + " ms");
					pathSuccess = true;
					break;
				}
				
				
				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour.walkable || closedSet.Contains(neighbour)) {
						continue;
					}
					
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
					
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						if (maxCost >= neighbour.hCost)
						{
							maxCost = neighbour.hCost;
							tam = neighbour;
						}

						if (!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
						}
						else
							openSet.UpdateItem(neighbour);
					}
				}
		//	}
		}
		yield return null;
		if (pathSuccess) {
			waypoints = RetracePath(startNode,targetNode);
		}else
		{
			pathSuccess = true;
			waypoints = RetracePath(startNode, tam);
		}


		requestManager.FinishedProcessingPath(waypoints,pathSuccess);
		
	}

	IEnumerator FindPathBFS(Vector3 startPos, Vector3 targetPos)
	{
		//Stopwatch sw = new Stopwatch();
		//sw.Start();

		//Vector3[] waypoints = new Vector3[0];
	//	bool pathSuccess = false;
		var visited = new HashSet<Node>();

		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node lastTarget = startNode;
		float targetPort = GetDistance(startNode, grid.NodeFromWorldPoint(targetPos));
		startNode.parent = startNode;

		var queue = new Queue<Node>();
		queue.Enqueue(startNode);
		while(queue.Count > 0)
		{
			Node currentNode = queue.Dequeue();
			if (visited.Contains(currentNode))
				continue;
			visited.Add(currentNode);
			//float dis = GetDistance(currentNode, grid.NodeFromWorldPoint(targetPos));
		//	if (targetPort >= dis)
			//{
				//targetPort = dis;
				lastTarget = currentNode;
		//	}
		//	lastTarget = currentNode;
			foreach(Node neighbor in grid.GetNeighbours(currentNode))
			{
				if (!visited.Contains(neighbor) && neighbor.walkable)
					queue.Enqueue(neighbor);
			}

		}

		//if (visited.Contains(grid.NodeFromWorldPoint(targetPos)) && targetPos != startPos)
		//	lastTarget = grid.NodeFromWorldPoint(targetPos);
	//	else
	//	{
	//		print("nope");
	//	}
		List<Node> path = FindShortestPath(lastTarget, startNode);
		List<Vector3> woldPoint = new List<Vector3>();

		for(int i = 0; i < path.Count; i++)
		{
			woldPoint.Add(path[i].worldPosition);
		}


	    yield return null;
		requestManager.FinishedProcessingPath(woldPoint.ToArray(), true);
	
	}
	private List<Node> FindShortestPath(Node tarGet, Node start)
	{
		//Node startNode = grid.NodeFromWorldPoint(startPos);

		start.parent = start;

		var queue = new Queue<Node>();
		queue.Enqueue(start);
		var previous = new Dictionary<Node, Node>();

		while (queue.Count > 0)
		{
			Node currentNode = queue.Dequeue();
		
			foreach (Node neighbor in grid.GetNeighbours(currentNode))
			{
				if (previous.ContainsKey(neighbor) || !neighbor.walkable)
					continue;

				previous[neighbor] = currentNode;
				queue.Enqueue(neighbor);
			}

		}
		var path = new List<Node>();
		var current = tarGet;
		while (!current.Equals(start))
		{
			path.Add(current);
			current = previous[current];
		}
		path.Add(start);
		path.Reverse();
		return path;
		
	}

		Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;
		
	}
	
	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			//if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			//}else
			//{
				//print("different way" + i + "")
			//}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		//if (dstX > dstY)
		//return 14*dstY + 10* (dstX-dstY);
		//return 14*dstX + 10 * (dstY-dstX);
		return 10 * (dstX + dstY);
	}
	
	
}

using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public GameObject pCharacter;
	public Transform target;
	private Vector3 target2;
	float speed = 50;
	Vector3[] path;
	int targetIndex;
	Vector3 endPosition;
	Vector3 newPosition;
	bool finding;
	private Animator animator;
	private Rigidbody rigidbody;
	private Character charLion;
	private bool isDead;
	void Start() {
		//	StartCoroutine(updatePath());
		//PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		animator = pCharacter.GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody>();
		charLion = GetComponent<Character>();
		target2 = target.position;
		endPosition = transform.position;
		StartCoroutine(updatePath());
	}
	IEnumerator updatePath()
	{
		while (true)
		{
			//if (!isDead)
			//{
				yield return new WaitForSeconds(.1f);
				PathRequestManager.RequestPath(transform.position, target2, OnPathFound);
				yield return new WaitForSeconds(.2f);
			//}
		}
	}


	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (!isDead)
		{
			if (pathSuccessful)
			{
				path = newPath;
				targetIndex = 0;
				if (path.Length > 0)
				{
					StopCoroutine("FollowPath");
					StartCoroutine("FollowPath");
				}
			}
			else
			{

				if (target2.x > transform.position.x)
				{
					PathRequestManager.RequestPath(transform.position, target2 + Vector3.left * 80, OnPathFound);
				}
				else
				{
					PathRequestManager.RequestPath(transform.position, target2 + Vector3.right * 80, OnPathFound);

				}

				//Debug.Log("Path don't succes");
			}
			finding = false;
		}
	}
	

	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];
	
		//while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
		endPosition = currentWaypoint;
		//Debug.Log(transform.position + " " + currentWaypoint);
		//Debug.Log(transform.position - endPosition);
		Vector3 direction = endPosition - transform.position;
		if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
		{
			Animating(0, direction.x, false);
		}
		else
		{
			Animating(direction.z, 0, false);
		}

	
		//	transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;
		

		//}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Cars")
		{
			Vector3 position = transform.position;
			Debug.Log("hit car");
			StopCoroutine(updatePath());
			TakeDamage();
		}
	}
	public void TakeDamage()
	{
		Physics.IgnoreLayerCollision(9, 12);
		Physics.IgnoreLayerCollision(8, 12);
		rigidbody.velocity = Vector3.zero;

		Animating(0, 0, true);
		isDead = true;
		//timeScale = Time.time;
	}
	public void ResetLife()
	{
		isDead = false;
		Animating(0, 0, false);
		StopCoroutine(updatePath());
		Physics.IgnoreLayerCollision(9, 12, false);
		Physics.IgnoreLayerCollision(8, 12, false);
	}
	private void FixedUpdate()
	{
		if (!isDead)
		{
			transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);


			if (transform.position == endPosition && transform.position != target.position && !finding)
			{
				//		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
				finding = true;
			}
			if (transform.position.x <= target.position.x)
			{
				target2 = target.position + new Vector3(90, 0, 0);
			}
		}
	}
	private void Animating(float h, float v, bool dead)
	{
		animator.SetFloat("Horizontal", -h);
		animator.SetFloat("Vertical", v);
		animator.SetBool("Dead", dead);
	}
	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}

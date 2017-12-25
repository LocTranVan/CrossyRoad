using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Enemy : MonoBehaviour
{

	// Use this for initialization
	public float speed, speed2, weightPlayer;
	public GameObject player;
	public LayerMask Tree, Cars;
	private float startTime, startTimeScale;

	private bool jump = false, couldJump = true;
	private Vector3 startMarker = Vector3.zero, endMarker = Vector3.zero;
	public AnimationCurve acc;
	private float journeyLength, journeyJump;
	private Rigidbody rigidbody;
	private Animation anim;
	private MeshRenderer mesh;
	int targetIndex;
	private float number = 0;
	private bool shrug = false, IsDead, IsGround;
	//private static Character intance;
	private GameObject carHit;
	private Transform target;
	private Vector3 target2;
	bool finding;
	Vector3[] path;
	public bool isDead
	{
		get
		{
			return IsDead;
		}
		set
		{
			this.IsDead = value;
		}
	}
	Vector3 endPosition;
	private Vector3 disJumpVertical = new Vector3(9, 0, 0),
		disJumpHorizontal = new Vector3(0, 0, 8);
	private Vector3 endJump = new Vector3(1, 1, 1), startJump = new Vector3(1, 0.8f, 1);
	// tranform character when hit.
	private Vector3 endScaleForWard = new Vector3(0.01f, 1, 1), endScaleUpSide = new Vector3(1, 0.05f, 1), LeftScaleSide = new Vector3(1, 1, 0.01f);
	private float journeyScale, timeScale;
	private bool hitTopSide, hitLeftSide, hitRightSide;

	private bool preesKey = false;
	private Vector3 velocityHit;
	private Pathfinding p;
	private GameObject YardPath;

	private Stopwatch sw;
	private void Awake()
	{
		p = GetComponent<Pathfinding>();
	}
	void Start()
	{
		sw = new Stopwatch();
		journeyLength = Vector3.Distance(Vector3.zero, disJumpHorizontal);
		journeyJump = Vector3.Distance(startJump, endJump);
		journeyScale = Vector3.Distance(new Vector3(1, 1, 1), endScaleForWard);

		GameObject PlayerCharacter = GameObject.FindGameObjectWithTag("Player");

		target = PlayerCharacter.GetComponent<Transform>();
		PlayerCharacter.GetComponent<Character>().setActiveYardPath();


		mesh = GetComponentInChildren<MeshRenderer>();

		rigidbody = GetComponent<Rigidbody>();
		anim = player.GetComponentInChildren<Animation>();
		target2 = target.position + new Vector3(90, 0, 0);

		endPosition = transform.position;
		//	PathRequestManager.RequestPath(transform.position, target2, OnPathFound);

		gameObject.GetComponentInChildren<MeshFilter>().mesh = target.gameObject.GetComponentInChildren<MeshFilter>().mesh;
		gameObject.GetComponentInChildren<Renderer>().material = target.gameObject.GetComponentInChildren<Renderer>().material;

		StartCoroutine(updatePath());
	}
	void Update()
	{
		/*
		if (!jump)
			HandleInput();
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			setUpForShrug();
			preesKey = !preesKey;
		} */
		if (transform.position.x >= (target.position.x + 90))
		{
			target2 = target.position;
		}
		else if (transform.position.x < target.position.x)
		{
			target2 = target.position + Vector3.right * 80;
		}

	}
	public void ResetLife()
	{
		isDead = false;
		transform.position = target.position;
		//StopCoroutine(updatePath());
		StopAllCoroutines();
		StartCoroutine(updatePath());
		transform.localScale = new Vector3(1, 1, 1);
		Physics.IgnoreLayerCollision(9, 12, false);
		Physics.IgnoreLayerCollision(8, 12, false);

	}
	private void setUpForShrug()
	{
		startTimeScale = Time.time;
		shrug = !shrug;
	}
	private void Move()
	{
		float disCovered = (Time.time - startTime) * speed;
		float fracJourney = disCovered / journeyLength;

		transform.position = Vector3.Lerp(startMarker, endMarker, acc.Evaluate(fracJourney));


		if (transform.position == endMarker)
		{
			jump = false;
			if (!finding)
			{
				//PathRequestManager.RequestPath(transform.position, target2, OnPathFound);
				//	StartCoroutine(updatePath());
				finding = true;

			}
		}
	}
	private bool checkCounldForJump(Vector3 startMark, Vector3 endMark)
	{
		//	bool check = (!Physics.Linecast(startMark + new Vector3(0, 2, 0), endMark + new Vector3(0, 2, 0), Tree)) ? true : false;
		bool check = true;
		if (check)
		{
			anim.Play("Jump");
			endMarker.x = Mathf.Round(endMarker.x / 9) * 9;
			endMarker.z = Mathf.Round(endMarker.z / 8) * 8;
			jump = false;
		}
		return check;


	}
	private void setUpForJump(Vector3 angleRotate)
	{
		setUpForShrug();
		startMarker = transform.position;
		startTime = Time.time;
		setRotatiton(angleRotate);

	}
	private void setRotatiton(Vector3 angle)
	{
		mesh.transform.localEulerAngles = angle;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Cars")
		{
			Vector3 position = transform.position;
			//	Debug.Log("hit car");
			carHit = other.gameObject;

			velocityHit = new Vector3(0, 0, -carHit.transform.position.z + position.z);
			TakeDamage();

		}
		//if (other.gameObject.tag == "Ocean")
			//Debug.Log("ocean");
	}
	public void TakeDamage()
	{
		gameManager.intance.ActiveBt();
		hitTopSide = Physics.Linecast(transform.position, transform.position + disJumpVertical * 1 / 2, Cars);
		hitLeftSide = Physics.Linecast(transform.position, transform.position - disJumpHorizontal * 1 / 2, Cars);
		hitRightSide = Physics.Linecast(transform.position, transform.position + disJumpHorizontal * 1 / 2, Cars);

		Physics.IgnoreLayerCollision(9, 12);
		Physics.IgnoreLayerCollision(8, 12);
		rigidbody.velocity = Vector3.zero;


		IsDead = true;
		//timeScale = 0;
		StopCoroutine(updatePath());
	}
	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
	IEnumerator updatePath()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			PathRequestManager.RequestPath(transform.position, target2, OnPathFound);
		}

	}
	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
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
		}
	}
	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0];
		//Debug.Log(path.Length);
		//Vector3 currentWaypoint = (path.Length > 1) ? path[1] : path[0];

		//while (true) {
		if (transform.position == currentWaypoint)
		{
			targetIndex++;
			if (targetIndex >= path.Length)
			{
				yield break;
			}
			currentWaypoint = path[targetIndex];
		}
		endPosition = currentWaypoint;

		Vector3 direction = endPosition - transform.position;

		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
		{
			//Animating(0, direction.x, false);
			Fake();
			if (direction.x > 0)
			{
				setUpForJump(new Vector3(0, -90, 0));
				endMarker = startMarker + disJumpVertical;
				//jump = checkCounldForJump(startMarker, endMarker);
			}
			else
			{
				setUpForJump(new Vector3(0, 90, 0));
				endMarker = startMarker - disJumpVertical; ;
			}
			//jump = checkCounldForJump(startMarker, endMarker);
		}
		else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.z))
		{
			Fake();
			if (direction.z < 0)
			{
				setUpForJump(new Vector3(0, 0, 0));
				endMarker = startMarker - disJumpHorizontal;
				jump = checkCounldForJump(startMarker, endMarker);
			}
			else
			{
				setUpForJump(new Vector3(0, 180, 0));
				endMarker = startMarker + disJumpHorizontal;

			}
			//Animating(direction.z, 0, false);

			//jump = checkCounldForJump(startMarker, endMarker);
		}
		jump = checkCounldForJump(startMarker, endMarker);


		finding = false;
		//Stopwatch sw = new Stopwatch();
		sw.Start();
		//	transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
		yield return null;


		//}
	}
	private void Fake()
	{
		setUpForShrug();
		preesKey = !preesKey;
	}
	void FixedUpdate()
	{
		//	Debug.DrawLine(transform.position, transform.position + disJumpVertical * 1 / 2, Color.red);
		//	Debug.DrawLine(transform.position, transform.position - disJumpHorizontal * 1 / 2, Color.red);
		//	Debug.DrawLine(transform.position, transform.position + disJumpHorizontal * 1 / 2, Color.red);
		//	TakeDamage();
		if (IsDead)
		{
			//rigidbody.isKinematic = true;
			float timeS = (Time.time - timeScale) * 7;
			float fracJourney = timeS / journeyScale;
			Vector3 endScale = (hitTopSide) ? endScaleForWard : endScaleUpSide;

			transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), endScale, fracJourney);
			if (endScale == endScaleForWard)
			{
				//rigidbody.isKinematic = true;
				//rigidbody.useGravity = false;
				if (carHit != null)
					transform.position = new Vector3(transform.position.x, transform.position.y, (carHit.transform.position + velocityHit).z);
				//	Debug.Log(rigidbody.velocity);

			}
		}
		else
		{

			if (jump)
			{
				Move();
				rigidbody.isKinematic = true;


			}
			else
			{
				rigidbody.isKinematic = false;
				Vector3 position = transform.position;
				position.y -= weightPlayer * Time.deltaTime;
				transform.position = position;
			}
			if (preesKey)
			{
				//	Debug.Log("Presskey");

				float timeS = (Time.time - startTimeScale) * speed2;
				float fracJourney = timeS / journeyJump;
				if (shrug)
				{
					transform.localScale = Vector3.Lerp(endJump, startJump, acc.Evaluate(fracJourney));
					//	Debug.Log("Shrug");
				}
				else
				{
					transform.localScale = Vector3.Lerp(startJump, endJump, acc.Evaluate(fracJourney));
					preesKey = (transform.localScale == endJump) ? false : true;
				}
			}
		}
	}

}

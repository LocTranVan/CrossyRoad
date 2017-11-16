using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

	// Use this for initialization
	public float speed, speed2;
	public GameObject player;
	public LayerMask Tree, Cars;
	private float startTime, startTimeScale;
	//private Rigidbody rigidbody;
	private bool jump = false, couldJump = true;
	private Vector3 startMarker = Vector3.zero, endMarker = Vector3.zero;
	public AnimationCurve acc;
	private float journeyLength, journeyJump;
	private Rigidbody rigidbody;
	private Animation anim;
	private MeshRenderer mesh;
	private float number = 0;
	private bool shrug = false, IsDead, IsGround;

	private Vector3 disJumpVertical = new Vector3(9, 0, 0),
		disJumpHorizontal = new Vector3(0, 0, 8);
	private Vector3 endJump = new Vector3(1, 1, 1), startJump = new Vector3(1, 0.8f, 1);
	// tranform character when hit.
	private Vector3 endScaleForWard = new Vector3(0.01f, 1, 1), endScaleUpSide = new Vector3(1, 0.01f, 1), LeftScaleSide = new Vector3(1, 1, 0.01f);
	private float journeyScale, timeScale;
	private bool hitTopSide, hitLeftSide, hitRightSide;

	private bool preesKey = false;
	private Vector3 velocityHit;
	void Start()
	{
		journeyLength = Vector3.Distance(Vector3.zero, disJumpHorizontal);
		journeyJump = Vector3.Distance(startJump, endJump);
		journeyScale = Vector3.Distance(new Vector3(1, 1, 1), endScaleForWard);

		mesh = player.GetComponent<MeshRenderer>();
		rigidbody = GetComponent<Rigidbody>();
		anim = player.GetComponent<Animation>();
	}
	void Update()
	{
		if (!jump)
			HandleInput();

		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			setUpForShrug();
			preesKey = !preesKey;
		}
	}
	private void HandleInput()
	{

		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			setUpForJump(new Vector3(0, -90, 0));
			endMarker = startMarker + disJumpVertical;

			jump = checkCounldForJump(startMarker, endMarker);
		}
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			setUpForJump(new Vector3(0, 90, 0));
			endMarker = startMarker - disJumpVertical; ;

			jump = checkCounldForJump(startMarker, endMarker);
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			setUpForJump(new Vector3(0, 0, 0));
			endMarker = startMarker - disJumpHorizontal;
			jump = checkCounldForJump(startMarker, endMarker);
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			setUpForJump(new Vector3(0, 180, 0));
			endMarker = startMarker + disJumpHorizontal;

			jump = checkCounldForJump(startMarker, endMarker);
		}
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
		}


	}
	private bool checkCounldForJump(Vector3 startMark, Vector3 endMark)
	{
		bool check = (!Physics.Linecast(startMark, endMark, Tree)) ? true : false;
		//Debug.DrawLine(startMark, endMark, Color.red);
		if (check)
		{
			anim.Play("Jump");
			//		IsGround = false;
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
		//Debug.Log(other.gameObject);
	

		if (other.gameObject.tag == "Cars")
		{

			rigidbody.isKinematic = true;
			Physics.IgnoreLayerCollision(9, 10);
			Physics.IgnoreLayerCollision(8, 10);
			TakeDamage();
			velocityHit = other.gameObject.GetComponent<Rigidbody>().velocity;
		}
	}
	private void TakeDamage()
	{
		hitTopSide = Physics.Linecast(transform.position, transform.position + disJumpVertical * 1 / 2, Cars);
		hitLeftSide = Physics.Linecast(transform.position, transform.position - disJumpHorizontal * 1 / 2, Cars);
		hitRightSide = Physics.Linecast(transform.position, transform.position + disJumpHorizontal * 1 / 2, Cars);

		IsDead = true;
		timeScale = Time.time;
	}
	void FixedUpdate()
	{
		//	Debug.DrawLine(transform.position, transform.position + disJumpVertical * 1 / 2, Color.red);
		//	Debug.DrawLine(transform.position, transform.position - disJumpHorizontal * 1 / 2, Color.red);
		//	Debug.DrawLine(transform.position, transform.position + disJumpHorizontal * 1 / 2, Color.red);
		//	TakeDamage();
		if (IsDead)
		{
			float timeS = (Time.time - timeScale) * 5;
			float fracJourney = timeS / journeyScale;
			Vector3 endScale = (hitTopSide) ? endScaleForWard : endScaleUpSide;

			transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), endScale, fracJourney);
			if (endScale == endScaleForWard)
			{
				rigidbody.isKinematic = false;
				rigidbody.velocity = velocityHit;
				//	Debug.Log(rigidbody.velocity);
			}

		}
		else
		{
			if (jump)
				Move();
			else if (transform.position.x % 9 != 0 || transform.position.z % 8 != 0)
			{

			}


			if (preesKey)
			{
				float timeS = (Time.time - startTimeScale) * speed2;
				float fracJourney = timeS / journeyJump;
				if (shrug)
					transform.localScale = Vector3.Lerp(endJump, startJump, acc.Evaluate(fracJourney));
				else
				{
					transform.localScale = Vector3.Lerp(startJump, endJump, acc.Evaluate(fracJourney));
					preesKey = (transform.localScale == endJump) ? false : true;
				}
			}
		}
	}

}



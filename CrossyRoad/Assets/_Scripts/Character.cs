using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

	// Use this for initialization
	public float speed, speed2;
	public GameObject player;
	public LayerMask Tree;
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
	private bool shrug = false;
	private Vector3 disJumpVertical = new Vector3(9, 0, 0),
		disJumpHorizontal = new Vector3(0, 0, 8);
	private Vector3 endJump = new Vector3(1, 1, 1), startJump = new Vector3(1, 0.8f, 1);
	private bool preesKey = false;
	void Start()
	{
		journeyLength = Vector3.Distance(Vector3.zero, disJumpHorizontal);
		journeyJump = Vector3.Distance(startJump, endJump);
		mesh = player.GetComponent<MeshRenderer>();
		rigidbody = player.GetComponent<Rigidbody>();
		anim = player.GetComponent<Animation>();
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			setUpForShrug();
			preesKey = !preesKey;
		}
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			setUpForJump(new Vector3(0, -90, 0));
			endMarker = startMarker + disJumpVertical;

			jump =	checkCounldForJump(startMarker, endMarker);
		}
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			setUpForJump(new Vector3(0, 90, 0));
			endMarker = startMarker - disJumpVertical;;

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

		if(jump) { 
			Move();
		}
		if (preesKey)
		{
			float timeS = (Time.time - startTimeScale) * speed2;
			float fracJourney = timeS / journeyJump;
			if (shrug)
			{
				transform.localScale = Vector3.Lerp(endJump, startJump, acc.Evaluate(fracJourney));
			}
			else
			{
				transform.localScale = Vector3.Lerp(startJump, endJump, acc.Evaluate(fracJourney));
				preesKey = (transform.localScale == endJump) ? false : true;
			}
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
		bool check = !Physics.Linecast(startMark, endMark, Tree) ? true : false;
		//Debug.DrawLine(startMark, endMark, Color.red);
		if(check)
			anim.Play("Jump");
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
	
	void OnCollisionEnter(Collision other)
	{


	}
	private void OnTriggerEnter(Collider other)
	{

	}
	void FixedUpdate()
	{
	}

}



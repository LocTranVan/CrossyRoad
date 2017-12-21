using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Character : MonoBehaviour
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
	private float number = 0;
	private bool shrug = false, IsDead = false, IsGround, IsPause = true;
	//private static Character intance;
	private GameObject carHit;
	public GameObject snowFalling;
	public GameObject YardPath;
	private Vector3 maxPosition = Vector3.zero;
	private Vector3 touchOrigin = Vector3.zero;
	private Texture2D snapshot;
	//Audio
	private AudioSource audioSource;
	public AudioClip jumpAudio, carHitAudio, getCoin;
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
	public bool isPause
	{
		get
		{
			return IsPause;
		}
		set
		{
			this.IsPause = value;
		}
	}

	private Vector3 disJumpVertical = new Vector3(9, 0, 0),
		disJumpHorizontal = new Vector3(0, 0, 8);
	private Vector3 endJump = new Vector3(1, 1, 1), startJump = new Vector3(1, 0.8f, 1);
	// tranform character when hit.
	private Vector3 endScaleForWard = new Vector3(0.01f, 1, 1), endScaleUpSide = new Vector3(1, 0.05f, 1), LeftScaleSide = new Vector3(1, 1, 0.01f);
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
		audioSource = GetComponent<AudioSource>();
		if (gameManager.intance.getCharacter() != null)
		{
			GameObject currentCharater = gameManager.intance.getCharacter();
			gameObject.GetComponentInChildren<MeshFilter>().mesh = currentCharater.GetComponentInChildren<MeshFilter>().mesh;
			gameObject.GetComponentInChildren<Renderer>().material = currentCharater.GetComponentInChildren<Renderer>().material;
		}
		if (gameManager.intance.GetMaterial() != null)
			snowFalling.SetActive(true);
		else snowFalling.SetActive(false);
	}
	void Update()
	{
		if (!jump)
			HandleInput();
	}

	private void HandleInput()
	{
		int horizontal = 0;
		int vertical = 0;
#if UNITY_STANDALONE

		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			vertical = 1;
		}
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			vertical = -1;
			
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			horizontal = 1;
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			horizontal = -1;
		
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			setUpForShrug();
			preesKey = !preesKey;
		}
#endif
#if UNITY_ANDROID
		
		if (Input.touchCount > 0)
			{

			Touch myTouch = Input.touches[0];
			if (myTouch.phase == TouchPhase.Began)
				{
				touchOrigin = myTouch.position;
				setUpForShrug();
				preesKey = !preesKey;
			}
			//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					float x = touchEnd.x - touchOrigin.x;
					float y = touchEnd.y - touchOrigin.y;

					touchOrigin.x = -1;
				if (Mathf.Abs(x) > Mathf.Abs(y))
				{
					horizontal = x > 0 ? 1 : -1;
				}
				else 
				{
					vertical = y >= 0 ? 1 : -1;
				}
			
			}
		

		}

#endif

		if (horizontal == 1)
		{
			setUpForJump(new Vector3(0, 0, 0));
			endMarker = startMarker - disJumpHorizontal;
			jump = checkCounldForJump(startMarker, endMarker);
		}
		else if (horizontal == -1)
		{
			setUpForJump(new Vector3(0, 180, 0));
			endMarker = startMarker + disJumpHorizontal;

			jump = checkCounldForJump(startMarker, endMarker);
		}
		else if (vertical == 1)
		{
			setUpForJump(new Vector3(0, -90, 0));
			endMarker = startMarker + disJumpVertical;

			jump = checkCounldForJump(startMarker, endMarker);
			if (jump && endMarker.x > maxPosition.x)
			{
				gameManager.intance.setScore();
				maxPosition = endMarker;
			}
		}
		else if(vertical == -1)
		{
			setUpForJump(new Vector3(0, 90, 0));
			endMarker = startMarker - disJumpVertical; ;

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
		bool check = (!Physics.Linecast(startMark + new Vector3(0, 2, 0), endMark + new Vector3(0, 2, 0), Tree)) ? true : false;
		if (check)
		{
			anim.Play("Jump");
			endMarker.x = Mathf.Round(endMarker.x / 9) * 9;
			endMarker.z = Mathf.Round(endMarker.z / 8) * 8;
			jump = false;

			audioSource.PlayOneShot(jumpAudio, 0.3f);
		}
		return check;


	}
	public void setActiveYardPath()
	{
		if (YardPath != null)
			YardPath.SetActive(true);
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
			audioSource.PlayOneShot(carHitAudio, 1f);
			TakeDamage();
		}
		if (other.gameObject.tag == "Coin")
		{
			other.gameObject.GetComponent<Coin>().prepareDisapear();
			audioSource.PlayOneShot(getCoin, 1f);
			gameManager.intance.setCoin();
		}
	}
	public void TakeDamage()
	{
		StartCoroutine(TakeSnapshot(Screen.width, Screen.height, 400, 400));
		hitTopSide = Physics.Linecast(transform.position, transform.position + disJumpVertical * 1 / 2, Cars);
		hitLeftSide = Physics.Linecast(transform.position, transform.position - disJumpHorizontal * 1 / 2, Cars);
		hitRightSide = Physics.Linecast(transform.position, transform.position + disJumpHorizontal * 1 / 2, Cars);

		Physics.IgnoreLayerCollision(9, 10);
		Physics.IgnoreLayerCollision(8, 10);
		rigidbody.velocity = Vector3.zero;


		IsDead = true;
		timeScale = Time.time;

		
		//StartCoroutine(TakeSnapshot(240, 290, 400, 400));
		//	Sprite mySprite = Sprite.Create(snapshot, new Rect(0.0f, 0.0f, 240, 290), new Vector2(5, 5), 100f);


		//	snapshot = mySprite.texture;
		Debug.Log(transform.position);
	}
	public IEnumerator TakeSnapshot(int width, int height, int posX, int posY)
	{
	
		yield return new WaitForEndOfFrame();
		Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, true);

		texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);

		texture.Apply();
		snapshot = null;
	//	Sprite mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, 240, 20), new Vector2(50, 15), 100f);
		snapshot = texture;

	}
	public Texture2D getTexture()
	{
		return snapshot;
	}

	void FixedUpdate()
	{
		//	Debug.DrawLine(transform.position, transform.position + disJumpVertical * 1 / 2, Color.red);
		//	Debug.DrawLine(transform.position, transform.position - disJumpHorizontal * 1 / 2, Color.red);
		//	Debug.DrawLine(transform.position, transform.position + disJumpHorizontal * 1 / 2, Color.red);
		//	TakeDamage();
		if (!isPause)
		{
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
				if (endScale == endScaleForWard || endScale == endScaleUpSide)
				{
					gameManager.intance.EndGame();
				//	isPause = true;
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

}



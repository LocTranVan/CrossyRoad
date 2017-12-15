using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class test : MonoBehaviour, IPointerDownHandler, IDragHandler
{
	public GameObject canvas;

	private Vector2 pointerOffset;
	private RectTransform canvasRectTransform;
	private RectTransform panelRectTransform;

	private Rigidbody rigidbody;
	// Use this for initialization
	private void Awake()
	{
		Canvas canv = canvas.GetComponent<Canvas>();
		if(canv != null)
		{
			canvasRectTransform = canvas.transform as RectTransform;
			panelRectTransform = transform as RectTransform;
		}

		rigidbody = GetComponent<Rigidbody>();
	}
	void Start () {
		
	}
	public  void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("On Begin Drag" + eventData.position);
		
	}
	public  void OnDrag(PointerEventData eventData)
	{
		Debug.Log("On Drag" + Time.time);
		rigidbody.velocity = new Vector3(1, 1, 1);
	}
	Vector2 ClampToWindow(PointerEventData data)
	{
		Vector2 rawPointerPosition = data.position;

		Vector3[] canvasCorners = new Vector3[4];
		canvasRectTransform.GetWorldCorners(canvasCorners);

		float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
		float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

		Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
		return newPointerPosition;
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerDown(PointerEventData data)
	{
		panelRectTransform.SetAsLastSibling();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);

	}
}

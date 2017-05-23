using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMOCameraController : MonoBehaviour
{
	[Header("References")]
	public Transform pointOfInterest;
	public Transform targetTransform;

	[Header("Camera Settings")]
	public float startingZoom = -3f;
	public float zoomSpeed = 2f;
	public float minZoom = -2f;
	public float maxZoom = -10f;
	public float minPitch = -60f;
	public float maxPitch = 60f;
	public Vector3 pointOfInterestOffset = new Vector3(0, 1f, 0);
	public Vector2 cameraSensitivity = new Vector2(10f, 10f);
	
	private Transform camTransform;
	private float xInput;
	private float yInput;
	private float scrollInput;
	private float currentZoom;

	void Start()
	{
		camTransform = GetComponent<Transform>();
		currentZoom = startingZoom;
	}

	void LateUpdate()
	{
		//Get Inputs
		scrollInput = Input.GetAxis("Mouse ScrollWheel");
		if (Input.GetMouseButton(1))
		{
			xInput += Input.GetAxis("Mouse X");
            yInput -= Input.GetAxis("Mouse Y"); //change sign to invert
		}

		//Clamp inputs
		currentZoom += scrollInput * zoomSpeed;
		currentZoom = Mathf.Clamp(currentZoom, maxZoom, minZoom);

		yInput = Mathf.Clamp(yInput, minPitch, maxPitch);	

		//Apply inputs
		camTransform.localPosition = new Vector3(0, 0, currentZoom);
		camTransform.LookAt(pointOfInterest);
		pointOfInterest.localRotation = Quaternion.Euler(yInput * cameraSensitivity.y, xInput * cameraSensitivity.x, 0);
		pointOfInterest.localPosition = targetTransform.position + pointOfInterestOffset;
	}



}

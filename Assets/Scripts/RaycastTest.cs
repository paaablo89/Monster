using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour 
{
	public float rayCastDistance = 3f;

	private Vector3 target;
	private int rayCastLayerMask;
	private Ray ray;
	private RaycastHit rHit;

	void Start()
	{
		rayCastLayerMask = LayerMask.NameToLayer("Objective");
		rayCastLayerMask = ~rayCastLayerMask;
	}

	void Update()
	{
		//SI HAGO CLICK, USAR RAYCAST DESDE PLAYER
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			ray.origin = transform.position;
			ray.direction = transform.forward;
			Physics.Raycast(ray, out rHit, rayCastDistance, rayCastLayerMask);

			if (rHit.transform != null)
			{
				Debug.Log("HITTING " + rHit.transform.gameObject.name);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMOCharacterController : MonoBehaviour 
{
    public float forwardSpeed = 2f;
    public float sideSpeed = 1.5f;
    public float rotationSpeed = 5f;

    private float sideMovementInput;
    private float forwardMovementInput;
    private Quaternion extraRotation;
    private Transform cameraPointOfInterest;
	private CharacterController charController;
	private Transform characterTransform;

    void Start()
	{
		cameraPointOfInterest = Camera.main.GetComponent<MMOCameraController>().pointOfInterest;
		charController = GetComponent<CharacterController>();
		characterTransform = transform;
	}

    void Update()
    {
        //MOVEMENT
        forwardMovementInput = Input.GetAxis("Vertical") * forwardSpeed;
        sideMovementInput = Input.GetAxis("Horizontal") * sideSpeed;

        Vector3 totalMovement = new Vector3(sideMovementInput, 0, forwardMovementInput);

        totalMovement = characterTransform.rotation * totalMovement;

        charController.Move(totalMovement * Time.deltaTime);

        //ROTATE WHEN MOVING FORWARD
		if (forwardMovementInput != 0)
        {
            characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, Quaternion.Euler(0, cameraPointOfInterest.eulerAngles.y, 0), Time.deltaTime * rotationSpeed);
        }
    }
}

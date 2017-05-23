using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	public bool lockCursor = true;
	public float mouseSensitivity = 10f;
	public Transform target;
	public float distanceFromTarget = 3f;
	public float minRot = -30f;
	public float maxRot = 55f;

	public float rotSmoothTime = 0.1f;

	private Vector3 rotSmoothVelocity;
	private Vector3 currentRotation;

	private float yaw; //rotacion en eje y
	private float pitch; //rotacion en eje x

	void Start()
	{
		//Este bloque de código permite bloquear el cursor a la pantalla de Game View y hacerlo invisible
		if (lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void LateUpdate()
	{
		//YAW: rot. en eje X | PITCH: rot. en eje Y | ROLL: rot. en eje Z
		//Tomamos input de rot en X (si desplazamos el mouse de arriba a abajo, rotamos en X)
		pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		//Limitamos la rot
		pitch = Mathf.Clamp(pitch, minRot, maxRot);
		//Tomamos input de rot en Y (si desplazamos el mouse de izq a der, rotamos en Y)
		yaw += Input .GetAxis("Mouse X") * mouseSensitivity;

		//Creamos nuestro vector de rotación (la vamos a aplicar como ángulos de Euler así podemos usar SmoothDamp)
		Vector3 targetRot = new Vector3(pitch, yaw, 0);
		//Aplicamos suavizado con SmoothDamp
		currentRotation = Vector3.SmoothDamp(currentRotation, targetRot, ref rotSmoothVelocity, rotSmoothTime);
		//Aplicamos la rotación, también podríamos hacer:
		//transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
		transform.eulerAngles = currentRotation;
		//Aplicamos la posición de la cámara, manteniendo la distancia que deseamos del Target.
		transform.position = target.position - transform.forward * distanceFromTarget;


	}
}

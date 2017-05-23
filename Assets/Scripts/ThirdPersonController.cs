using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour 
{
	//Variables de movimiento en XZ
	public float walkSpeed = 3f;
	public float runSpeed = 6f;

	//Variables de movimiento en Y
	public float jumpHeight = 1f;
	public float gravity = -11f;

	//Variables de tiempo de suavizado para SmoothDamp
	public float speedSmoothTime = 0.2f;
	public float rotSmoothTime = 0.1f;

	//Variables de parametros de referencia para SmoothDamp
	private float speedSmoothVelocity;
	private float rotSmoothVelocity;

	//Variable auxiliar para guardar velocidad actual
	private float currentHorizontalSpeed;

	//Variables auxiliares para tomar input
	private Vector2 directionInput;
	private float xInput;
	private float yInput;

	private bool isRunning = false;

	private Vector3 currentVelocity;
	private float yVelocity;
	private float xzVelocity;

	private Transform cameraTransform;
	private CharacterController characterController;

	void Start()
	{
		cameraTransform = Camera.main.transform;
		characterController = GetComponent<CharacterController>();
	}

	void Update()
	{
		//Tomamos Input
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");
		//Creamos un vector normalizado para saber la dirección de nuestro input
		directionInput = new Vector2(xInput, yInput).normalized;

		//A nuestra velocidad vertical le agregamos la aceleración gravitatoria.
		yVelocity += gravity * Time.deltaTime;
		//actualizamos velocidad con componente vertical

		//Siempre que haya input, vamos a rotar y aplicar movimiento
		if (directionInput != Vector2.zero)
		{
			Rotate();
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		Movement();

	}

	private void Jump()
	{
		//Si el CharacterController está en el piso (https://docs.unity3d.com/ScriptReference/CharacterController-isGrounded.html)
		if (characterController.isGrounded)
		{
			//Calculo velocidad vertical de salto
			yVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight); 
		}
	}

	private void Movement()
	{
		//Tomo input de velocidad, ¿estoy corriendo?
		isRunning = Input.GetKey(KeyCode.LeftShift);

		//Determinamos nuestra velocidad objetivo según corremos o no.
		//La notación que usamos se llama operador ternario o ternary operator, es un "mini if" (https://en.wikipedia.org/wiki/%3F:)
		//Despues lo multiplicamos por directionInput.magnitude, porque si no hay input, ese valor será 0 y no habrá movimiento
		float targetSpeed = ((isRunning == true) ? runSpeed : walkSpeed) * directionInput.magnitude;
		//Obtenemos la cantidad de velocidad suavizada (https://docs.unity3d.com/ScriptReference/Mathf.SmoothDamp.html)
		xzVelocity = Mathf.SmoothDamp(xzVelocity, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

		//Podríamos aplicar la traslación al Transform si no hubieramos sumado la velocidad vertical:
		//transform.Translate(velocity * Time.deltaTime, Space.World);
		//Pero aprovechamos el Character Controller que nos ofrece Unity que ya detecta colisiones, rampas y escalones
		//UNITY MANUAL: https://docs.unity3d.com/Manual/class-CharacterController.html
		//UNITY SCRIPTING API: https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
		currentVelocity = transform.forward * xzVelocity + Vector3.up * yVelocity;
		characterController.Move(currentVelocity * Time.deltaTime);

		//Obtenemos el valor real de la velocidad de nuestro Character Controller, magnitud del vector formado por la velocidad en X y Z.
		//Esto sirve para saber si el CC está contra un pared, su velocidad en realidad es 0 a pesar de que le pasemos otra por input.
		xzVelocity = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

		//Si nuestro CC está en el piso, información que el CC de Unity nos otorga, seteamos la velocidad vertical en 0.
		//Si no hicieramos ésto, nuestro personaje estaría acumulando potencial gravitatorio y cada caída consecutiva sería más y más fuerte.
		if (characterController.isGrounded)
			yVelocity = -0.00001f;
	}

	private void Rotate()
	{
		//Calculamos rotacion según el dirInput y le sumamos rotación según la cámara
		float targetRot = Mathf.Atan2(directionInput.x, directionInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
		//Obtenemos la cantidad de rotación suavizada (https://docs.unity3d.com/ScriptReference/Mathf.SmoothDampAngle.html)
		float smoothedRot = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref rotSmoothVelocity, rotSmoothTime);
		//Aplicamos la rotación suavizada a nuestra rotación local. Sería lo mismo escribir:
			// transform.localEulerAngles = new Vector3(0, smoothedRot, 0);
			// o también:
			//transform.localRotation = Quaternion.Euler(0, smoothedRot, 0);
		transform.localEulerAngles = Vector3.up * smoothedRot;
	}
}

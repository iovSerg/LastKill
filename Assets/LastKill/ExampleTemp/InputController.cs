using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class InputController : MonoBehaviour
{
   PlayerInputSystem inputSystem;

	public Vector2 Move;
	public Vector2 Look;

	private void Awake()
	{
		inputSystem = new PlayerInputSystem();


		inputSystem.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
		inputSystem.Player.Move.canceled += ctx => Move = ctx.ReadValue<Vector2>();

		inputSystem.Player.Look.performed += ctx => Look = ctx.ReadValue<Vector2>();
		inputSystem.Player.Look.canceled += ctx => Look = ctx.ReadValue<Vector2>();
		
	}



	private void OnEnable()
	{
		inputSystem.Enable();
	}
	private void OnDisable()
	{
		inputSystem.Disable();
	}
}

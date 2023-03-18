using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviour
{
    LastKill inputs;
	Rigidbody rb;
	public Vector2 move;
	private void Awake()
	{
		inputs = new LastKill();
		rb = GetComponent<Rigidbody>();
		inputs.Enable();

		inputs.Player.Move.performed += context => move = context.ReadValue<Vector2>();
		inputs.Player.Move.canceled += context => move = context.ReadValue<Vector2>();
	}

	private void FixedUpdate()
	{
		if(move != Vector2.zero)
		{
			rb.AddForce(new Vector3(move.y, 0f, move.x) * 100f);
		}
	}
}

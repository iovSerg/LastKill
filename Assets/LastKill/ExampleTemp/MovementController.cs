using LastKill;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour
{

    CharacterController _character;
	InputController _input;
    Animator _animator;
	CameraController _camera;
	Camera mainCamera;

	[SerializeField] private float smooth = 5f;
	[SerializeField] private float moveSpeed = 3f;
	[SerializeField] private float gravity = 9.5f;
	[SerializeField] private float turnSpeed = 15f;
	[SerializeField] private float luftAngle = 90f;

	[SerializeField] private Vector3 moveDirection = Vector3.zero;
	[SerializeField] private Vector3 moveVelocity = Vector3.zero;

	private void Start()
	{
		_animator = GetComponent<Animator>();	
		_character = GetComponent<CharacterController>();
		_input = GetComponent<InputController>();
	    _camera = GetComponent<CameraController>();
		mainCamera = Camera.main;
	}
	private void Update()
	{
		if (_input.Move != Vector2.zero)
		{
			_animator.SetFloat("Horizontal", _input.Move.x, 0.2f, Time.deltaTime);
			_animator.SetFloat("Vertical", _input.Move.y, 0.2f, Time.deltaTime);
		}
		else
		{
			_animator.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
			_animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
		}
	}
	public float cameraAngle;
	public float transformAngle;
	public float angleBeetwen = 0f;
	public float clampAngle;

	public float leftAngle;
	public float rightAngle;
	private void FixedUpdate()
	{

		if (_character.isGrounded)
		{
			moveVelocity.y = 0f;
		}
		else
		{
			moveVelocity.y -= gravity * Time.deltaTime;
		}

		transformAngle = transform.rotation.eulerAngles.y;
		cameraAngle = mainCamera.transform.eulerAngles.y;

		if(_input.Move != Vector2.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, cameraAngle, 0), turnSpeed * Time.fixedDeltaTime);
			moveDirection = _camera.GetCameraDirection(_input.Move);
			_character.Move(moveDirection * (moveSpeed * Time.deltaTime) + new Vector3(0.0f, moveVelocity.y, 0.0f));
		}
		

	}
	private float ClampAngle(float angle)
	{
		if (angle < 0f)
			angle += 360f;
		if (angle > 360f)
			angle -= 360f;
		return angle;
	}

	private void OnAnimatorIK(int layerIndex)
	{


		
	}
}

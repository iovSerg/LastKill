using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LastKill
{
	public class AiController : MonoBehaviour
	{
		private NavMeshAgent _agent;
		private Animator _animator;
		private float timer;

		public float maxTime = 1.0f;
		public float maxDistance = 1.0f;
		public Transform playerTransform;
		private void Start()
		{
			_agent = GetComponent<NavMeshAgent>();
			_animator = GetComponent<Animator>();

			if (playerTransform == null)
				playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		}
		private void Update()
		{
			timer -= Time.deltaTime;{
			if (timer < 0)
			{
				float distance = (playerTransform.position - _agent.destination).sqrMagnitude;
					if(distance < maxDistance * maxDistance)
					{

					}
					timer = maxTime;
			}
			_agent.destination = playerTransform.position;
			_animator.SetFloat("Speed", _agent.velocity.magnitude);
		}
		
		}
	}
}

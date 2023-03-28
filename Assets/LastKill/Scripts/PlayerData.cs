using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastKill
{
	[CreateAssetMenu(fileName = "Player data", menuName = "LastKill/Player data", order = 1)]
	public class PlayerData : ScriptableObject
	{
		public float walkSpeed;
		public float sprintSpeed;
		public float runSpeed;
		public float crouchSpeed;
		public float crawlSpeed;
		public float rollSpeed;
	}
}

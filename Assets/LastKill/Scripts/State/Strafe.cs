using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class Strafe : AbstractAbilityState
	{
		public override void OnStartState()
		{
			
		}

		public override bool ReadyToStart()
		{
			return _input.Aim;
		}

		public override void UpdateState()
		{
			if (!_input.Aim) StopState();
		}
		public override void OnStopState()
		{
			base.OnStopState();
		}
	}
}

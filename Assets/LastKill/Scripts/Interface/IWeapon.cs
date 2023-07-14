using System;
using UnityEngine;
using UnityEngine.Events;

namespace LastKill
{
	public interface IWeapon 
	{
		public bool IsReload { get; }
		public bool IsWeapon { get; }

	}

}
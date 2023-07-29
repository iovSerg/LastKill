using System;
using UnityEngine;

namespace LastKill
{
	public interface IInput
	{
		public Action EventDied { get; set; }
		public Action<bool> EventFire { get; set; }
		public Action<bool> EventAim { get; set; }
		public Action<int> EventReload { get; set; }
		public Action<int> EventSelectWeapon { get; set; }

		public Vector2 Move { get; }
		public Vector2 Look { get; }
		public float Scroll { get; }
		public bool Sprint { get; }
		public bool Jump { get; }
		public bool Crouch { get; }
		public bool Roll { get; }
		public bool Fire { get; }
		public bool Crawl { get; }
		public bool Aim { get; }
		public bool Reload { get; }
		public float Magnituda { get; }
		public int CurrentWeapon { get; }
	}

}
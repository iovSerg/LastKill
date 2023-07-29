
using UnityEngine;

namespace LastKill
{
	public interface IAnimator
	{
		public Animator Animator { get; }
		public bool NoAim { get; set; }
		public bool Aiming { get; set; }
		public bool Reload { get; set; }
		public int WeaponID { get; set; }
		public float WeaponPose { get; set; }
		public bool isDrawWeapon { get; }

		public void XYMove();
		public void ResetMovementParametrs();
		public void DisableAll();

		public void SetAnimationState(int hashName, int layerIndex = 0, float transitionDuration = 0.1f);
		public void SetAnimationState(string stateName, int layerIndex = 0, float transitionDuration = 0.1f);

		public bool HasFinishedAnimation(string stateName, int layerIndex);
		public bool HasFinishedAnimation(int layerIndex, float time = 0.95f);
		public void StrafeUpdate();
		public void LocomotionUpdate();
	}
}

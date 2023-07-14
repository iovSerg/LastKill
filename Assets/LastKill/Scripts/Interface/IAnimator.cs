
using UnityEngine;

namespace LastKill
{
    public interface IAnimator 
    {
        public Animator Animator { get; }
        public bool noAiming { get; set; }
        public bool Aiming { get; set; }
        public bool isDrawWeapon { get; }

        public void ResetMovementParametrs();

        public void SetAnimationState(int hashName, int layerIndex = 0, float transitionDuration = 0.1f);
        public void SetAnimationState(string stateName, int layerIndex = 0, float transitionDuration = 0.1f);

        public bool HasFinishedAnimation(string stateName, int layerIndex);
        public bool HasFinishedAnimation(int layerIndex,float time = 0.95f);
        public void StrafeUpdate();
        public void LocomotionUpdate();
    }
}

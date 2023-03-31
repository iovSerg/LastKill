using UnityEngine;

namespace LastKill
{
    public interface IDetection
    {
        public bool CanGetUp(float offset);
        public DescriptionClimb ClimbTargets();
        public RaycastHit ClimbTarget();
        public float ForwardHeight { get; }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public interface ICamera
    {
        public  Vector3 GetCameraDirection(Vector2 moveInput);
        public float Sensivity { get; set; }
    }
}

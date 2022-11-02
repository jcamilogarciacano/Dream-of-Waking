using System;
using UnityEngine;

namespace Railcam2D
{
    ///<summary>A target object for a camera to follow.</summary>
    [Serializable]
    public class CameraTarget
    {
        ///<summary>The amount camera position is effected by target position along the x-axis.</summary>
        [RangeAttribute(0f, 1f)]
        public float InfluenceX = 1;

        ///<summary>The amount camera position is effected by target position along the y-axis.</summary>
        [RangeAttribute(0f, 1f)]
        public float InfluenceY = 1;

        ///<summary>UnityEngine.Transform component of the target.</summary>
        public Transform Transform = null;

        ///<summary>Position of the target Transform.</summary>
        public Vector3 Position
        {
            get
            {
                return Transform == null ? Vector3.zero : Transform.position;
            }
        }
    }
}

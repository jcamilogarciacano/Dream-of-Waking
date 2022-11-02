using UnityEngine;

namespace Railcam2D
{
    ///<summary>Displaces a camera along a Rail.</summary>
    [System.Serializable]
    public class Effect
    {
        ///<summary>Determines whether or not the effect is turned on.</summary>
        public bool Active = true;

        ///<summary>Interpolation of camera position relative to target position between two Wapoints.</summary>
        [RangeAttribute(0f, 1f)]
        public float CameraInterpolation = 0.5f;

        ///<summary>Interpolation of target position relative to camera position between two Wapoints.</summary>
        [RangeAttribute(0f, 1f)]
        public float TargetInterpolation = 0.25f;

        ///<summary>Index of the effected Waypoint.</summary>
        public int WaypointIndex = -1;
    }
}

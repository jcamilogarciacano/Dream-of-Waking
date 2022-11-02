using UnityEngine;

namespace Railcam2D
{
    ///<summary>A waypoint along a rail's path.</summary>
    [System.Serializable]
    public class Waypoint
    {
        ///<summary>The position of the waypoint's quadratic bezier control point.</summary>
        public Vector2 CurveControlPoint;

        ///<summary>Determines whether the waypoint defines a straight line or quadratic bezier curve.</summary>
        public CurveType CurveType;

        ///<summary>Determines the camera's alignment to its target position.</summary>
        public FollowAxis FollowAxis;

        ///<summary>The waypoint's 2D position in the scene.</summary>
        public Vector2 Position;
    }
}

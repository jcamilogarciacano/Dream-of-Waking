using System.Collections.Generic;
using UnityEngine;

namespace Railcam2D
{
    ///<summary>A 2D camera path.</summary>
    public class Rail : MonoBehaviour
    {
        ///<summary>Determines whether or not the rail is used to calculate camera position.</summary>
        public bool Active = true;

        ///<summary>A list of effects that displace a camera along the rail.</summary>
        public List<Effect> Effects = new List<Effect>();

        ///<summary>A list of waypoints that define the rail's path.</summary>
        public List<Waypoint> Waypoints = new List<Waypoint>();

        ///<summary>Sets Active to true.</summary>
        public void Activate()
        {
            Active = true;
        }

        ///<summary>Sets Active to false.</summary>
        public void Deactivate()
        {
            Active = false;
        }
    }
}

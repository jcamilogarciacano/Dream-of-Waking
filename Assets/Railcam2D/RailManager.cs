using UnityEngine;

namespace Railcam2D
{
    ///<summary>Editor component that allows all rails to be edited using a single interface.</summary>
    [DisallowMultipleComponent]
    public class RailManager : MonoBehaviour
    {
        ///<summary>Determines whether or not the Scene View displays only the current rail, or all rails.</summary>
        public bool ViewSingle = false;
    }
}

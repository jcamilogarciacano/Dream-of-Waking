using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Railcam2D
{
    ///<summary>A component that moves the camera using rails.</summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class Railcam2D : MonoBehaviour
    {
        ///<summary>Determines whether or not Railcam 2D controls the camera.</summary>
        public bool Active = true;

        ///<summary>Offsets the camera position from its target position.</summary>
        public Vector2 Offset = Vector2.zero;

        ///<summary>Determines whether or not Railcam 2D uses rails to calculate camera position.</summary>
        public bool RailsConnected = true;

        ///<summary>Determines the degree of smoothing applied to camera movement.</summary>
        public Vector2 Smooth = new Vector2(0.25f, 0.25f);

        ///<summary>A list of CameraTargets used to calculate the camera's target position.</summary>
        public List<CameraTarget> Targets = new List<CameraTarget>();

        ///<summary>Determines the Unity update method in which to move the camera.</summary>
        public UpdateMethod UpdateMethod;

        ///<summary>The combined position of all camera targets.</summary>
        public Vector2 TargetPosition { get; private set; } = Vector2.zero;

        ///<summary>The position the camera moves toward. Adjusted for offset and dependent on rail calculations.</summary>
        public Vector2 AdjustedTargetPosition { get; private set; } = Vector2.zero;

        private Rail[] _rails;

        private void Awake()
        {
            FindRails();

            if (Active)
            {
                UpdateTargetPosition();
                transform.position = new Vector3(AdjustedTargetPosition.x, AdjustedTargetPosition.y, transform.position.z);
            }
        }

        private void FixedUpdate()
        {
            if (UpdateMethod == UpdateMethod.FixedUpdate && Active)
            {
                Move(Time.fixedDeltaTime);
            }
        }

        private void LateUpdate()
        {
            if (UpdateMethod == UpdateMethod.LateUpdate && Active)
            {
                Move(Time.deltaTime);
            }
        }

        ///<summary>Sets Active to true.</summary>
        public void Activate()
        {
            Active = true;
        }

        ///<summary>Adds a new target to the list of CameraTargets.</summary>
        public void AddTarget(Transform transform)
        {
            AddTarget(transform, 1, 1);
        }

        ///<summary>Adds a new target to the list of CameraTargets.</summary>
        public void AddTarget(Transform transform, float influenceX, float influenceY)
        {
            if (transform == null)
            {
                Debug.LogError("The Transform was null. A Railcam2D.CameraTarget requires a Transform component");
                return;
            }

            Targets.Add(new CameraTarget()
            {
                Transform = transform,
                InfluenceX = Mathf.Clamp(influenceX, 0, 1),
                InfluenceY = Mathf.Clamp(influenceY, 0, 1)
            });
        }

        ///<summary>Removes all targets from the list of CameraTargets.</summary>
        public void ClearTargets()
        {
            Targets.Clear();
        }

        ///<summary>Sets RailsConnected to true.</summary>
        public void ConnectRails()
        {
            RailsConnected = true;
        }

        ///<summary>Sets Active to false.</summary>
        public void Deactivate()
        {
            Active = false;
        }

        ///<summary>Sets RailsConnected to false.</summary>
        public void DisconnectRails()
        {
            RailsConnected = false;
        }

        ///<summary>Updates Railcam 2D's rail cache. This should be called after adding a rail to the scene during runtime.</summary>
        public void FindRails()
        {
            _rails = Object.FindObjectsOfType<Rail>();
        }

        ///<summary>Removes the specified target from the list of CameraTargets.</summary>
        public void RemoveTarget(Transform transform)
        {
            if (transform == null)
            {
                return;
            }

            var target = Targets.FirstOrDefault(t => t.Transform == transform);

            if (target == null)
            {
                return;
            }

            Targets.Remove(target);
        }

        ///<summary>Calculates a new target position and moves the Railcam 2D game object toward it.</summary>
        public void Move(float deltaTime)
        {
            if (!Active || Targets == null || Targets.Count < 1)
            {
                return;
            }

            UpdateTargetPosition();

            var newPosition = AdjustedTargetPosition;

            if (Smooth.x > 0 || Smooth.y > 0)
            {
                var displacementFromIntendedPos = newPosition - (Vector2)transform.position;

                if (Smooth.x != 0)
                {
                    newPosition.x -= ExponentialDecay(displacementFromIntendedPos.x, deltaTime, 1 / Mathf.Abs(Smooth.x));
                }

                if (Smooth.y != 0)
                {
                    newPosition.y -= ExponentialDecay(displacementFromIntendedPos.y, deltaTime, 1 / Mathf.Abs(Smooth.y));
                }
            }

            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }

        ///<summary>Calculates a new target position and adjusted target position.</summary>
        public void UpdateTargetPosition()
        {
            CalculateTargetPosition();
            CalculateAdjustedTargetPosition();
        }

        private void CalculateAdjustedTargetPosition()
        {
            var tempAdjustedTargetPosition = RailsConnected
                ? RailUtilities.GetCameraPosition(TargetPosition, _rails)
                : TargetPosition;

            if (Offset.x != 0)
            {
                tempAdjustedTargetPosition.x += Offset.x;
            }

            if (Offset.y != 0)
            {
                tempAdjustedTargetPosition.y += Offset.y;
            }

            AdjustedTargetPosition = tempAdjustedTargetPosition;
        }

        private void CalculateTargetPosition()
        {
            if (Targets == null || Targets.Count < 1)
            {
                TargetPosition = transform.position;
                return;
            }

            if (Targets.Count == 1)
            {
                if (Targets[0] != null && Targets[0].Transform != null)
                {
                    TargetPosition = new Vector2(
                        Targets[0].InfluenceX == 0 ? transform.position.x : Targets[0].Position.x,
                        Targets[0].InfluenceY == 0 ? transform.position.y : Targets[0].Position.y
                    );
                    return;
                }

                TargetPosition = transform.position;
                return;
            }

            var maxInfluenceX = 0f;
            var maxInfluenceY = 0f;

            for (var i = 0; i < Targets.Count; ++i)
            {
                var target = Targets[i];

                if (target == null || target.Transform == null)
                {
                    continue;
                }

                if (target.InfluenceX > maxInfluenceX)
                {
                    maxInfluenceX = target.InfluenceX;
                }

                if (target.InfluenceY > maxInfluenceY)
                {
                    maxInfluenceY = target.InfluenceY;
                }
            }

            if (maxInfluenceX == 0 && maxInfluenceY == 0)
            {
                TargetPosition = transform.position;
                return;
            }

            var targetX = 0f;
            var targetY = 0f;
            var totalInfluenceX = 0f;
            var totalInfluenceY = 0f;

            for (var i = 0; i < Targets.Count; ++i)
            {
                var target = Targets[i];

                if (target == null || target.Transform == null)
                {
                    continue;
                }

                if (target.InfluenceX > 0)
                {
                    var normalizedInfluenceX = target.InfluenceX * (1 / maxInfluenceX);
                    targetX += target.Position.x * normalizedInfluenceX;
                    totalInfluenceX += normalizedInfluenceX;
                }

                if (target.InfluenceY > 0)
                {
                    var normalizedInfluenceY = target.InfluenceY * (1 / maxInfluenceY);
                    targetY += target.Position.y * normalizedInfluenceY;
                    totalInfluenceY += normalizedInfluenceY;
                }
            }

            targetX = totalInfluenceX > 0 ? targetX / totalInfluenceX : transform.position.x;
            targetY = totalInfluenceY > 0 ? targetY / totalInfluenceY : transform.position.y;

            TargetPosition = new Vector2(targetX, targetY);
        }

        private static float ExponentialDecay(float value, float time, float rate)
        {
            return value * Mathf.Exp(-time * rate);
        }
    }
}

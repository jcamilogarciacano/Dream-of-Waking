using System.Collections.Generic;
using UnityEngine;

namespace Railcam2D
{
    ///<summary>Contains the core logic of Railcam 2D's rail system.</summary>
    public static class RailUtilities
    {
        private class WaypointDetails
        {
            public Waypoint Waypoint { get; set; }
            public Waypoint NextWaypoint { get; set; }
            public float Interpolation { get; set; }
            public float SqDistance { get; set; }
        }

        ///<summary>Calculates camera position using a specific Rail.</summary>
        public static Vector2 GetCameraPosition(Vector2 targetPosition, Rail rail)
        {
            if (rail == null)
            {
                Debug.LogError("The Railcam2D.Rail object was null.");
                return targetPosition;
            }

            var waypoints = rail.Waypoints;

            if (waypoints == null || waypoints.Count < 2)
            {
                var railName = rail.gameObject == null ? "" : "'" + rail.gameObject.name + "' ";
                Debug.LogError("The Railcam2D.Rail object " + railName + "has less than 2 Waypoints. A Rail must have at least 2 Waypoints.");
                return targetPosition;
            }

            var closestWaypointDetails = GetClosestWaypointDetails(waypoints, rail.Effects, targetPosition);
            return GetCameraPositionForClosestWaypointDetails(closestWaypointDetails);
        }

        ///<summary>The primary method used to calculate camera position. Used by the Railcam 2D component.</summary>
        public static Vector2 GetCameraPosition(Vector2 targetPosition, Rail[] rails)
        {
            if (rails == null)
            {
                Debug.LogError("The Railcam2D.Rail array was null.");
                return targetPosition;
            }

            if (rails.Length < 1)
            {
                return targetPosition;
            }

            var waypointDetailsList = new List<WaypointDetails>();

            for (var i = 0; i < rails.Length; ++i)
            {
                var rail = rails[i];
                if (rail.Active && rail.Waypoints != null && rail.Waypoints.Count > 1)
                {
                    waypointDetailsList.Add(GetClosestWaypointDetails(rail.Waypoints, rail.Effects, targetPosition));
                }
            }

            if (waypointDetailsList.Count < 1)
            {
                return targetPosition;
            }

            var closestWaypointDetails = GetClosestWaypointDetailsToPoint(targetPosition, waypointDetailsList);

            return GetCameraPositionForClosestWaypointDetails(closestWaypointDetails);
        }

        private static WaypointDetails GetClosestWaypointDetails(List<Waypoint> waypoints, List<Effect> effects, Vector2 targetPosition)
        {
            var waypointDetailsList = CreateWaypointDetailsList(waypoints, targetPosition);
            var closestWaypointDetails = GetClosestWaypointDetailsToPoint(targetPosition, waypointDetailsList);

            closestWaypointDetails =
                GetAdjustedClosestWaypointDetailsForCompatibleFollowAxis(
                    targetPosition,
                    waypointDetailsList,
                    waypoints,
                    waypointDetailsList.IndexOf(closestWaypointDetails));

            if (effects != null && effects.Count > 0)
            {
                closestWaypointDetails.Interpolation =
                    GetEffectedInterpolation(
                        effects,
                        waypointDetailsList.IndexOf(closestWaypointDetails),
                        closestWaypointDetails.Interpolation);
            }

            return closestWaypointDetails;
        }

        private static Vector2 GetCameraPositionForClosestWaypointDetails(WaypointDetails waypointDetails)
        {
            return waypointDetails.Waypoint.CurveType == CurveType.Linear
                ? GetPointOnLinearCurve(
                    waypointDetails.Waypoint.Position,
                    waypointDetails.NextWaypoint.Position,
                    waypointDetails.Interpolation)
                : GetPointOnQuadraticCurve(
                    waypointDetails.Waypoint.Position,
                    waypointDetails.Waypoint.CurveControlPoint,
                    waypointDetails.NextWaypoint.Position,
                    waypointDetails.Interpolation);
        }

        private static Vector2 GetPointOnLinearCurve(Vector2 a0, Vector2 a1, float t)
        {
            return a0 * (1 - t)
                + a1 * t;
        }

        private static Vector2 GetPointOnQuadraticCurve(Vector2 a0, Vector2 c0, Vector2 a1, float t)
        {
            return a0 * (1 - t) * (1 - t)
                + c0 * 2 * t * (1 - t)
                + a1 * t * t;
        }

        private static List<WaypointDetails> CreateWaypointDetailsList(List<Waypoint> waypoints, Vector2 p)
        {
            var waypointDetailsList = new List<WaypointDetails>();

            for (var i = 0; i < waypoints.Count - 1; ++i)
            {
                var waypoint = waypoints[i];
                var nextWaypoint = waypoints[i + 1];
                var t = -1f;
                var sqDistance = float.MaxValue;
                var point = Vector2.zero;

                if (waypoint.CurveType == CurveType.Linear)
                {
                    t = GetClosestInterpolationToPoint(p, waypoint.Position, nextWaypoint.Position, waypoint.FollowAxis);
                    point = GetPointOnLinearCurve(waypoint.Position, nextWaypoint.Position, t);
                    sqDistance = (p - point).sqrMagnitude;
                }
                else
                {
                    var tVals =
                        GetInterpolationValuesThatSatisfyPoint(
                            p,
                            waypoint.Position,
                            waypoint.CurveControlPoint,
                            nextWaypoint.Position,
                            waypoint.FollowAxis);

                    if (waypoint.FollowAxis == FollowAxis.XY)
                    {
                        tVals.Add(0);
                        tVals.Add(1);
                    }

                    for (var j = 0; j < tVals.Count; ++j)
                    {
                        var tVal = tVals[j];
                        if (tVal >= 0 && tVal <= 1)
                        {
                            var currentPoint =
                                GetPointOnQuadraticCurve(
                                    waypoint.Position,
                                    waypoint.CurveControlPoint,
                                    nextWaypoint.Position,
                                    tVal);

                            var currentSqDistance = (p - currentPoint).sqrMagnitude;
                            if (currentSqDistance <= sqDistance)
                            {
                                t = tVal;
                                sqDistance = currentSqDistance;
                                point = currentPoint;
                            }
                        }
                    }

                    if (t < 0)
                    {
                        var sqDistanceToWaypoint = GetSqDistanceToPointOnFollowAxis(p, waypoint.Position, waypoint.FollowAxis);
                        var sqDistanceToNextWaypoint = GetSqDistanceToPointOnFollowAxis(p, nextWaypoint.Position, waypoint.FollowAxis);
                        var sqDistanceToBoundPoint = float.MaxValue;
                        var curveBoundT = -1f;
                        var curveBoundPoint = Vector2.zero;

                        if (waypoint.FollowAxis != FollowAxis.XY)
                        {
                            curveBoundT = waypoint.FollowAxis == FollowAxis.X
                                ? GetMaxPointForCurve(waypoint.Position.x, waypoint.CurveControlPoint.x, nextWaypoint.Position.x)
                                : GetMaxPointForCurve(waypoint.Position.y, waypoint.CurveControlPoint.y, nextWaypoint.Position.y);

                            curveBoundPoint =
                                GetPointOnQuadraticCurve(
                                    waypoint.Position,
                                    waypoint.CurveControlPoint,
                                    nextWaypoint.Position,
                                    curveBoundT);

                            if (curveBoundT >= 0 && curveBoundT <= 1)
                            {
                                sqDistanceToBoundPoint = GetSqDistanceToPointOnFollowAxis(p, curveBoundPoint, waypoint.FollowAxis);
                            }
                        }

                        if (sqDistanceToBoundPoint <= sqDistanceToWaypoint
                            && sqDistanceToBoundPoint <= sqDistanceToNextWaypoint)
                        {
                            t = curveBoundT;
                            sqDistance = (p - curveBoundPoint).sqrMagnitude;
                            point = curveBoundPoint;
                        }
                        else if (sqDistanceToWaypoint < sqDistanceToNextWaypoint)
                        {
                            t = 0;
                            sqDistance = (p - waypoint.Position).sqrMagnitude;
                            point = waypoint.Position;
                        }
                        else
                        {
                            t = 1;
                            sqDistance = (p - nextWaypoint.Position).sqrMagnitude;
                            point = nextWaypoint.Position;
                        }
                    }
                }

                waypointDetailsList.Add(new WaypointDetails
                {
                    Waypoint = waypoint,
                    NextWaypoint = nextWaypoint,
                    Interpolation = t,
                    SqDistance = sqDistance
                });
            }

            return waypointDetailsList;
        }

        private static WaypointDetails GetClosestWaypointDetailsToPoint(Vector2 p, List<WaypointDetails> list)
        {
            var details = list[0];

            for (var i = 1; i < list.Count; ++i)
            {
                var d = list[i];
                if (d.SqDistance <= details.SqDistance)
                {
                    details = d;
                }
            }

            return details;
        }

        private static WaypointDetails GetAdjustedClosestWaypointDetailsForCompatibleFollowAxis(
            Vector2 p,
            List<WaypointDetails> list,
            List<Waypoint> waypoints,
            int index)
        {
            var item = list[index];

            if (list.Count < 2 || item.Waypoint.FollowAxis == FollowAxis.XY)
            {
                return item;
            }

            var candidate = list[index];

            if (index < list.Count - 1 && item.Interpolation == 1)
            {
                var i = index;
                var current = item;
                var next = list[i + 1];
                while (i < list.Count - 1
                    && current.Interpolation == 1
                    && next.Waypoint.FollowAxis == item.Waypoint.FollowAxis
                    && IsDirectionSame(
                        next.Waypoint.Position - current.Waypoint.Position,
                        waypoints[i + 2].Position - next.Waypoint.Position,
                        item.Waypoint.FollowAxis))
                {
                    candidate = next;
                    current = next;
                    ++i;
                    if (i < list.Count - 1)
                    {
                        next = list[i + 1];
                    }
                }
            }
            else if (index > 0 && item.Interpolation == 0)
            {
                var i = index;
                var current = item;
                var prev = list[i - 1];
                while (i > 0
                    && current.Interpolation == 0
                    && prev.Waypoint.FollowAxis == item.Waypoint.FollowAxis
                    && IsDirectionSame(
                        current.Waypoint.Position - prev.Waypoint.Position,
                        waypoints[i + 1].Position - current.Waypoint.Position,
                        item.Waypoint.FollowAxis))
                {
                    candidate = prev;
                    current = prev;
                    --i;
                    if (i > 0)
                    {
                        prev = list[i - 1];
                    }
                }
            }

            return candidate;
        }

        private static int GetNextIndexInDirectionLooped(int i, int count, bool isPositiveDirection)
        {
            return isPositiveDirection
                ? i < count - 1 ? i + 1 : 0 : i > 0
                ? i - 1
                : count - 1;
        }

        private static bool IsDirectionSame(Vector2 v0, Vector2 v1, FollowAxis axis)
        {
            return (axis == FollowAxis.X && 0 - v0.x <= 0 == 0 - v1.x <= 0)
                || (axis == FollowAxis.Y && 0 - v0.y <= 0 == 0 - v1.y <= 0);
        }

        private static float GetSqDistanceToPointOnFollowAxis(Vector2 p0, Vector2 p1, FollowAxis axis)
        {
            return axis == FollowAxis.XY
                ? (p0 - p1).sqrMagnitude
                : axis == FollowAxis.X
                ? (p0.x - p1.x) * (p0.x - p1.x) : (p0.y - p1.y) * (p0.y - p1.y);
        }

        private static float GetClosestInterpolationToPoint(Vector2 p, Vector2 a0, Vector2 a1, FollowAxis axis = FollowAxis.XY)
        {
            if (axis == FollowAxis.X)
            {
                return Mathf.InverseLerp(a0.x, a1.x, p.x);
            }

            if (axis == FollowAxis.Y)
            {
                return Mathf.InverseLerp(a0.y, a1.y, p.y);
            }

            var a0a1 = a1 - a0;
            return Mathf.Clamp(Vector2.Dot(p - a0, a0a1) / a0a1.sqrMagnitude, 0, 1);
        }

        private static List<float> GetInterpolationValuesThatSatisfyPoint(Vector2 p, Vector2 a0, Vector2 c0, Vector2 a1, FollowAxis axis)
        {
            if (axis == FollowAxis.XY)
            {
                var a0c0 = c0 - a0;
                var c0_ = a1 - c0 - a0c0;
                var pa0 = a0 - p;

                var a = c0_.x * c0_.x + c0_.y * c0_.y;
                var b = 3 * (a0c0.x * c0_.x + a0c0.y * c0_.y);
                var c = 2 * (a0c0.x * a0c0.x + a0c0.y * a0c0.y) + pa0.x * c0_.x + pa0.y * c0_.y;
                var d = pa0.x * a0c0.x + pa0.y * a0c0.y;

                return SolveCubicEquation(a, b, c, d);
            }
            else
            {
                var a = axis == FollowAxis.X ? a0.x - 2 * c0.x + a1.x : a0.y - 2 * c0.y + a1.y;
                var b = axis == FollowAxis.X ? 2 * (c0.x - a0.x) : 2 * (c0.y - a0.y);
                var c = axis == FollowAxis.X ? a0.x - p.x : a0.y - p.y;

                return SolveQuadraticEquation(a, b, c);
            }
        }

        private static float GetMaxPointForCurve(float a0, float c0, float a1)
        {
            if (2 * c0 == a0 + a1)
            {
                return -1;
            }

            return (c0 - a0) / (2 * c0 - a0 - a1);
        }

        private static float SolveLinearEquation(float a, float b)
        {
            return -b / a;
        }

        private static List<float> SolveQuadraticEquation(float a, float b, float c)
        {
            if (a == 0)
            {
                return new List<float> {
                SolveLinearEquation(b, c)
                };
            }

            var discriminant = b * b - 4 * a * c;
            var rootD = Mathf.Sqrt(discriminant);

            if (discriminant < 0)
            {
                return new List<float>();
            }
            else
            {
                return new List<float> {
                    (-b + rootD) / (2 * a),
                    (-b - rootD) / (2 * a)
                };
            }
        }

        // Cardano method
        private static List<float> SolveCubicEquation(float a, float b, float c, float d)
        {
            if (a == 0)
            {
                return SolveQuadraticEquation(b, c, d);
            }

            b /= a;
            c /= a;
            d /= a;
            var p = (3 * c - b * b) / 3;
            var q = (2 * b * b * b - 9 * b * c + 27 * d) / 27;

            if (p == 0)
            {
                return new List<float> {
                GetRealCubeRoot(-q)
                };
            }

            var discriminant = q * q / 4 + p * p * p / 27;

            if (discriminant == 0)
            {
                return new List<float> {
                GetRealCubeRoot(q / 2) - b / 3
                };
            }

            if (discriminant > 0)
            {
                return new List<float> {
                    GetRealCubeRoot(-(q / 2) + Mathf.Sqrt(discriminant))
                    - GetRealCubeRoot((q / 2) + Mathf.Sqrt(discriminant))
                    - b / 3
                };
            }

            var r = Mathf.Sqrt(Mathf.Pow(-(p / 3), 3));
            var phi = Mathf.Acos(-(q / (2 * r)));
            var s = 2 * GetRealCubeRoot(r);

            return new List<float> {
                s * Mathf.Cos(phi / 3) - b / 3,
                s * Mathf.Cos((phi + 2 * Mathf.PI) / 3) - b / 3,
                s * Mathf.Cos((phi + 4 * Mathf.PI) / 3) - b / 3
            };
        }

        private static float GetRealCubeRoot(float a)
        {
            return a < 0 ? -Mathf.Pow(-a, 1f / 3) : Mathf.Pow(a, 1f / 3);
        }

        private static float GetEffectedInterpolation(List<Effect> effects, int waypointIndex, float t)
        {
            if (effects == null)
            {
                return t;
            }

            for (var i = 0; i < effects.Count; ++i)
            {
                var effect = effects[i];
                if (!effect.Active || effect.WaypointIndex != waypointIndex)
                {
                    continue;
                }

                var cP = Mathf.Clamp(effect.CameraInterpolation, 0, 1);
                var tP = Mathf.Clamp(effect.TargetInterpolation, 0, 1);

                t = Mathf.Clamp(t, 0, 1);

                if (t <= tP && tP != 0)
                {
                    t = cP * t / tP;
                }
                else
                {
                    t = cP + (1 - cP) * (t - tP) / (1 - tP);
                }
            }

            return Mathf.Clamp(t, 0, 1);
        }
    }
}

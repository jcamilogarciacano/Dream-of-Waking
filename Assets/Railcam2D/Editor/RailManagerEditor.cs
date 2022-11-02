using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Railcam2D
{

    [CustomEditor(typeof(RailManager))]
    public class RailManagerEditor : Editor
    {
        private enum TabViewType
        {
            Waypoints,
            Effects
        }

        private static Regex _railNameRegex = new Regex(@"^Rail [\d]+$");
        private static EditorApplication.CallbackFunction _callbackRemoveObject = new EditorApplication.CallbackFunction(DestroyImmediate);
        private static GameObject _railObjectToDestroy;
        private Rail[] _rails;
        private Rail _rail;
        private List<Effect> _effects = new List<Effect>();
        private ReorderableList _effectsList;
        private List<Waypoint> _waypoints = new List<Waypoint>();
        private ReorderableList _waypointsList;
        private bool _shouldRepaintScene;
        private TabViewType _currentTabViewType;
        private SerializedObject _settings;
        private float _handleSizeFactor = 0.2f;
        private RailManager _railManager;

        public void OnEnable()
        {
            _railManager = (RailManager)target;
            if (_railManager == null)
            {
                return;
            }

            _settings = Railcam2DSettings.GetSerializedSettings();

            _rails = GetRailsOrderedByName();
            SetCurrentRail(_rails.Length > 0 ? _rails[0] : null);

            _waypointsList = new ReorderableList(_waypoints, typeof(Waypoint))
            {
                headerHeight = 0,
                onAddCallback = (list) =>
                {
                    var waypoints = _rail.Waypoints;
                    AddWaypoint(_rail, waypoints.Count, waypoints[waypoints.Count - 1].Position + new Vector2(10, 10));
                    _shouldRepaintScene = true;
                },
                onRemoveCallback = (list) =>
                {
                    RemoveWaypoint(_rail, list.index);
                },
                onCanRemoveCallback = (list) =>
                {
                    return _rail.Waypoints.Count > 2;
                },
                onReorderCallbackWithDetails = (list, oldIndex, newIndex) =>
                {
                    // list has already been reordered, so we need to reverse the reorder,
                    // then reorder again but utilizing ability to undo.
                    var oldWaypoint = _rail.Waypoints[newIndex];
                    _rail.Waypoints.RemoveAt(newIndex);
                    _rail.Waypoints.Insert(oldIndex, oldWaypoint);
                    SetWaypointOrder(_rail, oldIndex, newIndex);
                },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    rect.y += 4;

                    var waypoint = (Waypoint)_waypointsList.list[index];

                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Waypoint " + index, ""));

                    var xyColor = _settings.FindProperty("rail_waypoint_follow_xy_color").colorValue;
                    var xColor = _settings.FindProperty("rail_waypoint_follow_x_color").colorValue;
                    var yColor = _settings.FindProperty("rail_waypoint_follow_y_color").colorValue;

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 4, 80, 10), new GUIContent("Follow Axis", ""));

                    var oldGUIColor = GUI.backgroundColor;
                    GUI.backgroundColor = waypoint.FollowAxis == FollowAxis.XY
                        ? xyColor
                        : waypoint.FollowAxis == FollowAxis.X
                        ? xColor
                        : yColor;

                    var newFollowAxis = (FollowAxis)EditorGUI.EnumPopup(
                        new Rect(
                            rect.x + 80,
                            rect.y + (EditorGUIUtility.singleLineHeight + 4),
                            rect.width - 80,
                            EditorGUIUtility.singleLineHeight),
                        waypoint.FollowAxis);
                    if (waypoint.FollowAxis != newFollowAxis)
                    {
                        SetWaypointFollowAxis(_rail, waypoint, newFollowAxis);
                    }

                    GUI.backgroundColor = oldGUIColor;

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + (EditorGUIUtility.singleLineHeight + 4) * 2, 80, 10), new GUIContent("Position", ""));
                    var newPosition = EditorGUI.Vector2Field(
                        new Rect(
                            rect.x + 80,
                            rect.y + (EditorGUIUtility.singleLineHeight + 4) * 2,
                            rect.width - 80,
                            EditorGUIUtility.singleLineHeight),
                        "",
                        waypoint.Position);
                    if (waypoint.Position != newPosition)
                    {
                        SetWaypointPosition(_rail, waypoint, newPosition);
                    }

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + (EditorGUIUtility.singleLineHeight + 4) * 3, 80, 10), new GUIContent("Curve Type", ""));
                    var newCurveType = (CurveType)EditorGUI.EnumPopup(new Rect(
                            rect.x + 80,
                            rect.y + (EditorGUIUtility.singleLineHeight + 4) * 3,
                            (rect.width - 80) / 2 - 1,
                            EditorGUIUtility.singleLineHeight),
                        waypoint.CurveType);
                    if (waypoint.CurveType != newCurveType)
                    {
                        SetWaypointCurveType(_rail, waypoint, newCurveType);
                    }

                    if (waypoint.CurveType == CurveType.Linear)
                    {
                        GUI.enabled = false;
                    }

                    var newCurveControlPoint = EditorGUI.Vector2Field(new Rect(
                            rect.x + 80 + (rect.width - 80) / 2 + 1,
                            rect.y + (EditorGUIUtility.singleLineHeight + 4) * 3,
                            (rect.width - 80) / 2 - 1,
                            EditorGUIUtility.singleLineHeight),
                        "",
                        waypoint.CurveControlPoint);
                    if (waypoint.CurveControlPoint != newCurveControlPoint)
                    {
                        SetWaypointCurveControlPoint(_rail, waypoint, newCurveControlPoint);
                    }

                    GUI.enabled = true;
                }
            };

            _effectsList = new ReorderableList(_effects, typeof(Effect))
            {
                headerHeight = 0,
                onAddCallback = (list) =>
                {
                    AddEffect(_rail);
                },
                onRemoveCallback = (list) =>
                {
                    RemoveEffect(_rail, list.index);
                },
                onReorderCallbackWithDetails = (list, oldIndex, newIndex) =>
                {
                    // list has already been reordered, so we need to reverse the reorder,
                    // then reorder again but utilizing ability to undo.
                    var oldEffect = _rail.Effects[newIndex];
                    _rail.Effects.RemoveAt(newIndex);
                    _rail.Effects.Insert(oldIndex, oldEffect);
                    SetEffectOrder(_rail, oldIndex, newIndex);
                },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    rect.y += 4;

                    var effect = (Effect)_effectsList.list[index];

                    var waypointLabels = new List<GUIContent>() {
                            new GUIContent("-")
                        };
                    for (var i = 0; i < _rail.Waypoints.Count; ++i)
                    {
                        waypointLabels.Add(new GUIContent(i.ToString()));
                    }

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, 80, 10), new GUIContent("Active", ""));
                    var newActive = EditorGUI.Toggle(
                        new Rect(
                            rect.x + 80,
                            rect.y,
                            16,
                            EditorGUIUtility.singleLineHeight),
                        effect.Active);
                    if (effect.Active != newActive)
                    {
                        SetEffectActive(_rail, effect, newActive);
                    }

                    var waypointIndexPlusOne = effect.WaypointIndex + 1;
                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 4, 80, 10), new GUIContent("Waypoint", ""));
                    var newWaypointIndexPlusOne = EditorGUI.Popup(
                        new Rect(
                            rect.x + 80,
                            rect.y + EditorGUIUtility.singleLineHeight + 4,
                            rect.width - 80,
                            EditorGUIUtility.singleLineHeight),
                        waypointIndexPlusOne, waypointLabels.ToArray());
                    if (waypointIndexPlusOne != newWaypointIndexPlusOne)
                    {
                        SetEffectWaypoint(_rail, effect, newWaypointIndexPlusOne - 1);
                    }

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + (EditorGUIUtility.singleLineHeight + 4) * 2, 80, 10), new GUIContent("Camera Pos", ""));
                    var newCameraInterpolation = EditorGUI.Slider(new Rect(
                            rect.x + 80,
                            rect.y + (EditorGUIUtility.singleLineHeight + 4) * 2,
                            rect.width - 80,
                            EditorGUIUtility.singleLineHeight),
                        effect.CameraInterpolation, 0, 1);
                    if (effect.CameraInterpolation != newCameraInterpolation)
                    {
                        SetEffectCameraInterpolation(_rail, effect, newCameraInterpolation);
                    }

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + (EditorGUIUtility.singleLineHeight + 4) * 3, 80, 10), new GUIContent("Target Pos", ""));
                    var newTargetInterpolation = EditorGUI.Slider(new Rect(
                            rect.x + 80,
                            rect.y + (EditorGUIUtility.singleLineHeight + 4) * 3,
                            rect.width - 80,
                            EditorGUIUtility.singleLineHeight),
                        effect.TargetInterpolation, 0, 1);
                    if (effect.TargetInterpolation != newTargetInterpolation)
                    {
                        SetEffectTargetInterpolation(_rail, effect, newTargetInterpolation);
                    }
                }
            };
        }

        public void OnSceneGUI()
        {
            if (_railManager == null)
            {
                return;
            }

            _rails = GetRailsOrderedByName();

            var isWaypointDeleteModeOn = Event.current.shift;
            var isGridSnapOn = Application.platform == RuntimePlatform.OSXEditor ? Event.current.command : Event.current.control;

            if (_railManager.ViewSingle)
            {
                RenderRail(_rail, isWaypointDeleteModeOn, isGridSnapOn);
            }
            else
            {
                for (var i = 0; i < _rails.Length; i++)
                {
                    RenderRail(_rails[i], isWaypointDeleteModeOn, isGridSnapOn);
                }
            }

            // Sync editor values with scene if rail is displayed in editor
            if (_rail != null)
            {
                Repaint();
            }
        }

        private void RenderRail(Rail rail, bool isWaypointDeleteModeOn, bool isGridSnapOn)
        {
            if (rail == null)
            {
                return;
            }

            RenderRailPath(rail);
            RenderRailNameLabel(rail);
            RenderWaypointIndexLabels(rail);

            if (isWaypointDeleteModeOn && rail.Waypoints.Count > 2)
            {
                RenderWaypointDeleteButtons(rail);
            }
            else
            {
                RenderWaypointCreateButtons(rail, isGridSnapOn);
                RenderWaypointPositionHandles(rail, isGridSnapOn);
                RenderWaypointControlPointPositionHandles(rail, isGridSnapOn);
                RenderEffectHandles(rail);
            }
        }

        private Rail[] GetRailsOrderedByName()
        {
            return UnityEngine.Object.FindObjectsOfType<Rail>().OrderBy(r => r.gameObject.name).ToArray();
        }

        private void RenderEffectHandles(Rail rail)
        {
            if (rail == null || rail.gameObject == null)
            {
                return;
            }

            var effects = rail.Effects;
            var waypoints = rail.Waypoints;

            var zPos = rail.gameObject.transform.position.z;

            var handleSize = HandleUtility.GetHandleSize(Vector2.zero);

            var oldHandleColor = Handles.color;

            var targetColor = _settings.FindProperty("rail_effect_target_point_color").colorValue;
            var xyColor = _settings.FindProperty("rail_waypoint_follow_xy_color").colorValue;
            var xColor = _settings.FindProperty("rail_waypoint_follow_x_color").colorValue;
            var yColor = _settings.FindProperty("rail_waypoint_follow_y_color").colorValue;
            var inactiveColor = _settings.FindProperty("rail_inactive_color").colorValue;
            var railWidth = _settings.FindProperty("rail_width").floatValue;

            for (var i = 0; i < effects.Count; ++i)
            {
                var effect = effects[i];

                if (effect.WaypointIndex > -1 && effect.WaypointIndex < waypoints.Count - 1)
                {
                    var waypoint = waypoints[effect.WaypointIndex];
                    var nextWaypoint = waypoints[effect.WaypointIndex + 1];

                    var camColor = !rail.Active ? inactiveColor : waypoint.FollowAxis == FollowAxis.XY ? xyColor : waypoint.FollowAxis == FollowAxis.X ? xColor : yColor;

                    var camPos = waypoint.CurveType == CurveType.Linear
                        ? GetPointOnLinearCurve(waypoint.Position, nextWaypoint.Position, effect.CameraInterpolation)
                        : GetPointOnQuadraticCurve(waypoint.Position, waypoint.CurveControlPoint, nextWaypoint.Position, effect.CameraInterpolation);

                    var targetPos = waypoint.CurveType == CurveType.Linear
                        ? GetPointOnLinearCurve(waypoint.Position, nextWaypoint.Position, effect.TargetInterpolation)
                        : GetPointOnQuadraticCurve(waypoint.Position, waypoint.CurveControlPoint, nextWaypoint.Position, effect.TargetInterpolation);

                    var camPosV3 = new Vector3(camPos.x, camPos.y, zPos);
                    var targetPosV3 = new Vector3(targetPos.x, targetPos.y, zPos);

                    var a0 = waypoint.Position;
                    var c0 = waypoint.CurveControlPoint;
                    var a1 = nextWaypoint.Position;

                    var angleCam = GetAngleOfCurveAtPoint(a0, c0, a1, waypoint.CurveType, effect.CameraInterpolation);
                    var angleTarget = GetAngleOfCurveAtPoint(a0, c0, a1, waypoint.CurveType, effect.TargetInterpolation);

                    var railWidthFactor = 0.03f;

                    var cameraV1 = Quaternion.Euler(0, 0, angleCam) * new Vector3(0.25f * _handleSizeFactor * handleSize, 0.75f * _handleSizeFactor * handleSize + railWidth * railWidthFactor * _handleSizeFactor * handleSize, 0);
                    var cameraV2 = Quaternion.Euler(0, 0, angleCam) * new Vector3(-0.25f * _handleSizeFactor * handleSize, 0.75f * _handleSizeFactor * handleSize + railWidth * railWidthFactor * _handleSizeFactor * handleSize, 0);
                    var cameraV3 = Quaternion.Euler(0, 0, angleCam) * new Vector3(0, railWidth * railWidthFactor * _handleSizeFactor * handleSize, 0);
                    var targetV1 = Quaternion.Euler(0, 0, angleTarget) * new Vector3(0.25f * _handleSizeFactor * handleSize, 0.75f * _handleSizeFactor * handleSize + railWidth * railWidthFactor * _handleSizeFactor * handleSize, 0);
                    var targetV2 = Quaternion.Euler(0, 0, angleTarget) * new Vector3(-0.25f * _handleSizeFactor * handleSize, 0.75f * _handleSizeFactor * handleSize + railWidth * railWidthFactor * _handleSizeFactor * handleSize, 0);
                    var targetV3 = Quaternion.Euler(0, 0, angleTarget) * new Vector3(0, railWidth * railWidthFactor * _handleSizeFactor * handleSize, 0);

                    Handles.color = camColor;
                    Handles.DrawAAConvexPolygon(camPosV3 + cameraV3, camPosV3 + cameraV1, camPosV3 + cameraV2);
                    Handles.DrawAAConvexPolygon(camPosV3 - cameraV3, camPosV3 - cameraV1, camPosV3 - cameraV2);
                    Handles.color = targetColor;
                    Handles.DrawAAConvexPolygon(targetPosV3 + targetV3, targetPosV3 + targetV1, targetPosV3 + targetV2);
                    Handles.DrawAAConvexPolygon(targetPosV3 - targetV3, targetPosV3 - targetV1, targetPosV3 - targetV2);
                }
            }

            Handles.color = oldHandleColor;
        }

        private float GetAngleOfCurveAtPoint(Vector2 a0, Vector2 c0, Vector2 a1, CurveType curveType, float t)
        {
            var gradient = curveType == CurveType.Linear ? GetGradientAtPointOnLinearCurve(a0, a1) : GetGradientAtPointOnQuadraticCurve(a0, c0, a1, t);
            var angle = gradient == Mathf.Infinity ? 90 : Vector2.Angle(new Vector2(1, 0), new Vector2(1, gradient));
            return gradient < 0 ? -angle : angle;
        }

        private float GetGradientAtPointOnLinearCurve(Vector2 a0, Vector2 a1)
        {
            return (a1.y - a0.y) / (a1.x - a0.x);
        }

        private float GetGradientAtPointOnQuadraticCurve(Vector2 a0, Vector3 c0, Vector2 a1, float t)
        {
            return (t * 2 * (a0.y - 2 * c0.y + a1.y) + 2 * (c0.y - a0.y)) / (t * 2 * (a0.x - 2 * c0.x + a1.x) + 2 * (c0.x - a0.x));
        }

        private void RenderRailPath(Rail rail)
        {
            if (rail == null || rail.gameObject == null)
            {
                return;
            }

            var waypoints = rail.Waypoints;
            if (waypoints.Count < 2)
            {
                return;
            }

            var oldHandleColor = Handles.color;
            var xyColor = _settings.FindProperty("rail_waypoint_follow_xy_color").colorValue;
            var xColor = _settings.FindProperty("rail_waypoint_follow_x_color").colorValue;
            var yColor = _settings.FindProperty("rail_waypoint_follow_y_color").colorValue;
            var inactiveColor = _settings.FindProperty("rail_inactive_color").colorValue;
            var width = _settings.FindProperty("rail_width").floatValue;
            var isAntialiasingEnabled = _settings.FindProperty("is_antialiasing_enabled").boolValue;

            var lineTex = new Texture2D(1, 3);

            if (isAntialiasingEnabled)
            {
                lineTex.SetPixels(new Color[] { new Color(1, 1, 1, 0), new Color(0.75f, 0.75f, 0.75f, 1), new Color(1, 1, 1, 1) }, 0);
            }
            else
            {
                lineTex.SetPixels(new Color[] { new Color(1, 1, 1, 1), new Color(1, 1, 1, 1), new Color(1, 1, 1, 1) }, 0);
            }

            var zPos = rail.gameObject.transform.position.z;

            for (var i = 0; i < waypoints.Count - 1; ++i)
            {
                var waypoint = waypoints[i];
                var nextWaypoint = waypoints[i + 1];

                Handles.color = !rail.Active ? inactiveColor : waypoint.FollowAxis == FollowAxis.XY ? xyColor : waypoint.FollowAxis == FollowAxis.X ? xColor : yColor;
                var waypointPosV3 = new Vector3(waypoint.Position.x, waypoint.Position.y, zPos);
                var nextWaypointPosV3 = new Vector3(nextWaypoint.Position.x, nextWaypoint.Position.y, zPos);

                if (waypoint.CurveType == CurveType.Linear)
                {
                    Handles.DrawAAPolyLine(lineTex, width, waypointPosV3, nextWaypointPosV3);
                }
                else
                {
                    var controlPoint = waypoint.CurveControlPoint;
                    var controlPointPosV3 = new Vector3(waypoint.CurveControlPoint.x, waypoint.CurveControlPoint.y, zPos);
                    Handles.DrawBezier(waypointPosV3, nextWaypointPosV3, waypointPosV3 + ((controlPointPosV3 - waypointPosV3) * 2 / 3), nextWaypointPosV3 + ((controlPointPosV3 - nextWaypointPosV3) * 2 / 3), Handles.color, lineTex, width);
                }
            }

            Handles.color = oldHandleColor;
        }

        private void RenderRailNameLabel(Rail rail)
        {
            var waypoints = rail.Waypoints;

            if (waypoints.Count < 1)
            {
                return;
            }

            var color = _settings.FindProperty("rail_name_text_color").colorValue;
            var backgroundColor = _settings.FindProperty("rail_name_text_background_color").colorValue;
            var textSize = _settings.FindProperty("rail_name_text_size").intValue;
            var size = _settings.FindProperty("rail_waypoint_handle_size").floatValue * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero);
            var labelOffset = new Vector3(size, -size * 1.5f, 0);
            var posV3 = new Vector3(waypoints[0].Position.x, waypoints[0].Position.y, rail.gameObject.transform.position.z) + labelOffset;

            RenderTextAtPosition(rail.gameObject.name, posV3, color, backgroundColor, textSize);
        }

        private void RenderWaypointIndexLabels(Rail rail)
        {
            var waypoints = rail.Waypoints;

            var color = _settings.FindProperty("rail_waypoint_index_text_color").colorValue;
            var backgroundColor = _settings.FindProperty("rail_waypoint_index_text_background_color").colorValue;
            var textSize = _settings.FindProperty("rail_waypoint_index_text_size").intValue;
            var size = _settings.FindProperty("rail_waypoint_handle_size").floatValue * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero);
            var labelOffset = new Vector3(-size, -size * 1.5f, 0);

            for (var i = 0; i < waypoints.Count; ++i)
            {
                var posV3 = new Vector3(waypoints[i].Position.x, waypoints[i].Position.y, rail.gameObject.transform.position.z) + labelOffset;
                RenderTextAtPosition(i.ToString(), posV3, color, backgroundColor, textSize);
            }
        }

        private void RenderTextAtPosition(string text, Vector3 position, Color color, Color backgroundColor, int size)
        {
            GUIStyle style = new GUIStyle("Label");
            style.fontSize = size;
            style.normal.textColor = color;
            style.normal.background = new Texture2D(1, 1);
            style.normal.background.SetPixels(new Color[] { backgroundColor });
            Handles.Label(position, text, style);
        }

        private void RenderWaypointDeleteButtons(Rail rail)
        {
            var waypoints = rail.Waypoints;

            var oldHandleColor = Handles.color;
            Handles.color = _settings.FindProperty("rail_waypoint_delete_button_color").colorValue;
            var size = _settings.FindProperty("rail_waypoint_handle_size").floatValue * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero);

            var zPos = rail.gameObject.transform.position.z;

            for (var i = 0; i < waypoints.Count; ++i)
            {
                var posV3 = new Vector3(waypoints[i].Position.x, waypoints[i].Position.y, zPos);
                if (Handles.Button(posV3, Quaternion.identity, size, size, Handles.DotHandleCap))
                {
                    RemoveWaypoint(rail, i);
                }
            }

            Handles.color = oldHandleColor;
        }

        private void RenderWaypointCreateButtons(Rail rail, bool isGridSnapOn)
        {
            var waypoints = rail.Waypoints;

            if (waypoints.Count < 2)
            {
                return;
            }

            var oldHandleColor = Handles.color;
            Handles.color = _settings.FindProperty("rail_waypoint_create_button_color").colorValue;
            var size = _settings.FindProperty("rail_waypoint_create_button_size").floatValue * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero);
            var snapSize = _settings.FindProperty("grid_snap_size").vector2Value;

            var prependButtonPos = waypoints[0].CurveType == CurveType.Quadratic
                ? waypoints[0].Position - (waypoints[0].CurveControlPoint - waypoints[0].Position).normalized * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero) * 2
                : waypoints[0].Position - (waypoints[1].Position - waypoints[0].Position).normalized * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero) * 2;
            var appendButtonPos = waypoints[waypoints.Count - 2].CurveType == CurveType.Quadratic
                ? waypoints[waypoints.Count - 1].Position + (waypoints[waypoints.Count - 1].Position - waypoints[waypoints.Count - 2].CurveControlPoint).normalized * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero) * 2
                : waypoints[waypoints.Count - 1].Position + (waypoints[waypoints.Count - 1].Position - waypoints[waypoints.Count - 2].Position).normalized * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero) * 2;

            if (isGridSnapOn)
            {
                prependButtonPos =
                    new Vector2(Mathf.Round(prependButtonPos.x / snapSize.x) * snapSize.x, Mathf.Round(prependButtonPos.y / snapSize.y) * snapSize.y);
                appendButtonPos =
                    new Vector2(Mathf.Round(appendButtonPos.x / snapSize.x) * snapSize.x, Mathf.Round(appendButtonPos.y / snapSize.y) * snapSize.y);
            }

            var zPos = rail.gameObject.transform.position.z;

            for (var i = 0; i < waypoints.Count; i++)
            {

                var waypoint = waypoints[i];

                if (i == 0)
                {
                    var posV3 = new Vector3(prependButtonPos.x, prependButtonPos.y, zPos);
                    if (Handles.Button(posV3, Quaternion.identity, size, size, Handles.RectangleHandleCap))
                    {
                        AddWaypoint(rail, 0, prependButtonPos);
                    }
                }

                if (i < waypoints.Count - 1)
                {
                    var pos = waypoint.CurveType == CurveType.Quadratic
                        ? GetPointOnQuadraticCurve(waypoint.Position, waypoint.CurveControlPoint, waypoints[i + 1].Position, 0.5f)
                        : (waypoints[i].Position + waypoints[i + 1].Position) / 2;

                    if (isGridSnapOn)
                    {
                        pos.x = Mathf.Round(pos.x / snapSize.x) * snapSize.x;
                        pos.y = Mathf.Round(pos.y / snapSize.y) * snapSize.y;
                    }

                    var posV3 = new Vector3(pos.x, pos.y, zPos);
                    if (Handles.Button(posV3, Quaternion.identity, size, size, Handles.RectangleHandleCap))
                    {
                        AddWaypoint(rail, i + 1, pos);
                    }
                }

                if (i == waypoints.Count - 1)
                {
                    var posV3 = new Vector3(appendButtonPos.x, appendButtonPos.y, zPos);
                    if (Handles.Button(posV3, Quaternion.identity, size, size, Handles.RectangleHandleCap))
                    {
                        AddWaypoint(rail, waypoints.Count, appendButtonPos);
                    }
                }
            }

            Handles.color = oldHandleColor;
        }

        private void RenderWaypointControlPointPositionHandles(Rail rail, bool isGridSnapOn)
        {
            var oldHandleColor = Handles.color;
            var handleColor = _settings.FindProperty("control_point_handle_color").colorValue;
            var line1Color = _settings.FindProperty("control_point_line_1_color").colorValue;
            var line2Color = _settings.FindProperty("control_point_line_2_color").colorValue;
            var size = _settings.FindProperty("control_point_handle_size").floatValue * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero);
            var width = _settings.FindProperty("control_point_line_width").floatValue;
            var snapSize = _settings.FindProperty("grid_snap_size").vector2Value;
            var isAntialiasingEnabled = _settings.FindProperty("is_antialiasing_enabled").boolValue;
            var lineTex = new Texture2D(1, 3);
            var zPos = rail.gameObject.transform.position.z;
            var waypoints = rail.Waypoints;

            if (isAntialiasingEnabled)
            {
                lineTex.SetPixels(new Color[] { new Color(1, 1, 1, 0), new Color(0.75f, 0.75f, 0.75f, 1), new Color(1, 1, 1, 1) }, 0);
            }
            else
            {
                lineTex.SetPixels(new Color[] { new Color(1, 1, 1, 1), new Color(1, 1, 1, 1), new Color(1, 1, 1, 1) }, 0);
            }

            for (var i = 0; i < waypoints.Count - 1; ++i)
            {
                var waypoint = waypoints[i];
                var nextWaypoint = waypoints[i + 1];

                if (waypoint.CurveType == CurveType.Quadratic)
                {
                    var controlPointPosV3 = new Vector3(waypoint.CurveControlPoint.x, waypoint.CurveControlPoint.y, zPos);
                    var nodePosV3 = new Vector3(waypoint.Position.x, waypoint.Position.y, zPos);
                    var nextWaypointPosV3 = new Vector3(nextWaypoint.Position.x, nextWaypoint.Position.y, zPos);

                    Handles.color = handleColor;
                    var newControlPoint = (Vector2)Handles.FreeMoveHandle(controlPointPosV3, Quaternion.identity, size, snapSize, Handles.CircleHandleCap);
                    if (waypoint.CurveControlPoint != newControlPoint)
                    {
                        if (isGridSnapOn)
                        {
                            if (snapSize.x > 0)
                            {
                                newControlPoint.x = snapSize.x * Mathf.Round(newControlPoint.x / snapSize.x);
                            }
                            if (snapSize.y > 0)
                            {
                                newControlPoint.y = snapSize.y * Mathf.Round(newControlPoint.y / snapSize.y);
                            }
                        }
                        SetWaypointCurveControlPoint(rail, waypoint, newControlPoint);
                    }
                    Handles.color = line1Color;
                    Handles.DrawAAPolyLine(lineTex, width, nodePosV3, controlPointPosV3);
                    Handles.color = line2Color;
                    Handles.DrawAAPolyLine(lineTex, width, nextWaypointPosV3, controlPointPosV3);
                }
            }

            Handles.color = oldHandleColor;
        }

        private void RenderWaypointPositionHandles(Rail rail, bool isGridSnapOn)
        {
            var waypoints = rail.Waypoints;
            var oldHandleColor = Handles.color;
            Handles.color = _settings.FindProperty("rail_waypoint_handle_color").colorValue;
            var size = _settings.FindProperty("rail_waypoint_handle_size").floatValue * _handleSizeFactor * HandleUtility.GetHandleSize(Vector2.zero);
            var snapSize = _settings.FindProperty("grid_snap_size").vector2Value;
            var zPos = rail.gameObject.transform.position.z;

            for (var i = 0; i < waypoints.Count; ++i)
            {
                var waypoint = waypoints[i];
                var nodePosV3 = new Vector3(waypoint.Position.x, waypoint.Position.y, zPos);

                Vector2 newPosition = Handles.FreeMoveHandle(nodePosV3, Quaternion.identity, size, snapSize, Handles.RectangleHandleCap);
                if (waypoint.Position != newPosition)
                {
                    if (isGridSnapOn)
                    {
                        if (snapSize.x > 0)
                        {
                            newPosition.x = snapSize.x * Mathf.Round(newPosition.x / snapSize.x);
                        }
                        if (snapSize.y > 0)
                        {
                            newPosition.y = snapSize.y * Mathf.Round(newPosition.y / snapSize.y);
                        }
                    }
                    SetWaypointPosition(rail, waypoint, newPosition);
                }
            }

            Handles.color = oldHandleColor;
        }

        public override void OnInspectorGUI()
        {
            if (_railManager == null)
            {
                return;
            }

            serializedObject.Update();

            _rails = GetRailsOrderedByName();
            if (_rail == null && _rails.Length > 0)
            {
                for (var i = 0; i < _rails.Length; ++i)
                {
                    var rail = _rails[i];
                    if (rail.gameObject != null && rail.gameObject != _railObjectToDestroy)
                    {
                        SetCurrentRail(rail);
                        break;
                    }
                }
            }

            if (_rail != null)
            {
                _waypointsList.list = _rail.Waypoints;
                _effectsList.list = _rail.Effects;
            }
            else
            {
                _waypointsList.list = new List<Waypoint>();
                _effectsList.list = new List<Effect>();
            }

            if (_shouldRepaintScene)
            {

                SceneView.RepaintAll();
                _shouldRepaintScene = false;
            }

            EditorGUILayout.Space();

            RenderViewSingleToggle();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            RenderNewRailButton();

            if (_rail == null)
            {
                RenderNoRailsText();
                EditorGUILayout.EndHorizontal();
                return;
            }

            RenderRailPopup();
            RenderActionButtons();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            RenderActiveToggle();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            RenderTabSwitcher();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (_currentTabViewType == TabViewType.Effects)
            {
                RenderEffectsTab();
            }
            else
            {
                RenderWaypointsTab();
            }

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }

        private void RenderNewRailButton()
        {
            if (EditorApplication.isPlaying)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button(new GUIContent("New Rail", "Add a new rail object to the scene"), GUILayout.Width(96), GUILayout.Height(24)))
            {
                var nameIndexes = new int[_rails.Length];
                for (var i = 0; i < _rails.Length; ++i)
                {
                    var name = _rails[i].name;
                    if (_railNameRegex.IsMatch(name))
                    {
                        if (int.TryParse(name.Split(' ')[1], out int res))
                        {
                            nameIndexes[i] = res;
                        }
                    }
                }

                var index = nameIndexes.Length < 1 ? 0 : nameIndexes.Max() + 1;
                CreateRail(index);
            }

            GUI.enabled = true;
        }

        private void RenderNoRailsText()
        {
            GUILayout.BeginVertical();
            var headerStyle = new GUIStyle("Label")
            {
                fontSize = 13,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label("Add a rail using the button", headerStyle, GUILayout.Height(24));

            GUILayout.EndVertical();
        }

        private void RenderRailPopup()
        {
            var railNames = new string[_rails.Length];
            for (var i = 0; i < _rails.Length; ++i)
            {
                railNames[i] = _rails[i].name;
            }

            var style = new GUIStyle("popup");
            style.fixedHeight = 24f;
            style.fontSize = 17;

            var railIndex = Array.IndexOf(_rails, _rail);
            var newRailIndex = EditorGUILayout.Popup(railIndex, railNames, style, GUILayout.Height(24));
            if (railIndex != newRailIndex)
            {
                SetCurrentRail(_rails[newRailIndex]);
                _shouldRepaintScene = true;
            }
        }

        private void RenderActionButtons()
        {
            if (GUILayout.Button(new GUIContent("Show", "Move the scene view to this rail's position"), GUILayout.Height(24)))
            {
                if (_rail.Waypoints.Count > 1 && SceneView.lastActiveSceneView != null)
                {
                    var sceneViewHeight = 0f;
                    var sceneViewPadding = 1.5f;
                    var railDisplacementX = Mathf.Abs(_rail.Waypoints[0].Position.x - _rail.Waypoints[_rail.Waypoints.Count - 1].Position.x);
                    var railDisplacementY = Mathf.Abs(_rail.Waypoints[0].Position.y - _rail.Waypoints[_rail.Waypoints.Count - 1].Position.y);
                    if (railDisplacementX != 0 || railDisplacementY != 0)
                    {
                        if (railDisplacementX / SceneView.lastActiveSceneView.camera.aspect > railDisplacementY)
                            sceneViewHeight = railDisplacementX / SceneView.lastActiveSceneView.camera.aspect;
                        else
                            sceneViewHeight = railDisplacementY;

                        if (sceneViewHeight != 0)
                            SceneView.lastActiveSceneView.size = sceneViewHeight * sceneViewPadding;
                    }
                    SceneView.lastActiveSceneView.pivot = (_rail.Waypoints[0].Position + _rail.Waypoints[_rail.Waypoints.Count - 1].Position) * 0.5f;
                }
            }

            if (GUILayout.Button(new GUIContent("Delete", "Remove this rail's game object from the scene"), GUILayout.Height(24)))
            {
                DestroyRail(_rail);
            }
        }

        private void RenderViewSingleToggle()
        {
            var newViewSingle = EditorGUILayout.Toggle(new GUIContent("View Single", ""), _railManager.ViewSingle);
            if (_railManager.ViewSingle != newViewSingle)
            {
                _railManager.ViewSingle = newViewSingle;
                _shouldRepaintScene = true;
            }
        }

        private void RenderActiveToggle()
        {
            var newActive = EditorGUILayout.Toggle(new GUIContent("Active", ""), _rail.Active);
            if (_rail.Active != newActive)
            {
                Undo.RecordObject(_rail, "Change Rail Active");
                _rail.Active = newActive;
                _shouldRepaintScene = true;
            }
        }

        private void RenderTabSwitcher()
        {
            var oldGUIColor = GUI.backgroundColor;
            var activeColor = new Color32(154, 181, 217, 255);

            if (_currentTabViewType == TabViewType.Waypoints)
            {
                GUI.backgroundColor = activeColor;
            }

            if (GUILayout.Button(new GUIContent("Waypoints", "Show waypoints editor"), EditorStyles.miniButtonLeft, GUILayout.Height(18)))
            {
                _currentTabViewType = TabViewType.Waypoints;
            }

            if (_currentTabViewType == TabViewType.Effects)
            {
                GUI.backgroundColor = activeColor;
            }
            else
            {
                GUI.backgroundColor = oldGUIColor;
            }

            if (GUILayout.Button(new GUIContent("Effects", "Show effects editor"), EditorStyles.miniButtonRight, GUILayout.Height(18)))
            {
                _currentTabViewType = TabViewType.Effects;
            }

            GUI.backgroundColor = oldGUIColor;
        }

        private void RenderWaypointsTab()
        {
            if (_rail == null)
            {
                return;
            }

            if (_waypointsList != null)
            {
                _waypointsList.elementHeight = _rail.Waypoints.Count < 1 ? EditorGUIUtility.singleLineHeight : (EditorGUIUtility.singleLineHeight + 4) * 4 + 8;
                _waypointsList.DoLayoutList();
            }
        }

        private void RenderEffectsTab()
        {
            if (_rail == null)
            {
                return;
            }

            if (_effectsList != null)
            {
                _effectsList.elementHeight = _rail.Effects.Count < 1 ? EditorGUIUtility.singleLineHeight : (EditorGUIUtility.singleLineHeight + 4) * 4 + 8;
                _effectsList.DoLayoutList();
            }
        }

        private void CreateRail(int newIndex)
        {
            var obj = new GameObject();
            obj.name = "Rail " + newIndex.ToString();

            var rail = obj.AddComponent<Rail>();
            Vector2 waypoint1Pos = GetRoundedScenePivot();

            AddWaypoint(rail, 0, waypoint1Pos);
            AddWaypoint(rail, 1, waypoint1Pos + new Vector2(10, 0));

            Undo.RegisterCreatedObjectUndo(obj, "Add Rail Object");

            SetCurrentRail(rail);
        }

        private void DestroyRail(Rail rail)
        {
            if (rail.Waypoints.Count < 3 || EditorUtility.DisplayDialog("Remove Rail?", "Do you really want to remove " + rail.gameObject.name + " from the scene?", "Remove", "Cancel"))
            {
                var railIndex = Array.IndexOf(_rails, rail);
                // SetCurrentRail(_rails.Length == 1 ? null : railIndex == 0 ? _rails[Array.IndexOf(_rails, rail) + 1] : _rails[Array.IndexOf(_rails, rail) - 1]);
                SetCurrentRail(null);
                _railObjectToDestroy = rail.gameObject;
                EditorApplication.delayCall += _callbackRemoveObject;
                GUIUtility.ExitGUI();
            }
        }

        private static void DestroyImmediate()
        {
            if (_railObjectToDestroy != null)
            {
                Undo.DestroyObjectImmediate(_railObjectToDestroy);
            }
            EditorApplication.delayCall -= _callbackRemoveObject;
        }

        private void SetCurrentRail(Rail rail)
        {
            if (_rail != rail)
            {
                _rail = rail;
                _waypoints = rail == null ? new List<Waypoint>() : rail.Waypoints;
                _effects = rail == null ? new List<Effect>() : rail.Effects;
            }
        }

        private void AddWaypoint(Rail rail, int index, Vector2 position)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(rail, "Insert rail waypoint");

            var waypoints = rail.Waypoints;
            var defaultFollowAxis = (FollowAxis)_settings.FindProperty("rail_waypoint_default_follow_axis").enumValueIndex;
            var shouldInheritFollowAxis = _settings.FindProperty("on_create_rail_waypoint_inherit_follow_axis").boolValue;
            var defaultCurveType = (CurveType)_settings.FindProperty("rail_waypoint_default_curve_type").enumValueIndex;
            var shouldInheritCurveType = _settings.FindProperty("on_create_rail_waypoint_inherit_curve_type").boolValue;

            var newWaypoint = new Waypoint()
            {
                Position = position
            };

            if (waypoints.Count > 0 && shouldInheritFollowAxis)
            {
                newWaypoint.FollowAxis = index == 0 ? waypoints[0].FollowAxis : waypoints[index - 1].FollowAxis;
            }
            else
            {
                newWaypoint.FollowAxis = defaultFollowAxis;
            }

            if (waypoints.Count > 0 && shouldInheritCurveType)
            {
                newWaypoint.CurveType = index == 0 ? waypoints[0].CurveType : waypoints[index - 1].CurveType;
            }
            else
            {
                newWaypoint.CurveType = defaultCurveType;
            }

            if (newWaypoint.CurveType == CurveType.Quadratic)
            {
                if (index == 0)
                {
                    newWaypoint.CurveControlPoint = waypoints.Count > 0
                        ? GetInitialControlPointPosition(newWaypoint.Position, waypoints[0].Position)
                        : Vector2.zero;
                }
                else if (index < waypoints.Count)
                {
                    newWaypoint.CurveControlPoint = GetInitialControlPointPosition(newWaypoint.Position, waypoints[index].Position);
                }
                else
                {
                    waypoints[index - 1].CurveControlPoint = GetInitialControlPointPosition(waypoints[index - 1].Position, newWaypoint.Position);
                }
            }

            if (index == waypoints.Count)
            {
                waypoints.Add(newWaypoint);
            }
            else
            {
                waypoints.Insert(index, newWaypoint);
            }

            foreach (var e in rail.Effects)
            {
                if (e.WaypointIndex >= index)
                {
                    e.WaypointIndex += 1;
                }
            }

            _shouldRepaintScene = true;
        }

        private Vector2 GetInitialControlPointPosition(Vector2 a0, Vector2 a1)
        {
            var length = a1 - a0;
            var midpoint = a0 + (length / 2);
            return midpoint + ((a0 + (a1 - (a0 + Vector2.Perpendicular(length))) / 2) - midpoint) / 2;
        }

        private void RemoveWaypoint(Rail rail, int index)
        {
            SetCurrentRail(rail);
            if (rail.Waypoints.Count > 2)
            {
                Undo.RecordObject(rail, "Delete rail waypoint");
                rail.Waypoints.RemoveAt(index);

                foreach (var e in rail.Effects)
                {
                    if (e.WaypointIndex == index)
                    {
                        e.WaypointIndex = -1;
                    }
                    else if (e.WaypointIndex > index)
                    {
                        e.WaypointIndex -= 1;
                    }
                }

                _shouldRepaintScene = true;
            }
        }

        private void SetWaypointOrder(Rail rail, int oldIndex, int newIndex)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(_rail, "Set waypoint order");
            var waypoint = rail.Waypoints[oldIndex];
            rail.Waypoints.RemoveAt(oldIndex);
            rail.Waypoints.Insert(newIndex, waypoint);

            foreach (var e in rail.Effects)
            {
                if (e.WaypointIndex == oldIndex)
                {
                    e.WaypointIndex = newIndex;
                }
                else if (e.WaypointIndex < oldIndex && e.WaypointIndex >= newIndex)
                {
                    e.WaypointIndex += 1;
                }
                else if (e.WaypointIndex > oldIndex && e.WaypointIndex <= newIndex)
                {
                    e.WaypointIndex -= 1;
                }
            }

            _shouldRepaintScene = true;
        }

        private void SetWaypointFollowAxis(Rail rail, Waypoint waypoint, FollowAxis followAxis)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(rail, "Set waypoint follow axis");
            waypoint.FollowAxis = followAxis;
            _shouldRepaintScene = true;
        }

        private void SetWaypointPosition(Rail rail, Waypoint waypoint, Vector2 position)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(rail, "Set waypoint position");
            waypoint.Position = position;
            _shouldRepaintScene = true;
        }

        private void SetWaypointCurveType(Rail rail, Waypoint waypoint, CurveType curveType)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(rail, "Set waypoint curve type");

            var index = rail.Waypoints.IndexOf(waypoint);
            if (curveType == CurveType.Quadratic && waypoint.CurveType == CurveType.Linear && index < rail.Waypoints.Count - 1)
            {
                waypoint.CurveControlPoint = GetInitialControlPointPosition(waypoint.Position, rail.Waypoints[index + 1].Position);
            }

            waypoint.CurveType = curveType;
            _shouldRepaintScene = true;
        }

        private void SetWaypointCurveControlPoint(Rail rail, Waypoint waypoint, Vector2 curveControlPoint)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(rail, "Set waypoint curve control point");
            waypoint.CurveControlPoint = curveControlPoint;
            _shouldRepaintScene = true;
        }

        private void AddEffect(Rail rail)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(rail, "Add effect");
            rail.Effects.Add(new Effect());
        }

        private void RemoveEffect(Rail rail, int index)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(rail, "Remove effect");
            rail.Effects.RemoveAt(index);
            _shouldRepaintScene = true;
        }

        private void SetEffectOrder(Rail rail, int oldIndex, int newIndex)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(_rail, "Set effect order");
            var effect = rail.Effects[oldIndex];
            rail.Effects.RemoveAt(oldIndex);
            rail.Effects.Insert(newIndex, effect);
        }

        private void SetEffectActive(Rail rail, Effect effect, bool active)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(rail, "Change effect active");
            effect.Active = active;
            _shouldRepaintScene = true;
        }

        private void SetEffectWaypoint(Rail rail, Effect effect, int nodeIndex)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(_rail, "Change effect waypoint");
            effect.WaypointIndex = nodeIndex;
            _shouldRepaintScene = true;
        }

        private void SetEffectCameraInterpolation(Rail rail, Effect effect, float interpolation)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(_rail, "Change effect camera interpolation");
            effect.CameraInterpolation = interpolation;
            _shouldRepaintScene = true;
        }

        private void SetEffectTargetInterpolation(Rail rail, Effect effect, float interpolation)
        {
            SetCurrentRail(rail);
            Undo.RecordObject(_rail, "Change effect target interpolation");
            effect.TargetInterpolation = interpolation;
            _shouldRepaintScene = true;
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

        private static Vector3 GetRoundedScenePivot()
        {
            var pivot = Vector3.zero;

            if (SceneView.lastActiveSceneView != null)
            {
                pivot = SceneView.lastActiveSceneView.pivot;
            }

            var roundX = Mathf.Round(pivot.x);
            var roundY = Mathf.Round(pivot.y);
            var roundZ = Mathf.Round(pivot.z);
            return new Vector3(roundX, roundY, roundZ);
        }
    }
}

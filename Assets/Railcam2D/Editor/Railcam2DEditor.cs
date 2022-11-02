using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Railcam2D
{
    [CustomEditor(typeof(Railcam2D))]
    public class Railcam2DEditor : Editor
    {
        private Rail[] _rails;
        private int _currentRailDropDownIndex = 0;
        private SerializedObject _settings;
        private ReorderableList _cameraTargetsReorderableList;
        private Railcam2D _railcam2d;

        public void OnEnable()
        {
            _railcam2d = (Railcam2D)target;
            if (_railcam2d == null)
            {
                return;
            }

            _settings = Railcam2DSettings.GetSerializedSettings();
            _rails = GetRailsOrderedByName();

            _cameraTargetsReorderableList = new ReorderableList(_railcam2d.Targets, typeof(CameraTarget))
            {
                headerHeight = 0,
                onAddCallback = (list) =>
                {
                    _railcam2d.Targets.Add(new CameraTarget());
                },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var target = (CameraTarget)_railcam2d.Targets[index];

                    rect.y += 4;

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, 80, 10), new GUIContent("Transform", "The target transform"));
                    var newTransform = (Transform)EditorGUI.ObjectField(new Rect(
                            rect.x + 80,
                            rect.y,
                            rect.width - 80,
                            EditorGUIUtility.singleLineHeight),
                        target.Transform, typeof(Transform), true);
                    if (target.Transform != newTransform)
                    {
                        Undo.RecordObject(_railcam2d, "Change target transform");
                        target.Transform = newTransform;
                    }

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 4, 80, 10), new GUIContent("Influence X", "The target transform"));
                    var newInfluenceX = EditorGUI.Slider(new Rect(
                            rect.x + 80,
                            rect.y + EditorGUIUtility.singleLineHeight + 4,
                            rect.width - 80,
                            EditorGUIUtility.singleLineHeight),
                        target.InfluenceX, 0, 1);
                    if (target.InfluenceX != newInfluenceX)
                    {
                        Undo.RecordObject(_railcam2d, "Change target influence x");
                        target.InfluenceX = newInfluenceX;
                    }

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + (EditorGUIUtility.singleLineHeight + 4) * 2, 80, 10), new GUIContent("Influence Y", "The target transform"));
                    var newInfluenceY = EditorGUI.Slider(new Rect(
                            rect.x + 80,
                            rect.y + (EditorGUIUtility.singleLineHeight + 4) * 2,
                            rect.width - 80,
                            EditorGUIUtility.singleLineHeight),
                        target.InfluenceY, 0, 1);
                    if (target.InfluenceY != newInfluenceY)
                    {
                        Undo.RecordObject(_railcam2d, "Change target influence y");
                        target.InfluenceY = newInfluenceY;
                    }
                }
            };
        }

        public void OnSceneGUI()
        {
            if (_railcam2d == null)
            {
                return;
            }

            var handleSize = HandleUtility.GetHandleSize(Vector3.zero) / 5;

            if (_railcam2d.GetComponent<RailManager>() == null)
            {
                _rails = GetRailsOrderedByName();

                for (var i = 0; i < _rails.Length; ++i)
                {
                    if (_rails[i].Waypoints.Count > 1)
                    {
                        RenderRailPath(_rails[i]);
                    }
                }
            }
        }

        private Rail[] GetRailsOrderedByName()
        {
            return Object.FindObjectsOfType<Rail>().OrderBy(r => r.gameObject.name).ToArray();
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

                Handles.color = waypoint.FollowAxis == FollowAxis.XY ? xyColor : waypoint.FollowAxis == FollowAxis.X ? xColor : yColor;
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

        public override void OnInspectorGUI()
        {
            if (_railcam2d == null)
            {
                return;
            }

            serializedObject.Update();

            _rails = GetRailsOrderedByName();

            RenderMainEditor();

            serializedObject.ApplyModifiedProperties();
        }

        private void RenderMainEditor()
        {
            EditorGUILayout.Space();

            if (_cameraTargetsReorderableList != null)
            {
                _cameraTargetsReorderableList.elementHeight = _railcam2d.Targets.Count < 1 ? EditorGUIUtility.singleLineHeight : (EditorGUIUtility.singleLineHeight + 4) * 3 + 8;
                EditorGUILayout.LabelField(new GUIContent("Targets", "Object transforms the camera will follow"));
                _cameraTargetsReorderableList.DoLayoutList();
            }

            EditorGUILayout.Space();

            var newActive = EditorGUILayout.Toggle(new GUIContent("Active", "Determines whether or not the Railcam 2D component controls camera movement"), _railcam2d.Active);
            if (_railcam2d.Active != newActive)
            {
                Undo.RecordObject(_railcam2d, "Change Active");
                _railcam2d.Active = newActive;
            }

            EditorGUILayout.Space();

            var newUpdateMethod = EditorGUILayout.EnumPopup(new GUIContent("Update Method", "The Update method used to move the camera"), _railcam2d.UpdateMethod);
            if (_railcam2d.UpdateMethod != (UpdateMethod)newUpdateMethod)
            {
                Undo.RecordObject(_railcam2d, "Change Update Method");
                _railcam2d.UpdateMethod = (UpdateMethod)newUpdateMethod;
            }

            EditorGUILayout.Space();

            var newOffset = EditorGUILayout.Vector2Field(new GUIContent("Offset", ""), _railcam2d.Offset, GUILayout.ExpandWidth(true));
            if (_railcam2d.Offset != newOffset)
            {
                Undo.RecordObject(_railcam2d, "Change Offset");
                _railcam2d.Offset = newOffset;
            }

            EditorGUILayout.Space();

            var newSmooth = EditorGUILayout.Vector2Field(new GUIContent("Smooth", ""), _railcam2d.Smooth, GUILayout.ExpandWidth(true));
            if (_railcam2d.Smooth != newSmooth)
            {
                Undo.RecordObject(_railcam2d, "Change Smooth");
                _railcam2d.Smooth = newSmooth;
            }

            EditorGUILayout.Space();

            var newRailsConnected = EditorGUILayout.Toggle(new GUIContent("Rails Connected", ""), _railcam2d.RailsConnected);
            if (_railcam2d.RailsConnected != newRailsConnected)
            {
                Undo.RecordObject(_railcam2d, "Change Rails Connected");
                _railcam2d.RailsConnected = newRailsConnected;
            }

            EditorGUILayout.Space();

            RenderRailEditingButton();

            EditorGUILayout.Space();
        }

        private void RenderRailEditingButton()
        {

            var oldGUIColor = GUI.backgroundColor;

            if (_railcam2d.GetComponent(typeof(RailManager)) == null)
            {
                if (GUILayout.Button("Enable Rail Editing", GUILayout.Height(32)))
                    Undo.AddComponent<RailManager>(_railcam2d.gameObject);
            }
            else
            {
                GUI.backgroundColor = new Color32(154, 181, 217, 255);
                if (GUILayout.Button("Disable Rail Editing", GUILayout.Height(32)))
                {
                    Undo.DestroyObjectImmediate(_railcam2d.GetComponent(typeof(RailManager)));
                    GUIUtility.ExitGUI();
                }
            }

            GUI.backgroundColor = oldGUIColor;
        }
    }
}

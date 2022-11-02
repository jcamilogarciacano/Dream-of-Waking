using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Railcam2D
{
    public class Railcam2DSettings : ScriptableObject
    {
        public const string railcam2DSettingsPath = "Assets/Railcam2D/Editor/Railcam2DSettings.asset";

        [SerializeField]
        private float rail_width;
        [SerializeField]
        private float rail_waypoint_handle_size;
        [SerializeField]
        private float rail_waypoint_create_button_size;
        [SerializeField]
        private float control_point_line_width;
        [SerializeField]
        private float control_point_handle_size;
        [SerializeField]
        private Color rail_waypoint_follow_xy_color;
        [SerializeField]
        private Color rail_waypoint_follow_x_color;
        [SerializeField]
        private Color rail_waypoint_follow_y_color;
        [SerializeField]
        private Color rail_inactive_color;
        [SerializeField]
        private Color rail_waypoint_create_button_color;
        [SerializeField]
        private Color rail_waypoint_handle_color;
        [SerializeField]
        private Color rail_waypoint_delete_button_color;
        [SerializeField]
        private Color control_point_handle_color;
        [SerializeField]
        private Color control_point_line_1_color;
        [SerializeField]
        private Color control_point_line_2_color;
        [SerializeField]
        private Color rail_name_text_color;
        [SerializeField]
        private int rail_name_text_size;
        [SerializeField]
        private Color rail_name_text_background_color;
        [SerializeField]
        private Color rail_waypoint_index_text_color;
        [SerializeField]
        private int rail_waypoint_index_text_size;
        [SerializeField]
        private Color rail_waypoint_index_text_background_color;
        [SerializeField]
        private Vector2 grid_snap_size;
        [SerializeField]
        private CurveType rail_waypoint_default_curve_type;
        [SerializeField]
        private FollowAxis rail_waypoint_default_follow_axis;
        [SerializeField]
        private bool on_create_rail_waypoint_inherit_curve_type;
        [SerializeField]
        private bool on_create_rail_waypoint_inherit_follow_axis;
        [SerializeField]
        private bool is_antialiasing_enabled;
        [SerializeField]
        private Color rail_effect_target_point_color;

        public static Color rail_effect_target_point_color_default = new Color(1, 1, 1);

        public static void SetDefaultValues(Railcam2DSettings settings)
        {
            if (settings == null)
            {
                return;
            }

            settings.rail_width = 6;
            settings.rail_waypoint_handle_size = 0.5f;
            settings.rail_waypoint_create_button_size = 0.25f;
            settings.control_point_line_width = 3;
            settings.control_point_handle_size = 0.5f;
            settings.rail_waypoint_follow_xy_color = new Color(0.1215686f, 1, 0.1654902f);
            settings.rail_waypoint_follow_x_color = new Color(1, 0.8235294f, 0.1176471f);
            settings.rail_waypoint_follow_y_color = new Color(0.1176471f, 0.7333333f, 1);
            settings.rail_waypoint_handle_color = new Color(1, 1, 1);
            settings.rail_waypoint_create_button_color = new Color(1, 1, 1);
            settings.rail_waypoint_delete_button_color = new Color(1, 0, 0);
            settings.control_point_handle_color = new Color(1, 1, 1);
            settings.control_point_line_1_color = new Color(1, 1, 1, 0.75f);
            settings.control_point_line_2_color = new Color(1, 1, 1, 0.75f);
            settings.rail_name_text_color = new Color(1, 1, 1);
            settings.rail_name_text_size = 11;
            settings.rail_name_text_background_color = new Color(0, 0, 0, 0.5f);
            settings.rail_waypoint_index_text_color = new Color(1, 1, 1);
            settings.rail_waypoint_index_text_size = 11;
            settings.rail_waypoint_index_text_background_color = new Color(0, 0, 0, 0);
            settings.grid_snap_size = new Vector2(1, 1);
            settings.rail_waypoint_default_curve_type = CurveType.Linear;
            settings.rail_waypoint_default_follow_axis = FollowAxis.XY;
            settings.on_create_rail_waypoint_inherit_curve_type = true;
            settings.on_create_rail_waypoint_inherit_follow_axis = true;
            settings.is_antialiasing_enabled = true;
            settings.rail_effect_target_point_color = new Color(1, 1, 1);
            settings.rail_inactive_color = new Color(0.5f, 0.5f, 0.5f);
        }

        internal static Railcam2DSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<Railcam2DSettings>(railcam2DSettingsPath);

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<Railcam2DSettings>();
                SetDefaultValues(settings);
                AssetDatabase.CreateAsset(settings, railcam2DSettingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    static class Railcam2DSettingsIMGUIRegister
    {
        [SettingsProvider]
        public static SettingsProvider Railcam2DSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Railcam2D", SettingsScope.Project)
            {
                label = "Railcam 2D",

                guiHandler = (searchContext) =>
                {
                    var settings = Railcam2DSettings.GetSerializedSettings();
                    settings.Update();

                    var headingStyle = new GUIStyle("Label")
                    {
                        fontSize = 12,
                        fontStyle = FontStyle.Bold
                    };
                    var subheadingStyle = new GUIStyle("Label")
                    {
                        fontSize = 11,
                        fontStyle = FontStyle.Bold
                    };

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("General", headingStyle);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Default Curve Type"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_default_curve_type"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Default Follow Axis"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_default_follow_axis"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Inherit Curve Type"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("on_create_rail_waypoint_inherit_curve_type"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Inherit Follow Axis"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("on_create_rail_waypoint_inherit_follow_axis"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Size", headingStyle);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Rail"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_width"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Waypoint Handle"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_handle_size"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Waypoint Create Button"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_create_button_size"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Control Point Line"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("control_point_line_width"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Control Point Handle"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("control_point_handle_size"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Name Text"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_name_text_size"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Waypoint Text"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_index_text_size"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Grid Snap"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("grid_snap_size"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Color", headingStyle);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Antialiasing"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("is_antialiasing_enabled"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Follow XY"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_follow_xy_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Follow X"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_follow_x_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Follow Y"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_follow_y_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Inactive"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_inactive_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Waypoint Handle"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_handle_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Waypoint Create Button"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_create_button_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Waypoint Delete Button"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_delete_button_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Control Point Handle"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("control_point_handle_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Control Point Line 1"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("control_point_line_1_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Control Point Line 2"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("control_point_line_2_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Name Text"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_name_text_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Name Background"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_name_text_background_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Waypoint Text"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_index_text_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Waypoint Text Background"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_waypoint_index_text_background_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Effect Target"), GUILayout.Width(160));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(settings.FindProperty("rail_effect_target_point_color"), GUIContent.none);
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (GUILayout.Button("Use Defaults", GUILayout.ExpandWidth(false)))
                    {
                        var settingsObj = (Railcam2DSettings)settings.targetObject;
                        Railcam2DSettings.SetDefaultValues(settingsObj);
                        var newSettings = new SerializedObject(settingsObj);
                        var prop = newSettings.GetIterator();

                        prop.Next(true);

                        do
                        {
                            settings.CopyFromSerializedProperty(prop);
                        }
                        while (prop.Next(false));
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();

                    settings.ApplyModifiedProperties();
                },

                keywords = new HashSet<string>(new[] { "rail", "railcam", "railcam2d", "2d" })
            };

            return provider;
        }
    }
}

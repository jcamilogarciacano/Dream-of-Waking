using UnityEditor;

namespace Railcam2D
{
    [CustomEditor(typeof(Rail))]
    public class RailEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Use a Rail Manager to edit this rail.", MessageType.Info);
        }
    }
}

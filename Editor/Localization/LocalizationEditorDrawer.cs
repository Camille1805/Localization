using UnityEditor;
using UnityEngine;


namespace Localization
{
    [CustomEditor(typeof(LocalizationConfigEditor), true)]
    public class LocalizationEditorDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            var list = serializedObject.FindProperty("config");
            EditorGUILayout.PropertyField(list, new GUIContent("My List Test"), true);
        }
    }
}

using UnityEditor;

namespace Playrika.GameFoundation.UI.Components.Editor
{
    [CustomEditor(typeof(GridSizeFitter))]
    public class GridSizeFitterEditor : UnityEditor.Editor
    {
        private SerializedProperty _fitOnAwake;

        private SerializedProperty _forcedSquare;

        private SerializedProperty _fitHorizontal;

        private SerializedProperty _horizontalCellsCount;

        private SerializedProperty _fitVertical;

        private SerializedProperty _verticalCellsCount;

        private void OnEnable()
        {
            _fitOnAwake = serializedObject.FindProperty("_fitOnAwake");
            _forcedSquare = serializedObject.FindProperty("_forcedSquare");
            _fitHorizontal = serializedObject.FindProperty("_fitHorizontal");
            _horizontalCellsCount = serializedObject.FindProperty("_horizontalCellsCount");
            _fitVertical = serializedObject.FindProperty("_fitVertical");
            _verticalCellsCount = serializedObject.FindProperty("_verticalCellsCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_fitOnAwake);

            if (_forcedSquare.boolValue && _fitHorizontal.boolValue && _fitVertical.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(
                    "You cannot use \"Forced Square\" & \"Fit Horizontal\" & \"Fit Vertical\" at the same time.",
                    MessageType.Error);
            }

            EditorGUILayout.PropertyField(_forcedSquare);

            EditorGUILayout.PropertyField(_fitHorizontal);
            if (_fitHorizontal.boolValue)
                EditorGUILayout.PropertyField(_horizontalCellsCount);

            EditorGUILayout.PropertyField(_fitVertical);
            if (_fitVertical.boolValue)
                EditorGUILayout.PropertyField(_verticalCellsCount);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
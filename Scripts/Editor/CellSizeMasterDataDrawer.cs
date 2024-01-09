using UnityEditor;
using UnityEngine;

namespace Unity1week202312.Editor
{
    public class CellSizeMasterDataDrawer
    {
        private readonly SerializedObject _serializedObject;

        public CellSizeMasterDataDrawer(SerializedObject serializedObject)
        {
            _serializedObject = serializedObject;
        }

        public void OnGUI()
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                var sizeProperty = _serializedObject.FindProperty("_size");
                var toSize = EditorGUILayout.Vector2IntField("Size", sizeProperty.vector2IntValue);
                sizeProperty.vector2IntValue = new Vector2Int(
                    Mathf.Max(toSize.x, 1),
                    Mathf.Max(toSize.y, 1)
                );
                Vector2Int size = sizeProperty.vector2IntValue;
                _serializedObject.ApplyModifiedProperties();

                var pivotProperty = _serializedObject.FindProperty("_pivot");
                var toPivot = EditorGUILayout.Vector2IntField("Pivot", pivotProperty.vector2IntValue);
                pivotProperty.vector2IntValue = new Vector2Int(
                    Mathf.Clamp(toPivot.x, 0, size.x - 1),
                    Mathf.Clamp(toPivot.y, 0, size.y - 1)
                );

                _serializedObject.ApplyModifiedProperties();

                EditorGUILayout.LabelField("CellSizeMap");
                var dataProperty = _serializedObject.FindProperty("_cellSizeMap");
                var allSize = size.x * size.y;
                if (allSize != dataProperty.arraySize)
                {
                    dataProperty.arraySize = allSize;
                }

                EditorGUILayout.BeginVertical();

                for (int y = 0; y < size.y; y++)
                {
                    EditorGUILayout.BeginHorizontal();

                    for (int x = 0; x < size.x; x++)
                    {
                        var index = y * size.x + x;
                        var valueProperty = dataProperty.GetArrayElementAtIndex(index);
                        var isPivot = pivotProperty.vector2IntValue == new Vector2Int(x, y);
                        GUI.color = isPivot ? Color.red : Color.white;
                        valueProperty.boolValue =
                            EditorGUILayout.Toggle(valueProperty.boolValue, new[] { GUILayout.Width(20f) });
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
                GUI.color = Color.white;

                if (scope.changed)
                {
                    _serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(_serializedObject.targetObject);
                }
            }
        }
    }
}
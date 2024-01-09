using UnityEditor;
using UnityEngine;

namespace Unity1week202312.Editor
{
    [CustomEditor(typeof(CellSizeMasterData))]
    public class CellSizeMasterDataEditor : UnityEditor.Editor
    {
        private CellSizeMasterData _target;
        private CellSizeMasterDataDrawer _drawer;

        private void OnEnable()
        {
            _target = target as CellSizeMasterData;
            _drawer = new CellSizeMasterDataDrawer(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            var serializedObject = new SerializedObject(_target);

            _drawer.OnGUI();

            if (GUILayout.Button("Clear"))
            {
                var dataProperty = serializedObject.FindProperty("_cellSizeMap");
                for (int i = 0; i < dataProperty.arraySize; i++)
                {
                    dataProperty.GetArrayElementAtIndex(i).boolValue = false;
                }

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(_target);
            }
        }
    }
}
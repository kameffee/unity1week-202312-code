using Unity1week202312.PlaceObjects;
using UnityEditor;
using UnityEngine;

namespace Unity1week202312.Editor
{
    [CustomEditor(typeof(PlaceObjectMasterData))]
    [CanEditMultipleObjects]
    public class PlaceObjectMasterDataEditor : UnityEditor.Editor
    {
        private SerializedProperty _prefabProperty;

        private void OnEnable()
        {
            _prefabProperty = serializedObject.FindProperty("_prefab");
        }

        public override void OnInspectorGUI()
        {
            // 元のInspector部分を表示
            base.OnInspectorGUI();

            serializedObject.Update();

            // Prefabのプレビューを表示
            var prefab = _prefabProperty.objectReferenceValue as GameObject;

            if (prefab != null)
            {
                var previewTexture = AssetPreview.GetAssetPreview(prefab);
                if (previewTexture != null)
                {
                    GUILayout.Space(5f);
                    GUILayout.Label(previewTexture, GUILayout.MaxHeight(200f));
                }

                if (prefab.TryGetComponent<PlaceObjectView>(out var placeObjectView))
                {
                    // 無効にする
                    using (new EditorGUI.DisabledScope(true))
                    {
                        var placeObject = new SerializedObject(placeObjectView);
                        var cellSizeMasterData = placeObject.FindProperty("_cellSizeMasterData");
                        EditorGUILayout.PropertyField(cellSizeMasterData);

                        var cellSizeMasterDataProperty = new CellSizeMasterDataDrawer(
                            new SerializedObject(cellSizeMasterData.objectReferenceValue));
                        cellSizeMasterDataProperty.OnGUI();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
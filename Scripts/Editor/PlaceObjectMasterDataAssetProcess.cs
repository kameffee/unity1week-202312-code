using System.Linq;
using NUnit.Framework;
using Unity1week202312.PlaceObjects;
using UnityEditor;

namespace Unity1week202312.Editor
{
    public class PlaceObjectMasterDataAssetProcess
    {
        private const string FolderPath = "Assets/Application/ScriptableObjects/PlaceObjectMasterData/";
        private const string DataStorePath = "Assets/Application/ScriptableObjects/PlaceObjectMasterDataStoreSource.asset";
        
        public void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var dataStore = AssetDatabase.LoadAssetAtPath<PlaceObjectMasterDataStoreSource>(DataStorePath);
            Assert.IsNotNull(dataStore);

            var targetImportedAssets = importedAssets
                .Where(path => !path.Contains(DataStorePath))
                .Where(path => path.StartsWith(FolderPath))
                .ToArray();

            // 新規で追加されたアセットをデータストアに追加する
            foreach (var importedAsset in targetImportedAssets)
            {
                var masterData = AssetDatabase.LoadAssetAtPath<PlaceObjectMasterData>(importedAsset);
                Assert.IsNotNull(masterData);
                dataStore.Add(masterData);
            }

            var anyDeletedAssets = deletedAssets
                .Where(path => !path.Contains(DataStorePath))
                .Any(path => path.StartsWith(FolderPath));

            if (targetImportedAssets.Any() || anyDeletedAssets)
            {
                dataStore.Validate();
                EditorUtility.SetDirty(dataStore);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
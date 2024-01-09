using UnityEditor;

namespace Unity1week202312.Editor
{
    public class MasterDataAssetProcessor : AssetPostprocessor
    {
        private static readonly PlaceObjectMasterDataAssetProcess _placeObjectMasterDataAssetProcess = new();

        public static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            _placeObjectMasterDataAssetProcess.OnPostprocessAllAssets(
                importedAssets,
                deletedAssets,
                movedAssets,
                movedFromAssetPaths);
        }
    }
}
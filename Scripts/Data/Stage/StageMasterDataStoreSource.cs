using UnityEngine;

namespace Unity1week202312.Stage
{
    [CreateAssetMenu(fileName = "StageMasterDataStoreSource", menuName = "StageMasterDataStoreSource", order = 0)]
    public class StageMasterDataStoreSource : ScriptableObject
    {
        public StageMasterData[] Datas => _stageMasterData;

        [SerializeField]
        private StageMasterData[] _stageMasterData;
    }
}
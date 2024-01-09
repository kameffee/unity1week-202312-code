using System.Collections.Generic;
using System.Linq;
using Unity1week202312.PlaceObjects;
using UnityEngine;

namespace Unity1week202312.Stage
{
    [CreateAssetMenu(fileName = "StageMasterData_", menuName = "StageMasterData", order = 0)]
    public class StageMasterData : ScriptableObject
    {
        public StageId StageId => new(_stageId);

        [SerializeField]
        private int _stageId;

        [SerializeField]
        private List<PlaceObjectMasterData> _lotteryPlaceObjectMasterDataList;

        // 条件付きで配置されるオブジェクト
        [SerializeField]
        private List<ConditionalPlaceObject> _conditionalPlaceObjectMasterDataList;

        public IEnumerable<IConditionalPlaceObject> GetConditionalPlaceObjects()
        {
            return _lotteryPlaceObjectMasterDataList.Concat<IConditionalPlaceObject>(_conditionalPlaceObjectMasterDataList);
        }
    }
}
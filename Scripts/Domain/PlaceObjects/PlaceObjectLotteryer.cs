using System.Collections.Generic;
using System.Linq;
using Kameffee.Extensions;
using Unity1week202312.PlaceBoard.History;
using Unity1week202312.Stage;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceObjectLotteryer
    {
        private readonly StageMasterDataRepository _stageMasterDataRepository;
        private readonly PlaceBoardHistory _placeBoardHistory;
        private readonly PlaceObjectMasterDataRepository _placeObjectMasterDataRepository;

        public PlaceObjectLotteryer(
            StageMasterDataRepository stageMasterDataRepository,
            PlaceBoardHistory placeBoardHistory,
            PlaceObjectMasterDataRepository placeObjectMasterDataRepository)
        {
            _stageMasterDataRepository = stageMasterDataRepository;
            _placeBoardHistory = placeBoardHistory;
            _placeObjectMasterDataRepository = placeObjectMasterDataRepository;
        }

        public bool TryLottery(StageId currentStageId, IReadOnlyCollection<PlaceObjectId> ignoreIds, out PlaceObjectId placeObjectId)
        {
            var stageMasterData = _stageMasterDataRepository.Get(currentStageId);
            var targetIds = stageMasterData.GetConditionalPlaceObjects()
                .Where(conditionalPlaceObject => ConditionCheck(currentStageId, conditionalPlaceObject))
                .Select(data => data.Id)
                .ToList();

            if (_placeBoardHistory.Any() && !currentStageId.IsFirst())
            {
                var previousStageId = currentStageId.Previous();
                var previousRecord = _placeBoardHistory.GetRecord(previousStageId);
                // 前回配置したものを抽選対象にする.
                var additionalAgedPlaceObjectIds = previousRecord.PackedPlaceObjects
                    .Select(packedPlaceObject => TryGetAgedPlaceObjectId(packedPlaceObject.Id, out var result)
                        ? result
                        : packedPlaceObject.Id)
                    .ToArray();

                targetIds.AddRange(additionalAgedPlaceObjectIds);
            }

            var objectIds = targetIds
                .Where(id => !ignoreIds.Contains(id))
                .Shuffle()
                .ToArray();

            if (!objectIds.Any())
            {
                placeObjectId = default;
                return false;
            }

            placeObjectId = objectIds.First();
            return true;
        }

        private bool ConditionCheck(StageId currentStageId, IConditionalPlaceObject conditionalPlaceObject)
        {
            if (currentStageId.IsFirst())
                return true;

            if (!conditionalPlaceObject.ConditionalTargets.Any())
                return true;

            // 前のステージのID
            var previousStageId = currentStageId.Previous();

            // 前のステージに条件となるPlaceObjectを配置したかどうかをチェックする
            return conditionalPlaceObject.ConditionalTargets
                .Select(data => data.Id)
                .All(id => _placeBoardHistory.Exists(previousStageId, id));
        }

        /// <summary>
        /// 経年劣化するものは、経年劣化後のものを返す.
        /// </summary>
        private bool TryGetAgedPlaceObjectId(PlaceObjectId placeObjectId, out PlaceObjectId resultId)
        {
            var agedPlaceObjectId = placeObjectId.AddAge(1);
            if (_placeObjectMasterDataRepository.TryGet(agedPlaceObjectId, out var placeObjectMasterData))
            {
                resultId = placeObjectMasterData.Id;
                return true;
            }

            resultId = placeObjectId;
            return false;
        }
    }
}
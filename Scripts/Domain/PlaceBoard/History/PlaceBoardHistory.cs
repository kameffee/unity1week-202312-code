using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity1week202312.PlaceObjects;
using Unity1week202312.Stage;
using Debug = UnityEngine.Debug;

namespace Unity1week202312.PlaceBoard.History
{
    public class PlaceBoardHistory
    {
        private readonly Dictionary<StageId, PlaceBoardRecord> _placeBoardRecords = new();

        public void Record(PlaceBoardRecord placeBoardRecord)
        {
            _placeBoardRecords.Add(placeBoardRecord.StageId, placeBoardRecord);

            // ログを出力
            Log(placeBoardRecord);
        }

        public bool Exists(StageId stageId, PlaceObjectId placeObjectId)
        {
            if (_placeBoardRecords.TryGetValue(stageId, out var placeBoardRecord))
            {
                return placeBoardRecord.Exists(placeObjectId);
            }

            return false;
        }

        public bool Any() => _placeBoardRecords.Any();

        public PlaceBoardRecord GetLastRecord() => _placeBoardRecords.Last().Value;

        public PlaceBoardRecord GetRecord(StageId stageId) => _placeBoardRecords[stageId];

        [Conditional("UNITY_EDITOR")]
        private void Log(PlaceBoardRecord placeBoardRecord)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("[PlaceBoardHistory.Log]");
            sb.AppendLine($"StageId: {placeBoardRecord.StageId}");
            sb.AppendLine($"PlaceableMap:\n{placeBoardRecord.PlaceableMap}");
            sb.AppendLine("PackedPlaceObjects:");
            foreach (var packedPlaceObject in placeBoardRecord.PackedPlaceObjects)
            {
                sb.AppendLine($"  ID:{packedPlaceObject.Id}, position:{packedPlaceObject.Position}");
            }

            sb.AppendLine("count: " + placeBoardRecord.PackedPlaceObjects.Length);
            Debug.Log(sb.ToString());
        }
    }
}
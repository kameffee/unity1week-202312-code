using System.Linq;
using Unity1week202312.Stage;

namespace Unity1week202312.PlaceBoard.History
{
    public class PlaceBoardRecordUseCase
    {
        private readonly PlaceBoardHistory _placeBoardHistory;
        private readonly PlaceBoardService _placeBoardService;

        public PlaceBoardRecordUseCase(PlaceBoardHistory placeBoardHistory, PlaceBoardService placeBoardService)
        {
            _placeBoardHistory = placeBoardHistory;
            _placeBoardService = placeBoardService;
        }

        public void Record(StageId currentStageId)
        {
            var record = new PlaceBoardRecord(
                currentStageId,
                _placeBoardService.GetMap(),
                _placeBoardService.PackedPlaceObjects.ToArray());

            _placeBoardHistory.Record(record);
        }
    }
}
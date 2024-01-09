using Unity1week202312.Stage;

namespace Unity1week202312
{
    public class StageService
    {
        public StageId CurrentStageId => _currentStageId;

        private readonly StageMasterDataRepository _stageMasterDataRepository;

        private StageId _currentStageId;

        public StageService(StageMasterDataRepository stageMasterDataRepository)
        {
            _stageMasterDataRepository = stageMasterDataRepository;
        }

        public void SetCurrentStageId(StageId stageId)
        {
            _currentStageId = stageId;
        }
    }
}
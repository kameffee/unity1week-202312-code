using System.Linq;

namespace Unity1week202312.Stage
{
    public class StageMasterDataRepository
    {
        private readonly StageMasterData[] _stageMasterData;

        public StageMasterDataRepository(StageMasterDataStoreSource storeSource)
        {
            _stageMasterData = storeSource.Datas;
        }

        public StageMasterData Get(StageId stageId)
        {
            return _stageMasterData.First(data => data.StageId == stageId);
        }

        public StageMasterData[] GetAll() => _stageMasterData;
    }
}
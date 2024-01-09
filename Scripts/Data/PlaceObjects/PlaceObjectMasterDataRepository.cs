using System.Collections.Generic;
using System.Linq;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceObjectMasterDataRepository
    {
        private readonly PlaceObjectMasterDataStoreSource _source;

        public PlaceObjectMasterDataRepository(PlaceObjectMasterDataStoreSource source)
        {
            _source = source;
        }

        public IReadOnlyList<PlaceObjectMasterData> GetAll() => _source.Data;

        public PlaceObjectMasterData Get(PlaceObjectId id)
        {
            return _source.Data.First(x => x.Id == id);
        }

        public bool TryGet(PlaceObjectId placeObjectId, out PlaceObjectMasterData masterData)
        {
            masterData = _source.Data.FirstOrDefault(data => data.Id == placeObjectId);
            return masterData != null;
        }
    }
}
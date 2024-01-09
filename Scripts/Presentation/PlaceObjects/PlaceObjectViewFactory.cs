using UnityEngine;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceObjectViewFactory
    {
        private readonly PlaceObjectMasterDataRepository _repository;
        private readonly Transform _parent;

        public PlaceObjectViewFactory(PlaceObjectMasterDataRepository repository, Transform parent)
        {
            _repository = repository;
            _parent = parent;
        }

        public PlaceObjectView Create(PlaceObjectId id)
        {
            var masterData = _repository.Get(id);
            var instance = Object.Instantiate(masterData.Prefab, _parent).GetComponent<PlaceObjectView>();
            return instance;
        }
    }
}
using UnityEngine;

namespace Unity1week202312.PlaceObjects
{
    public class DraggingPlaceObjectFactory
    {
        private readonly Transform _parent;
        private readonly PlaceObjectMasterDataRepository _placeObjectMasterDataRepository;

        public DraggingPlaceObjectFactory(
            Transform parent,
            PlaceObjectMasterDataRepository placeObjectMasterDataRepository)
        {
            _parent = parent;
            _placeObjectMasterDataRepository = placeObjectMasterDataRepository;
        }

        public PlaceObjectView Create(PlaceObjectId placeObjectId, Vector3 position)
        {
            var masterData = _placeObjectMasterDataRepository.Get(placeObjectId);
            var placeObjectView = Object.Instantiate(masterData.Prefab, position, Quaternion.identity, _parent).GetComponent<PlaceObjectView>();
            placeObjectView.ApplyViewModel(new PlaceObjectView.ViewModel(placeObjectId));
            placeObjectView.SetDefaultScale();
            return placeObjectView;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceObjectFactory
    {
        private readonly PlaceObjectMasterDataRepository _repository;
        private readonly IReadOnlyList<PlaceObjectView> _prefabs;

        public PlaceObjectFactory(PlaceObjectMasterDataRepository repository)
        {
            _repository = repository;
        }

        public PlaceObject Create(PlaceObjectId id)
        {
            Assert.IsTrue(_repository.GetAll().Any(prefab => prefab.Id == id), $"PlaceObjectViewのId={id}が見つかりませんでした。");
            var masterData = _repository.Get(id);
            return new PlaceObject(id, masterData.Prefab, masterData.Prefab.GetComponent<PlaceObjectView>().CellSizeMasterData);
        }
    }
}
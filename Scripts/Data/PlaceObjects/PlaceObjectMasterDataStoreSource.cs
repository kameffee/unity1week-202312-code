using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1week202312.PlaceObjects
{
    [CreateAssetMenu(menuName = "PlaceObjectMasterDataStoreSource", fileName = "PlaceObjectMasterDataStoreSource")]
    public class PlaceObjectMasterDataStoreSource : ScriptableObject
    {
        public IReadOnlyList<PlaceObjectMasterData> Data => _data;

        [SerializeField]
        private List<PlaceObjectMasterData> _data;

        public void Add(PlaceObjectMasterData masterData)
        {
            if (_data.Contains(masterData)) return;
            _data.Add(masterData);
        }

        public void Validate()
        {
            _data = _data.Distinct()
                .Where(data => data != null)
                .OrderBy(data => data.Id.Value)
                .ToList();
        }
    }
}
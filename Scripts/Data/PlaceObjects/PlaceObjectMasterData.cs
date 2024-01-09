using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1week202312.PlaceObjects
{
    [CreateAssetMenu(menuName = "PlaceObjectMasterData", fileName = "PlaceObjectMasterData_000")]
    public class PlaceObjectMasterData : ScriptableObject, IConditionalPlaceObject
    {
        public PlaceObjectId Id => new(_id);
        public IEnumerable<PlaceObjectMasterData> ConditionalTargets => Array.Empty<PlaceObjectMasterData>();
        public GameObject Prefab => _prefab;
        public Genre Genre => _genre;
        public int Point => _point;

        [SerializeField]
        private int _id;

        [SerializeField]
        private Genre _genre;

        [SerializeField]
        private int _point = 1;

        [SerializeField]
        private GameObject _prefab;

    }
}
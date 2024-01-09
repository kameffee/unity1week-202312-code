using System;
using System.Collections.Generic;
using Unity1week202312.PlaceObjects;
using UnityEngine;

namespace Unity1week202312.Stage
{
    [Serializable]
    public class ConditionalPlaceObject : IConditionalPlaceObject
    {
        public PlaceObjectId Id => _placeObjectMasterData.Id;
        public IEnumerable<PlaceObjectMasterData> ConditionalTargets => _conditionalTargets;

        [SerializeField]
        private PlaceObjectMasterData _placeObjectMasterData;

        [SerializeField]
        private List<PlaceObjectMasterData> _conditionalTargets;

        public ConditionalPlaceObject(
            PlaceObjectMasterData placeObjectMasterData,
            List<PlaceObjectMasterData> conditionalTargets)
        {
            _placeObjectMasterData = placeObjectMasterData;
            _conditionalTargets = conditionalTargets;
        }

        public static ConditionalPlaceObject CreateByNonCondition(PlaceObjectMasterData placeObjectMasterData)
        {
            return new ConditionalPlaceObject(placeObjectMasterData, new List<PlaceObjectMasterData>());
        }
    }
}
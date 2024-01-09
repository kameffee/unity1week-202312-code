using Unity1week202312.PlaceObjects;
using UnityEngine;

namespace Unity1week202312.PlaceBoard
{
    public class PackedPlaceObject
    {
        public PlaceObjectId Id { get; }
        public PlaceObject PlaceObject { get; }
        public Vector2Int Position { get; }

        public PackedPlaceObject(PlaceObject placeObject, Vector2Int position)
        {
            Id = placeObject.Id;
            PlaceObject = placeObject;
            Position = position;
        }
    }
}
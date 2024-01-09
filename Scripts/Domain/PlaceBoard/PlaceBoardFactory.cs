using UnityEngine;

namespace Unity1week202312.PlaceBoard
{
    public class PlaceBoardFactory
    {
        public PlaceableMap Create(Vector2Int size)
        {
            return new PlaceableMap(size);
        }
    }
}
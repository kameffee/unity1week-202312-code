using UnityEngine;

namespace Unity1week202312
{
    public readonly struct PlaceStatus
    {
        public Vector2Int Position { get; }
        public bool IsPlaced { get; }

        public PlaceStatus(Vector2Int position, bool isPlaced)
        {
            Position = position;
            IsPlaced = isPlaced;
        }
    }
}
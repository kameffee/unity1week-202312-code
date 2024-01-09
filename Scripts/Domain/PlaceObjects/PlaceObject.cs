using System.Linq;
using UnityEngine;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceObject
    {
        public PlaceObjectId Id { get; }
        public GameObject Prefab { get; }
        public CellSizeMasterData CellSize { get; }

        public PlaceObject(PlaceObjectId id, GameObject prefab, CellSizeMasterData cellSize)
        {
            Id = id;
            Prefab = prefab;
            CellSize = cellSize;
        }

        public Vector2Int[] GetPositions(Vector2Int originPosition)
        {
            var positions = CellSize.GetCellPositions();
            return positions.Select(pos => originPosition + pos).ToArray();
        }
    }
}
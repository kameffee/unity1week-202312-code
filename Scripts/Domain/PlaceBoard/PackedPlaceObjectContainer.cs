using System.Collections.Generic;
using Unity1week202312.PlaceObjects;
using UnityEngine;

namespace Unity1week202312.PlaceBoard
{
    /// <summary>
    /// 配置済みのオブジェクトコンテナ
    /// </summary>
    public class PackedPlaceObjectContainer
    {
        private readonly List<PackedPlaceObject> _packedPlaceObjects = new();

        public void Add(Vector2Int originPosition, PlaceObject placeObject)
        {
            _packedPlaceObjects.Add(new PackedPlaceObject(placeObject, originPosition));
        }

        public IReadOnlyList<PackedPlaceObject> GetAll() => _packedPlaceObjects;

        public void Clear() => _packedPlaceObjects.Clear();
    }
}
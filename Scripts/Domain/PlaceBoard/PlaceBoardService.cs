using System;
using System.Collections.Generic;
using UniRx;
using Unity1week202312.PlaceObjects;
using UnityEngine;

namespace Unity1week202312.PlaceBoard
{
    public class PlaceBoardService
    {
        public IReadOnlyReactiveProperty<PlaceableMap> CurrentPlaceableMap => _currentPlaceableMap;
        public IReadOnlyList<PackedPlaceObject> PackedPlaceObjects => _packedPlaceObjectContainer.GetAll();

        private readonly ReactiveProperty<PlaceableMap> _currentPlaceableMap = new();
        private readonly Subject<Unit> _onClearSubject = new();
        
        private readonly PackedPlaceObjectContainer _packedPlaceObjectContainer = new();

        public void SetMap(PlaceableMap placeableMap) => _currentPlaceableMap.Value = placeableMap;

        public PlaceableMap GetMap() => _currentPlaceableMap.Value;

        public void AddPlaceObject(Vector2Int originPosition, PlaceObject placeObject)
        {
            _packedPlaceObjectContainer.Add(originPosition, placeObject);
        }

        public void ClearPlacedObjects()
        {
            _packedPlaceObjectContainer.Clear();
            _onClearSubject.OnNext(Unit.Default);
        }

        public IObservable<Unit> OnClearAsObservable() => _onClearSubject;

        public bool IsFull() => _currentPlaceableMap.Value.IsFull();
    }
}
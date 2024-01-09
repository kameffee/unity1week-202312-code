using System;
using System.Linq;
using UniRx;
using Unity1week202312.PlaceBoard;
using Unity1week202312.PlaceObjects;
using UnityEngine;

namespace Unity1week202312
{
    public class PlaceUseCase
    {
        private readonly PlaceBoardService _placeBoardService;
        private readonly Subject<PlaceStatus[]> _onChangePlaceStatusAsObservable = new();
        private readonly Subject<Unit> _onFullPacking = new();

        public PlaceUseCase(PlaceBoardService placeBoardService)
        {
            _placeBoardService = placeBoardService;
        }


        public IObservable<PlaceStatus[]> OnChangePlaceStatusAsObservable() => _onChangePlaceStatusAsObservable;

        public IObservable<Unit> OnFullPackingAsObservable() => _onFullPacking;

        public bool IsFullPacking() => _placeBoardService.IsFull();

        public void Place(Vector2Int originPosition, PlaceObject placeObject)
        {
            var positions = placeObject.GetPositions(originPosition);
            Place(positions);

            _placeBoardService.AddPlaceObject(originPosition, placeObject);

            if (_placeBoardService.IsFull())
            {
                _onFullPacking.OnNext(Unit.Default);
            }
        }

        private void Place(params Vector2Int[] positions)
        {
            var placeBoard = _placeBoardService.GetMap();
            if (!placeBoard.CanPlace(positions))
                return;

            placeBoard.Place(positions);

            // 通知
            var placeStatus = positions
                .Select(position => new PlaceStatus(position, true))
                .ToArray();
            _onChangePlaceStatusAsObservable.OnNext(placeStatus);
        }

        public void Remove(params Vector2Int[] positions)
        {
            _placeBoardService.GetMap().Remove(positions);

            // 通知
            var placeStatus = positions
                .Select(position => new PlaceStatus(position, false))
                .ToArray();
            _onChangePlaceStatusAsObservable.OnNext(placeStatus);
        }

        public bool CanPlace(Vector2Int originPosition, PlaceObject placeObject)
        {
            var positions = placeObject.GetPositions(originPosition);
            var placeableMap = _placeBoardService.GetMap();
            return placeableMap.CanPlace(positions);
        }

        public bool CanPlace(Vector2Int originPosition, params Vector2Int[] relativePositions)
        {
            var array = relativePositions.Select(pos => originPosition + pos).ToArray();
            var placeableMap = _placeBoardService.GetMap();

            return placeableMap.CanPlace(array);
        }

        public IObservable<Unit> OnClearAsObservable()
        {
            return _placeBoardService.OnClearAsObservable();
        }
    }
}
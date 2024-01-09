using System;
using System.Collections.Generic;

namespace Unity1week202312.PlaceObjects
{
    public class LotteryHistory
    {
        public IReadOnlyList<PlaceObjectId> All => _history;

        private readonly List<PlaceObjectId> _history = new();

        public void Add(PlaceObjectId id)
        {
            _history.Add(id);
        }

        public bool Contains(PlaceObjectId id)
        {
            return _history.Contains(id);
        }

        public void Clear()
        {
            _history.Clear();
        }

        public void Remove(PlaceObjectId id)
        {
            _history.Remove(id);
        }
    }

    public class NextPlaceObjectUseCase
    {
        private readonly StageService _stageService;
        private readonly PlaceableObjectContainer _placeableObjectContainer;
        private readonly PlaceObjectFactory _placeObjectFactory;
        private readonly PlaceObjectLotteryer _placeObjectLotteryer;
        private readonly LotteryHistory _lotteryHistory = new();

        public NextPlaceObjectUseCase(
            StageService stageService,
            PlaceableObjectContainer placeableObjectContainer,
            PlaceObjectFactory placeObjectFactory,
            PlaceObjectLotteryer placeObjectLotteryer)
        {
            _stageService = stageService;
            _placeableObjectContainer = placeableObjectContainer;
            _placeObjectFactory = placeObjectFactory;
            _placeObjectLotteryer = placeObjectLotteryer;
        }

        public IObservable<(int holderIndex, PlaceObject placeObject)> OnSetAsObservable()
        {
            return _placeableObjectContainer.OnSetAsObservable();
        }

        public IObservable<(int HolderIndex, PlaceObject PlaceObject)> OnRemoveAsObservable()
        {
            return _placeableObjectContainer.OnRemoveAsObservable();
        }

        public void SetNext(int index)
        {
            var currentStageId = _stageService.CurrentStageId;

            if (!_placeObjectLotteryer.TryLottery(currentStageId, ignoreIds: _lotteryHistory.All, out var id)) return;

            Return(index);

            var placeObject = _placeObjectFactory.Create(id);
            _placeableObjectContainer.Set(index, placeObject);

            _lotteryHistory.Add(id);
        }

        public void SetAll()
        {
            RemoveAll();

            var currentStageId = _stageService.CurrentStageId;

            for (var index = 0; index < _placeableObjectContainer.Size; index++)
            {
                if (!_placeObjectLotteryer.TryLottery(currentStageId, ignoreIds: _lotteryHistory.All, out var id))
                {
                    return;
                }

                var placeObject = _placeObjectFactory.Create(id);
                _placeableObjectContainer.Remove(index);
                _placeableObjectContainer.Set(index, placeObject);

                _lotteryHistory.Add(id);
            }
        }

        public PlaceObject Get(int index)
        {
            return _placeableObjectContainer.Get(index);
        }

        private bool TryGet(int index, out PlaceObject result)
        {
            return _placeableObjectContainer.TryGet(index, out result);
        }

        public void Return(int index)
        {
            if (TryGet(index, out var placeObject))
            {
                _lotteryHistory.Remove(placeObject.Id);
                _placeableObjectContainer.Remove(index);
            }
        }

        public void Submit(int index)
        {
            _placeableObjectContainer.Remove(index);
        }

        private void RemoveAll()
        {
            for (var index = 0; index < _placeableObjectContainer.Size; index++)
            {
                Return(index);
            }
        }

        public void Reset()
        {
            _lotteryHistory.Clear();
        }
    }
}
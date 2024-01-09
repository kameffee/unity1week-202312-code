using System.Collections.Generic;
using Kameffee.Extensions;
using Unity1week202312.PlaceObjects;
using UnityEngine;
using UniRx;
using VContainer.Unity;

namespace Unity1week202312.PlaceBoard
{
    public class PlaceBoardPresenter : Presenter, IInitializable
    {
        private readonly Transform _placedObjectViewParent;
        private readonly PlaceUseCase _placeUseCase;
        private readonly List<PlaceObjectView> _placedObjectViews = new();

        public PlaceBoardPresenter(Transform placedObjectViewParent, PlaceUseCase placeUseCase)
        {
            _placedObjectViewParent = placedObjectViewParent;
            _placeUseCase = placeUseCase;
        }

        public void Initialize()
        {
            _placeUseCase.OnClearAsObservable()
                .Subscribe(_ => Clear())
                .AddTo(this);
        }

        public void Clear()
        {
            foreach (var placedObjectView in _placedObjectViews)
            {
                Object.Destroy(placedObjectView.gameObject);
            }

            _placedObjectViews.Clear();
        }

        public void Add(PlaceObjectView placeObjectView, Vector3 position)
        {
            // GridCellViewから座標を取得する
            var placedObject = Object.Instantiate(
                placeObjectView,
                position,
                Quaternion.identity,
                _placedObjectViewParent);
            _placedObjectViews.Add(placedObject);
        }
    }
}
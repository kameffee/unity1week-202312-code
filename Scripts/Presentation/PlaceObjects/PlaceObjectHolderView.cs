using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Unity1week202312.PlaceObjects
{
    /// <summary>
    /// 配置可能オブジェクトの表示
    /// </summary>
    public class PlaceObjectHolderView : MonoBehaviour
    {
        public bool IsShow { get; private set; }

        [SerializeField]
        private List<PlaceObjectHolderElementView> _holderElementViews = new();

        private readonly Subject<int> _onSelectedElement = new();
        private readonly List<PlaceObjectView> _placeObjectViews = new();

        private void Awake()
        {
            foreach (var placeObjectHolderElementView in _holderElementViews)
            {
                placeObjectHolderElementView.OnClickAsObservable()
                    .Where(_ => !placeObjectHolderElementView.IsSelected)
                    .Subscribe(index => _onSelectedElement.OnNext(index))
                    .AddTo(this);
            }
        }

        public void Set(int index, PlaceObjectView placeObjectView)
        {
            var view = _holderElementViews.First(view => view.Index == index);
            _placeObjectViews.Add(placeObjectView);
            view.Set(placeObjectView);
        }

        public void Remove(int index)
        {
            var view = _holderElementViews.First(view => view.Index == index);
            _placeObjectViews.Remove(view.Element);
            view.Remove();
        }

        public PlaceObjectView Get(int index)
        {
            var view = _holderElementViews.First(view => view.Index == index);
            return view.Element;
        }

        public void Select(int index)
        {
            var view = _holderElementViews.First(view => view.Index == index);
            view.Select();
        }

        public void Deselect(int index)
        {
            var view = _holderElementViews.First(view => view.Index == index);
            view.Deselect();
        }

        public void SetAvailableAll(bool isAvailable)
        {
            foreach (var view in _holderElementViews)
            {
                view.SetAvailable(isAvailable);
            }
        }

        public IObservable<int> OnSelectAsObservable() => _onSelectedElement;

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            IsShow = true;

            const float interval = 0.16f;

            static async UniTask ShowHolderAsync(int index, PlaceObjectHolderElementView holderView,
                CancellationToken cancellationToken)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(interval * index), cancellationToken: cancellationToken);
                await holderView.ShowAsync(cancellationToken);
            }

            var showTask = _holderElementViews
                .Select((view, index) => ShowHolderAsync(index, view, cancellationToken));
            await UniTask.WhenAll(showTask);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            IsShow = false;

            const float interval = 0.16f;

            static async UniTask HideHolderAsync(int index, PlaceObjectHolderElementView holderView,
                CancellationToken cancellationToken)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(interval * index), cancellationToken: cancellationToken);
                await holderView.HideAsync(cancellationToken);
            }

            var showTask = _holderElementViews
                .Select((view, index) => HideHolderAsync(index, view, cancellationToken));
            await UniTask.WhenAll(showTask);
        }
    }
}